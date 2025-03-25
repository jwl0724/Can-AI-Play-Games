using Godot;
using System;

public partial class Player : Node2D
{
    // Constants
    public Vector2 StartPosition { get; private set; } = new Vector2(540, 56);
    private Vector2 BoxBorders;

    // Components
    protected Node2D fruitPosition;

    // Running variables
    [Export] public float Speed { get; private set; } = 200;
    public bool IsHoldingFruit { get; private set; } = false;
    public bool InputDisabled { get; private set; } = false;
    public bool IsDead { get; private set; } = false;

    // Signals
    [Signal] public delegate void DropFruitEventHandler();

    // Running variables
    public Fruit HeldFruit { get; private set; } = null;

    public override void _Ready()
    {
        fruitPosition = GetNode<Node2D>("FruitPosition");
        StartPosition = Position;

        GameLoop game = Owner as GameLoop;
        game.Connect(GameLoop.SignalName.GameOver, Callable.From((int score) => OnGameOver()));
        BoxBorders = game.BoxBorders;
    }

    public override void _Process(double delta)
    {
        if (IsDead) return;
        if (HeldFruit != null) HeldFruit.Position = fruitPosition.GlobalPosition;
    }

    public void DisablePlayerInput(bool disabled) {
        InputDisabled = disabled;
    }

    public void Reset()
    {
        IsDead = false;
        HeldFruit = null;
        IsHoldingFruit = false;
        Position = StartPosition;
    }

    public void Move(float direction, double delta)
    {
        if (IsDead) return;

        Vector2 component = Vector2.Right * direction * Speed * (float) delta;
        Position += component;

        if (!IsInBounds()) Position -= component; // Undo move if went out of bounds
    }

    public void DropHeldFruit()
    {
        if (HeldFruit == null || IsDead) return;
        HeldFruit.Activate();
        HeldFruit = null;
        IsHoldingFruit = false;
        EmitSignal(SignalName.DropFruit);
    }

    public void GiveFruit(Fruit fruit)
    {
        if (IsDead) return;
        IsHoldingFruit = true;
        HeldFruit = fruit;

        // Shift player back into area if newer fruit is bigger
        float left = fruitPosition.GlobalPosition.X - HeldFruit.Radius;
        float right = fruitPosition.GlobalPosition.X + HeldFruit.Radius;
        float difference = 0;

        if (right > BoxBorders.Y) difference = BoxBorders.Y - right - 2;
        else if (left < BoxBorders.X) difference = BoxBorders.X - left + 2;
        Position += Vector2.Right * difference;
    }

    public Vector2 GetFruitSpawnPoint()
    {
        return fruitPosition.GlobalPosition;
    }

    private void OnGameOver()
    {
        IsDead = true;
        DropHeldFruit();
    }

    private bool IsInBounds()
    {
        if (HeldFruit == null)
            return Position.X > BoxBorders.X && Position.X < BoxBorders.Y + 40;

        float fruitLeftSide = fruitPosition.GlobalPosition.X - HeldFruit.Radius;
        float fruitRightSide = fruitPosition.GlobalPosition.X + HeldFruit.Radius;
        return fruitLeftSide > BoxBorders.X && fruitRightSide < BoxBorders.Y;
    }
}
