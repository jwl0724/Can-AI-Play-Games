using Godot;
using System;

// Each agent needs to be the entire game scene (no shared map)
public partial class Agent : GameLoop
{
    public static readonly string Path = "res://Scenes/Prefabs/Agent.tscn";
    public static readonly short NeuralNetInputCount = 7;
    public static readonly short NeuralNetOutputCount = 2;
    [Signal] public delegate void deathEventHandler(); // Required signal for neural net (has to be lowercase)
    [Export] private RayCast2D ray; // Taken from dropper preview UI raycast

    // Functions required for NEAT integration, follows gdscript naming conventions for easier integration
    // Information to feed network with
    public Godot.Collections.Array sense() {
        Godot.Collections.Array inputData = new();
        // Next fruit -> type
        inputData.Add((int) (Owner as GameLoop).NextFruit);

        // Held fruit -> x, y, fruit type
        if (Player.HeldFruit == null) {
            inputData.Add(0); // Play area is 300 to 780 range, so 0, 0 is impossible -> no fruit held
            inputData.Add(0);
            inputData.Add(-1); // Enum can't be -1 -> no fruit held
        } else {
            inputData.Add(Player.HeldFruit.Position.X);
            inputData.Add(Player.HeldFruit.Position.Y);
            inputData.Add((int) Player.HeldFruit.Type);
        }
        // Targetted fruit -> x, y, fruit type
        Fruit targettedFruit = GetTargettedFruit();
        if (targettedFruit == null) {
            inputData.Add(0);
            inputData.Add(0);
            inputData.Add(-1);
        } else {
            inputData.Add(targettedFruit.Position.X);
            inputData.Add(targettedFruit.Position.Y);
            inputData.Add((int) targettedFruit.Type);
        }

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

    private Fruit GetTargettedFruit() {
        GodotObject collider = ray.GetCollider();
        if (collider is not Fruit fruit) return null;
        else return fruit;
    }

    // SPECIAL METHODS OF AGENT
	public void DisablePlayerInput()
	{
        Player.DisablePlayerInput(true);
	}

    // METHOD OVERRIDES SECTION
    public override void EndGame()
    {
        base.EndGame();
        EmitSignal(SignalName.death);
    }
}
