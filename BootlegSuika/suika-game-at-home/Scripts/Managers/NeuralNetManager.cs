using Godot;
using System;

public partial class NeuralNetManager : Node
{
    // Neural Net scripts
    private static GDScript GAScript = GD.Load<GDScript>("res://Addons/Neat/NEAT_code/ga.gd");
    private GodotObject GA;

    // Neural Net method names (TODO: Get rid of some of these functions that may not be used)

    // (input num, output num, body path, use gui?, param name?) -> void
    private static readonly StringName Init = "_init";
    // () -> Array
    private static readonly StringName CreateInitialPopulation = "create_initial_population";
    // () -> void
    private static readonly StringName EvaluateGeneration = "evaluate_generation";
    // () -> void
    private static readonly StringName NextGeneration = "next_generation";
    // (Genome) -> Species
    private static readonly StringName FindSpecies = "find_species";
    // () -> void
    private static readonly StringName MakeNewSpecies = "make_new_species";
    // () -> void
    private static readonly StringName NextTimestep = "next_timestep";
    // () -> void
    private static readonly StringName FinishCurrentAgents = "finish_current_agents";
    // () -> Array
    private static readonly StringName UpdateCurrentSpecies = "update_curr_species";
    // (num of hybrids to spawn) -> Array
    private static readonly StringName MakeHybrids = "make_hybrids";
    // (species1, species2) -> bool
    private static readonly StringName SortBySpecFitness = "sort_by_spec_fitness";
    // () -> Array
    private static readonly StringName GetCurrentBodies = "get_curr_bodies";
    // () -> void
    private static readonly StringName PrintStatus = "print_status";
    // (index) -> void
    private static readonly StringName UpdateVisibility = "update_visibility";

    // Components
    private Node2D spawnPoint;

    public override void _Ready()
    {
        spawnPoint = GetNode<Node2D>("SpawnPoint");

        // Init genetic algorithm (TODO: Create AI scene that's identical to player)
        GA = (GodotObject) GAScript.New(AI.NeuralNetInputCount, AI.NeuralNetOutputCount,
                "res://Scenes/Prefabs/Player.tscn", false);
        AddChild((Node) GA);
        PlaceBodies((Godot.Collections.Array) GA.Call(GetCurrentBodies));
    }

    private void PlaceBodies(Godot.Collections.Array bodies)
    {
        foreach(Player body in spawnPoint.GetChildren())
        {
            body.QueueFree();
        }
        foreach(Player body in bodies)
        {
            body.Position = new Vector2(540, 56);
            AddChild(body);
        }
    }
}
