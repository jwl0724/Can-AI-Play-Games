using Godot;
using System;

public partial class NeuralNetManager : Node
{
    // Constants
    private readonly float timeStepInterval = 0.01f;

    // Components
    [Export] private Node gameScenes; // Node where agents are placed
    private NEATWrapper neat;
    private TrainingManager manager;

    // Running variables
    public bool IsTraining { get; private set; } = false;
    private float timeStepElapsed = 0;

    public override void _Ready()
    {
        manager = Owner as TrainingManager;
        neat = new NEATWrapper(Agent.NeuralNetInputCount, Agent.NeuralNetOutputCount, manager.AgentCount, Agent.Path, false, manager.NetConfigName);
        GD.Print($"--- STARTING TRAINING FOR {neat.GetCurrentBodies().Count} AGENTS ---");
        AddChild(neat.GA);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsTraining) return;

        timeStepElapsed += (float) delta;
        if (timeStepElapsed > timeStepInterval)
        {
            // NextTimeStep is the main loop that controls AI action
            neat.NextTimestep();
            timeStepElapsed = 0;
        }
        if (AllAgentsDead())
        {
            GD.Print("--- ALL AGENTS DEAD - MOVING TO NEXT GENERATION ---");
            manager.NextIteration();
        }
    }

    public void StartTraining()
    {
        Godot.Collections.Array agents = neat.GetCurrentBodies();
        foreach(Agent agent in agents)
        {
            agent.CallDeferred(nameof(agent.DisablePlayerInput));
        }
        PlaceBodies(agents);
        IsTraining = true;
    }

    public void StopTraining()
    {
        IsTraining = false;
        GD.Print("--- TRAINING COMPLETE ---");
        neat.PrintStatus();
        GD.Print($"--- Saving Network As {manager.NetworkName} ---");

        // Saves to C:\Users\YourName\AppData\Roaming\Godot\app_userdata\YourProject\network_configs
        neat.SaveNetworkToJson(manager.NetworkName);
    }

    public int StopCurrentLoop()
    {
        neat.EvaluateGeneration();
        return neat.GetCurrentBest();
    }

    public void StartNextLoop()
    {
        neat.NextGeneration();
        StartTraining();
    }

    private bool AllAgentsDead()
    {
        Godot.Collections.Array agents = neat.GetCurrentBodies();
        foreach(Agent agent in agents)
        {
            if (agent.Playing) return false;
        }
        return true;
    }

    private void PlaceBodies(Godot.Collections.Array bodies)
    {
        foreach(Agent body in gameScenes.GetChildren())
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
