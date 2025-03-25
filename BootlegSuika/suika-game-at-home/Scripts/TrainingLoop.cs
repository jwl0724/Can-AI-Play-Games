using Godot;
using System;

public partial class TrainingLoop : Node
{
    // Training parameters
    [Export] private int trainingLoops = 300;
    [Export] private int timeLimitPerGeneration = 20;
    [Export] private string pretrainedData; // TODO: Incorporate this later on

    // Components
    private Timer timer;
    private NeuralNetManager neuralNet;

    // Running variables
    public int CurrentGeneration { get; private set; }= 0;
    public int BestScoreInGeneration { get; private set; }= 0;

    public override void _Ready()
    {
        neuralNet = GetNode<NeuralNetManager>("NeuralNetManager");

        timer = GetNode<Timer>("IterationTimer");
        timer.WaitTime = timeLimitPerGeneration;
        // timer.Connect(Timer.SignalName.Timeout, )
    }


}
