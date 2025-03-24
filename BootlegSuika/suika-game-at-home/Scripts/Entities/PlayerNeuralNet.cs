using Godot;
using System;

public partial class Player : Node2D
{
    // Functions required for NEAT integration, follows gdscript naming conventions for easier integration
    [Export] private RayCast2D ray; // Taken from dropper preview UI raycast

    // Information to feed network with
    public Godot.Collections.Array sense() {
        Godot.Collections.Array inputData = new();
        // Next fruit -> type
        inputData.Add((int) GameLoop.Instance.NextFruit);

        // Held fruit -> x, y, fruit type
        if (HeldFruit == null) {
            inputData.Add(0); // Play area is 300 to 780 range, so 0, 0 is impossible -> no fruit held
            inputData.Add(0);
            inputData.Add(-1); // Enum can't be -1 -> no fruit held
        } else {
            inputData.Add(HeldFruit.Position.X);
            inputData.Add(HeldFruit.Position.Y);
            inputData.Add((int) HeldFruit.Type);
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
        if (networkOutput[0] > 0.5f) DropHeldFruit();
        if (networkOutput[1] < -0.5f) Move(-1, GetProcessDeltaTime());
        if (networkOutput[1] > 0.5f) Move(1, GetProcessDeltaTime());
    }

    public int get_fitness() {
        return GameLoop.Instance.Score;
    }

    private Fruit GetTargettedFruit() {
        GodotObject collider = ray.GetCollider();
        if (collider is not Fruit fruit) return null;
        else return fruit;
    }
}
