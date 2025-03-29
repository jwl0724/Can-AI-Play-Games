using Godot;
using System;

// Each agent needs to be the entire game scene (no shared map)
public partial class Agent : GameLoop
{
    // Components for library
    public static readonly string Path = "res://Scenes/Prefabs/Agent.tscn";
    public static readonly short NeuralNetInputCount = 6; // 13
    public static readonly short NeuralNetOutputCount = 2;
    [Signal] public delegate void deathEventHandler(); // Required signal for neural net (has to be lowercase)
    [Export] private RayCast2D ray; // Taken from dropper preview UI raycast

    // Running variables
    private float lifespan = 10;
    private float timeAlive = 0;
    private float age = 0;
    private bool InTrainingMode = false;

    // Functions required for NEAT integration, follows gdscript naming conventions for easier integration
    // Information to feed network with
    public Godot.Collections.Array sense() {
        Godot.Collections.Array inputData = new();
        // Next fruit -> type
        inputData.Add((int) NextFruit);

        // Held fruit -> x, y, fruit type
        inputData.Add(Player.Position.X);
        // AppendWithNullCheck(inputData, Player.HeldFruit);

        // Targetted fruit -> x, y, fruit type
        Fruit targettedFruit = GetTargettedFruit();
        if (targettedFruit != null) inputData.Add((int) targettedFruit.Type);
        else inputData.Add(0); // Values are out of bounds of play area
        // AppendWithNullCheck(inputData, targettedFruit);

        if (Player.HeldFruit == null)
        {
            float distanceFromTarget = ray.GetCollisionPoint().DistanceTo(Player.GetFruitSpawnPoint());
            inputData.Add(distanceFromTarget);
        } else inputData.Add(-1000); // Some impossible value

        float distanceFromLeft = Player.GlobalPosition.X - BoxBorders.X;
        float distanceFromRight = BoxBorders.Y - Player.GlobalPosition.X;
        inputData.Add(distanceFromLeft);
        inputData.Add(distanceFromRight);

        // // First same type fruit as held -> x, y, fruit type
        // Fruit firstSameTypeHeld = null;
        // foreach(Fruit fruit in FruitContainer.GetChildren())
        // {
        //     if (fruit.Type == Player.HeldFruit?.Type)
        //     {
        //         firstSameTypeHeld = fruit;
        //         break;
        //     }
        // }
        // AppendWithNullCheck(inputData, firstSameTypeHeld);

        // // First same type fruit as next -> x, y, fruit type
        // Fruit firstSameTypeNext = null;
        // foreach(Fruit fruit in FruitContainer.GetChildren())
        // {
        //     if (fruit.Type == NextFruit)
        //     {
        //         firstSameTypeNext = fruit;
        //         break;
        //     }
        // }
        // AppendWithNullCheck(inputData, firstSameTypeNext);
        return inputData;
    }

    // Output action to act with
    public void act(Godot.Collections.Array<float> networkOutput) {
        if (networkOutput[0] > -0.5) Player.DropHeldFruit();

        if (Player.HeldFruit == null) return;
        float moveCoefficient = networkOutput[1];

        float offset = Player.GlobalPosition.X - Player.GetFruitSpawnPoint().X;
        float boxWidth = BoxBorders.Y - BoxBorders.X;
        float midpoint = boxWidth / 2 + BoxBorders.X;
        float newX = midpoint + boxWidth / 2 * moveCoefficient;

        if (newX <= midpoint) newX += Player.HeldFruit.Radius;
        else newX -= Player.HeldFruit.Radius;

        Player.GlobalPosition = new Vector2(newX + offset, Player.GlobalPosition.Y);
    }

    public float get_fitness() {
        return Score * timeAlive / 60;
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
            // data.Add(0);
            data.Add(-1);
        }
        else
        {
            data.Add(fruit.Position.X);
            // data.Add(fruit.Position.Y);
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
        Connect(SignalName.ScoreChange, new Callable(this, nameof(Reward)));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!InTrainingMode) return;
        // Put time limit to force evolution instead of passive play
        age += (float) delta;
        timeAlive += (float) delta;
        if (age > lifespan)
        {
            EndGame();
            return;
        }
    }

    public override void EndGame()
    {
        Playing = false;
        Visible = false;
        InTrainingMode = false;
        EmitSignal(SignalName.GameOver, Score);
        Score /= 1000; // Make fitness very low if agent died
        EmitSignal(SignalName.death);
    }

    // Reward function for scoring
    private void Reward(int score)
    {
        float timerReduction = score / 2;
        age = age - timerReduction > 0 ? timerReduction : 0;
    }
}
