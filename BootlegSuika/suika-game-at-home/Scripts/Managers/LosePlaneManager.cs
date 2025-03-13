using Godot;
using System;
using System.Linq;

public partial class LosePlaneManager : Node2D
{
    public override void _Ready()
    {
        foreach(Area2D plane in GetChildren().Cast<Area2D>())
        {
            plane.Connect(Area2D.SignalName.BodyEntered, new Callable(this, nameof(OnBodyEntered)));
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Fruit fruit || !GameLoop.Instance.Playing) return;
        fruit.InBounds = false;
        GameLoop.Instance.EndGame();
    }
}
