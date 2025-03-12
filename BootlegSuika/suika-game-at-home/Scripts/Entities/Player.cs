using Godot;
using System;

public partial class Player : Node2D
{
    // Components
    private Node2D fruitPosition;

    // Signals
    [Signal] public delegate void DropFruitEventHandler();

    // Running variables
    private Fruit heldFruit = null;

    public override void _Ready()
    {
        fruitPosition = GetNode<Node2D>("FruitPosition");
    }

    public override void _Process(double delta)
    {
        if (heldFruit != null) heldFruit.GlobalPosition = fruitPosition.GlobalPosition;
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
