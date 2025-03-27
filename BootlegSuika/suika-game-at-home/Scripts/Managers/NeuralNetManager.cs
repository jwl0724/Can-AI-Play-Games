using Godot;
using System;

public partial class NeuralNetManager : Node
{
    [Export] private Node gameScenes; // Node where agents are placed
    private NEATWrapper neat;
    private TrainingManager manager;

    public override void _Ready()
    {
        neat = new NEATWrapper(Agent.NeuralNetInputCount, Agent.NeuralNetOutputCount, Agent.Path, false);
        manager = Owner as TrainingManager;
    }

    public void StartTraining(TrainingManager manager)
    {
        Godot.Collections.Array agents = neat.GetCurrentBodies();
        foreach(Agent agent in agents)
        {
            agent.Connect(Agent.SignalName.ScoreChange, Callable.From(() => manager.ExtendTimer()));
            agent.CallDeferred(nameof(agent.DisablePlayerInput));
        }
        PlaceBodies(agents);
    }

    public void StartNextGeneration()
    {

    }

    private void PlaceBodies(Godot.Collections.Array bodies)
    {
        foreach(Player body in gameScenes.GetChildren())
        {
            body.QueueFree();
        }
        for (int i = 0; i < bodies.Count; i++)
        {
            Agent body = (Agent) bodies[i];
            body.Position = new Vector2(manager.AgentWidth * i, 0);
            body.SetBorders(
                (int) body.BoxBorders.X + manager.AgentWidth * i,
                (int) body.BoxBorders.Y + manager.AgentWidth * i
            );
            gameScenes.AddChild(body);
        }
    }
}
