using Godot;
using System;

public partial class TrainingManager : Node
{
    // Training parameters
    [Export] public string NetworkName { get; private set; } = "GamerAI"; // Saved network name
    [Export] private int trainingLoops = 300;
    [Export] private int timeLimit = 10;
    [Export] private int rewardAmount = 5; // Amount of time given when improved
    [Export] private int fitnessThreshold = 4000; // Score in which to stop training, use int.MaxValue for no limit
    // Found in C://Users/YourName/AppData/Roaming/Godot/app_userdata/Suika Game at Home/network_configs -> name should match config file to load without extension
    [Export] public string NetConfigName = "Default";

    // Parameters used in library
    [Export] public int AgentCount = 300;

    // Components
    private Timer timer;
    private Node gameScenes; // Node to instantiate the multiple game scenes
    private NeuralNetManager neuralNet;
    private Camera2D camera;

    // Running variables
    public int CurrentGeneration { get; private set; } = 0;
    public int AllTimeBest { get; private set; } = 0;
    private float elapsedTime = 0;
    private int iterations = 0;

    // Constant variables
    public int AgentWidth => (int) camera.GetViewportRect().Size.X;

    public override void _Ready()
    {
        neuralNet = GetNode<NeuralNetManager>("NeuralNetManager");
        timer = GetNode<Timer>("IterationTimer");
        camera = GetNode<Camera2D>("Camera");

        timer.WaitTime = timeLimit;
        timer.Connect(Timer.SignalName.Timeout, new Callable(this, nameof(NextIteration)));

        neuralNet.StartTraining();
        timer.Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        elapsedTime += (float) delta;
    }

    public void NextIteration()
    {
        int bestInGen = neuralNet.StopCurrentLoop();
        bool newBest = bestInGen > AllTimeBest;
        AllTimeBest = newBest ? bestInGen : AllTimeBest;
        if (iterations > trainingLoops || bestInGen > fitnessThreshold)
        {
            EndTraining();
            return;
        }
        iterations++;
        if (newBest) timeLimit += rewardAmount; // Increase time limit if improvement was made
        elapsedTime = 0;
        neuralNet.StartNextLoop();
    }

    private void EndTraining()
    {
        timer.Stop();
        neuralNet.StopTraining();
        GetTree().Quit(); // Closes game when threshold is hit
    }
}
