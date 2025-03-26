using Godot;
using System;

public partial class NeuralNetManager : Node
{
    [Export] private Node gameScenes; // Node where agents are placed
    private NEATWrapper neat;
    public override void _Ready()
    {
        neat = new NEATWrapper(Agent.NeuralNetInputCount, Agent.NeuralNetOutputCount, Agent.Path, false);
    }

    public void StartTraining(TrainingManager manager)
    {
        Godot.Collections.Array agents = neat.GetCurrentBodies();
        foreach(Agent agent in agents)
        {
            agent.Connect(Agent.SignalName.ScoreChange, Callable.From(() => manager.ExtendTimer()));
        }
        PlaceBodies(agents);
    }

    public void StartNextGeneration()
    {

    }

    // TODO: Place each scene separate from one another
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
