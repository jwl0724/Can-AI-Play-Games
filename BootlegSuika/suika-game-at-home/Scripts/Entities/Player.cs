using Godot;
using System;

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
        if (heldFruit != null) heldFruit.GlobalPosition = fruitPosition.GlobalPosition;
    }

    public void Reset()
    {
        heldFruit = null;
        Position = startPosition;
    }

    public void Move(float direction, double delta)
    {
        Position += Vector2.Right * direction * Speed * (float) delta;
    }

    public void DropHeldFruit()
    {
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
