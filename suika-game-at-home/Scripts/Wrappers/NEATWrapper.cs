using Godot;

// Wrapper class to allow C# integration with gdscript library
public partial class NEATWrapper
{
    // Neural Net scripts
    private static readonly GDScript GAScript = GD.Load<GDScript>("res://Addons/Neat/NEAT_code/ga.gd");
    public readonly Node GA;

    // () -> Array
    private static readonly StringName initPopulationName = "create_initial_population";
    // () -> void
    private static readonly StringName evalName = "evaluate_generation";
    // () -> void
    private static readonly StringName nextGenName = "next_generation";
    // (Genome) -> Species
    private static readonly StringName findSpeciesName = "find_species";
    // () -> void
    private static readonly StringName newSpeciesName = "make_new_species";
    // () -> void
    private static readonly StringName nextTimestepName = "next_timestep";
    // () -> void
    private static readonly StringName finishCurrentName = "finish_current_agents";
    // () -> Array
    private static readonly StringName updateCurrentName = "update_curr_species";
    // (num of hybrids to spawn) -> Array
    private static readonly StringName makeHybridName = "make_hybrids";
    // (species1, species2) -> bool
    private static readonly StringName sortBySpecFitnessName = "sort_by_spec_fitness";
    // () -> Array
    private static readonly StringName getCurrBodiesName = "get_curr_bodies";
    // () -> void
    private static readonly StringName printStatusName = "print_status";
    // (index) -> void
    private static readonly StringName updateVisibilityName = "update_visibility";
    // (string) -> void
    private static readonly StringName saveName = "save_to_json";

    public NEATWrapper(
        int inputCount,
        int outputCount,
        int agentCount,
        string agentPath,
        bool usingGui,
        string customParamName = "Default")
    {
        GA = (Node) GAScript.New(inputCount, outputCount, agentCount, agentPath, usingGui, customParamName);
    }

    public int GetCurrentBest()
    {
        GodotObject genome = (GodotObject) GA.Get("curr_best"); // Genome class, see genome.gd
        return (int) genome.Get("fitness");
    }

    public void SaveNetworkToJson(string configName)
    {
        var genome =  (GodotObject) GA.Get("curr_best");
        var agent = (GodotObject) genome.Get("agent");
        var network = (GodotObject) agent.Get("network");
        network.Call(saveName, configName);
    }

    public Godot.Collections.Array CreateInitialPopulation()
    {
        return (Godot.Collections.Array) GA.Call(initPopulationName);
    }

    public void EvaluateGeneration()
    {
        GA.Call(evalName);
    }

    public void NextGeneration()
    {
        GA.Call(nextGenName);
    }

    public GodotObject FindSpecies(GodotObject genome)
    {
        return (GodotObject) GA.Call(findSpeciesName, genome);
    }

    public void MakeNewSpecies()
    {
        GA.Call(newSpeciesName);
    }

    public void NextTimestep()
    {
        GA.Call(nextTimestepName);
    }

    public void FinishCurrentAgents()
    {
        GA.Call(finishCurrentName);
    }

    public Godot.Collections.Array UpdateCurrentSpecies()
    {
        return (Godot.Collections.Array) GA.Call(updateCurrentName);
    }

    public Godot.Collections.Array MakeHybrids(int numToSpawn)
    {
        return (Godot.Collections.Array) GA.Call(makeHybridName, numToSpawn);
    }

    public bool SortBySpecFitness(GodotObject species1, GodotObject species2)
    {
        return (bool) GA.Call(sortBySpecFitnessName, species1, species2);
    }

    public Godot.Collections.Array GetCurrentBodies()
    {
        return (Godot.Collections.Array) GA.Call(getCurrBodiesName);
    }

    public void PrintStatus()
    {
        GA.Call(printStatusName);
    }

    public void UpdateVisibility(int index)
    {
        GA.Call(updateVisibilityName, index);
    }
}
