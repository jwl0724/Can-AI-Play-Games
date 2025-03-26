using Godot;
using System;

public partial class TrainingManager : Node
{
    // Training parameters
    [Export] private int trainingLoops = 300;
    [Export] private int timeLimitPerGeneration = 20;
    [Export] private int maxAllowedTimePerGeneration = 3 * 60;
    [Export] private string pretrainedData; // TODO: Incorporate this later on

    // Components
    private Timer timer;
    private Node gameScenes; // Node to instantiate the multiple game scenes
    private NeuralNetManager neuralNet;

    // Running variables
    public int CurrentGeneration { get; private set; } = 0;
    public int BestScoreInGeneration { get; private set; } = 0;
    private float totalGenerationTime = 0;

    public override void _Ready()
    {
        neuralNet = GetNode<NeuralNetManager>("NeuralNetManager");
        timer = GetNode<Timer>("IterationTimer");

        timer.WaitTime = timeLimitPerGeneration;
        timer.Connect(Timer.SignalName.Timeout, Callable.From(() => {
            totalGenerationTime = 0;
            neuralNet.StartNextGeneration();
        }));
        neuralNet.StartTraining(this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        totalGenerationTime += (float) delta;
    }

    public void ExtendTimer()
    {
        if (totalGenerationTime >= maxAllowedTimePerGeneration) return;
        timer.Stop();
        timer.Start();
    }
}
