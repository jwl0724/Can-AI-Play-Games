using Godot;
using System;

// Each agent needs to be the entire game scene (no shared map)
public partial class Agent : GameLoop
{
    // Components for library
    public static readonly string Path = "res://Scenes/Prefabs/Agent.tscn";
    public static readonly short NeuralNetInputCount = 13;
    public static readonly short NeuralNetOutputCount = 2;
    [Signal] public delegate void deathEventHandler(); // Required signal for neural net (has to be lowercase)
    [Export] private RayCast2D ray; // Taken from dropper preview UI raycast

    // Running variables
    private float lifespan = 10;
    private float age = 0;
    private bool InTrainingMode = false;

    // Functions required for NEAT integration, follows gdscript naming conventions for easier integration
    // Information to feed network with
    public Godot.Collections.Array sense() {
        Godot.Collections.Array inputData = new();
        // Next fruit -> type
        inputData.Add((int) NextFruit);

        // Held fruit -> x, y, fruit type
        AppendWithNullCheck(inputData, Player.HeldFruit);

        // Targetted fruit -> x, y, fruit type
        Fruit targettedFruit = GetTargettedFruit();
        AppendWithNullCheck(inputData, targettedFruit);

        // First same type fruit as held -> x, y, fruit type
        Fruit firstSameTypeHeld = null;
        foreach(Fruit fruit in FruitContainer.GetChildren())
        {
            if (fruit.Type == Player.HeldFruit?.Type)
            {
                firstSameTypeHeld = fruit;
                break;
            }
        }
        AppendWithNullCheck(inputData, firstSameTypeHeld);

        // First same type fruit as next -> x, y, fruit type
        Fruit firstSameTypeNext = null;
        foreach(Fruit fruit in FruitContainer.GetChildren())
        {
            if (fruit.Type == NextFruit)
            {
                firstSameTypeNext = fruit;
                break;
            }
        }
        AppendWithNullCheck(inputData, firstSameTypeNext);
        return inputData;
    }

    // Output action to act with
    public void act(Godot.Collections.Array<float> networkOutput) {
        if (networkOutput[0] > 0.5f) Player.DropHeldFruit();
        if (networkOutput[1] < -0.5f) Player.Move(-1, GetProcessDeltaTime());
        if (networkOutput[1] > 0.5f) Player.Move(1, GetProcessDeltaTime());
    }

    public int get_fitness() {
        return Score;
    }

    // HELPER FUNCTIONS SECTION
    private Fruit GetTargettedFruit() {
        GodotObject collider = ray.GetCollider();
        if (collider is not Fruit fruit) return null;
        else return fruit;
    }

    // Data in x, y, type order
    private static void AppendWithNullCheck(Godot.Collections.Array data, Fruit fruit)
    {
        if (fruit == null)
        {
            data.Add(0); // Values are out of bounds of play area
            data.Add(0);
            data.Add(-1);
        }
        else
        {
            data.Add(fruit.Position.X);
            data.Add(fruit.Position.Y);
            data.Add((int) fruit.Type);
        }
    }

    // SPECIAL METHODS OF AGENT
	public void DisablePlayerInput()
	{
        Player.DisablePlayerInput(true);
	}

    public void SetBorders(int lower, int upper)
    {
        BoxBorders = new Vector2(lower, upper);
    }

    // METHOD OVERRIDES SECTION
    public override void _Ready()
    {
        base._Ready();
        if (GetTree().CurrentScene is TrainingManager) InTrainingMode = true;
        Connect(SignalName.ScoreChange, Callable.From((int score) => age = age - score > 0 ? age - score : 0)); // Scoring extends longevity
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!InTrainingMode) return;
        // Put time limit to force evolution instead of passive play
        age += (float) delta;
        if (age > lifespan)
        {
            EndGame();
            return;
        }
    }

    public override void EndGame()
    {
        Playing = false;
        EmitSignal(SignalName.GameOver, Score);
        EmitSignal(SignalName.death);
    }
}
