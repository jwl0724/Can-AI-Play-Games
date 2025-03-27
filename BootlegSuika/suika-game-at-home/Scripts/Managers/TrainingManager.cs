using Godot;
using System;

public partial class TrainingManager : Node
{
    // Training parameters
    [Export] public string NetworkName { get; private set; } = "GamerAI"; // Saved network name
    [Export] private int trainingLoops = 300;
    [Export] private int timeLimitPerGeneration = 20;
    [Export] private int maxAllowedTimePerGeneration = 3 * 60;
    [Export] private int fitnessThreshold = 1000; // Score in which to stop training, use int.MaxValue for no limit

    // Parameters used in library
    public static readonly int AgentCount = 300;

    // Components
    private Timer timer;
    private Node gameScenes; // Node to instantiate the multiple game scenes
    private NeuralNetManager neuralNet;
    private Camera2D camera;

    // Running variables
    public int CurrentGeneration { get; private set; } = 0;
    public int AllTimeBest { get; private set; } = 0;
    private float totalGenerationTime = 0;
    private float iterations = 0;

    // Constant variables
    public int AgentWidth => (int) camera.GetViewportRect().Size.X;

    public override void _Ready()
    {
        neuralNet = GetNode<NeuralNetManager>("NeuralNetManager");
        timer = GetNode<Timer>("IterationTimer");
        camera = GetNode<Camera2D>("Camera");

        timer.WaitTime = timeLimitPerGeneration;
        timer.Connect(Timer.SignalName.Timeout, new Callable(this, nameof(NextIteration)));

        neuralNet.StartTraining();
        timer.Start();
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

    public void NextIteration()
    {
        int bestInGen = neuralNet.StopCurrentLoop();
        AllTimeBest = bestInGen > AllTimeBest ? bestInGen : AllTimeBest;
        if (iterations > trainingLoops || bestInGen > fitnessThreshold)
        {
            EndTraining();
            return;
        }
        iterations++;
        totalGenerationTime = 0;
        neuralNet.StartNextLoop();
    }

    private void EndTraining()
    {
        timer.Stop();
        neuralNet.StopTraining();
        GetTree().Quit(); // Closes game when threshold is hit
    }
}
