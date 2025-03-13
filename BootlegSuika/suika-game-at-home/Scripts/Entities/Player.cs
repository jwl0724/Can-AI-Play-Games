using Godot;
using System;

// TODO: Add a line between fruit position and the closest fruit down
public partial class Player : Node2D
{
    // Constants
    private Vector2 startPosition;

    // Components
    private Node2D fruitPosition;

    // Running variables
    [Export] public float Speed { get; private set; } = 200;

    // Signals
    [Signal] public delegate void DropFruitEventHandler();

    // Running variables
    private Fruit heldFruit = null;

    public override void _Ready()
    {
        fruitPosition = GetNode<Node2D>("FruitPosition");
        startPosition = Position;

        GameLoop.Instance.SetComponent(this);
    }

    public override void _Process(double delta)
    {
        if (heldFruit != null) heldFruit.Position = fruitPosition.GlobalPosition;
    }

    public void Reset()
    {
        heldFruit = null;
        Position = startPosition;
    }

    public void Move(float direction, double delta)
    {
        // TODO: Add box boundaries such that the fruit held can never go outside of the box
        Position += Vector2.Right * direction * Speed * (float) delta;
    }

    public void DropHeldFruit()
    {
        if (heldFruit == null) return;

        heldFruit.Sleeping = false;
        heldFruit.Freeze = false;
        heldFruit = null;
        EmitSignal(SignalName.DropFruit);
    }

    public void GiveFruit(Fruit fruit)
    {
        heldFruit = fruit;
    }

    public Vector2 GetFruitSpawnPoint()
    {
        return fruitPosition.GlobalPosition;
    }
}
