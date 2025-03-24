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
    public bool IsHoldingFruit { get; private set; } = false;
    private bool stop = false;

    // Signals
    [Signal] public delegate void DropFruitEventHandler();
    [Signal] public delegate void DeathEventHandler();

    // Running variables
    public Fruit HeldFruit { get; private set; } = null;

    public override void _Ready()
    {
        fruitPosition = GetNode<Node2D>("FruitPosition");
        startPosition = Position;

        GameLoop.Instance.SetComponent(this);
        GameLoop.Instance.Connect(GameLoop.SignalName.GameOver, Callable.From((int score) => OnGameOver()));
    }

    public override void _Process(double delta)
    {
        if (stop) return;
        if (HeldFruit != null) HeldFruit.Position = fruitPosition.GlobalPosition;
    }

    public void Reset()
    {
        stop = false;
        HeldFruit = null;
        IsHoldingFruit = false;
        Position = startPosition;
    }

    public void Move(float direction, double delta)
    {
        if (stop) return;

        Vector2 component = Vector2.Right * direction * Speed * (float) delta;
        Position += component;

        if (!IsInBounds()) Position -= component; // Undo move if went out of bounds
    }

    public void DropHeldFruit()
    {
        if (HeldFruit == null || stop) return;
        HeldFruit.Activate();
        HeldFruit = null;
        IsHoldingFruit = false;
        EmitSignal(SignalName.DropFruit);
    }

    public void GiveFruit(Fruit fruit)
    {
        if (stop) return;
        IsHoldingFruit = true;
        HeldFruit = fruit;

        // Shift player back into area if newer fruit is bigger
        float left = fruitPosition.GlobalPosition.X - HeldFruit.Radius;
        float right = fruitPosition.GlobalPosition.X + HeldFruit.Radius;
        float difference = 0;
        if (right > GameLoop.Instance.BoxBorders.Y) difference = GameLoop.Instance.BoxBorders.Y - right - 2;
        else if (left < GameLoop.Instance.BoxBorders.X) difference = GameLoop.Instance.BoxBorders.X - left + 2;
        Position += Vector2.Right * difference;
    }

    public Vector2 GetFruitSpawnPoint()
    {
        return fruitPosition.GlobalPosition;
    }

    private void OnGameOver()
    {
        stop = true;
        DropHeldFruit();
        EmitSignal(SignalName.Death);
    }

    private bool IsInBounds()
    {
        if (HeldFruit == null)
            return Position.X > GameLoop.Instance.BoxBorders.X && Position.X < GameLoop.Instance.BoxBorders.Y + 40;

        float fruitLeftSide = fruitPosition.GlobalPosition.X - HeldFruit.Radius;
        float fruitRightSide = fruitPosition.GlobalPosition.X + HeldFruit.Radius;
        return fruitLeftSide > GameLoop.Instance.BoxBorders.X && fruitRightSide < GameLoop.Instance.BoxBorders.Y;
    }
}
