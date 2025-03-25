using Godot;
using System;

public partial class NeuralNetManager : Node
{

    public override void _Ready()
    {
        // spawnPoint = GetNode<Node2D>("SpawnPoint");

        // // Init genetic algorithm (TODO: Create AI scene that's identical to player)
        // GA = (GodotObject) GAScript.New(AI.NeuralNetInputCount, AI.NeuralNetOutputCount,
        //         "res://Scenes/Prefabs/Player.tscn", false);
        // AddChild((Node) GA);
        // PlaceBodies((Godot.Collections.Array) GA.Call(getCurrBodiesName));
    }

    public void StartTraining(TrainingManager manager)
    {

    }

    public void StartNextGeneration()
    {

    }

    private void PlaceBodies(Godot.Collections.Array bodies)
    {
    //     foreach(Player body in spawnPoint.GetChildren())
    //     {
    //         body.QueueFree();
    //     }
    //     foreach(Player body in bodies)
    //     {
    //         body.Position = new Vector2(540, 56);
    //         AddChild(body);
    //     }
    }
}
