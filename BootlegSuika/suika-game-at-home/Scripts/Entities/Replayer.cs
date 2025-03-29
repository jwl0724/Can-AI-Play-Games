using Godot;
using System;

public partial class Replayer : Agent
{
    // Components
    private NEATConfigLoadWrapper network;
    [Export] private string configFileName = "ScuffedNet";

    // Running variables
    private float elapsedTime = 0;

    public override void _Ready()
    {
        base._Ready();
        network = new NEATConfigLoadWrapper(configFileName);
    }

    public override void _PhysicsProcess(double delta)
    {
        elapsedTime += (float) delta;
        if (elapsedTime > NeuralNetManager.TimeStepInterval)
        {
            Godot.Collections.Array<float> output = network.Update(sense());
            act(output);
            elapsedTime = 0;
        }
    }
}
