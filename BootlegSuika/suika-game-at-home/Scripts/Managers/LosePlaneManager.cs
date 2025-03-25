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
        GameLoop game = Owner as GameLoop;
        if (body is not Fruit fruit || game.Playing) return;
        fruit.InBounds = false;
        game.EndGame();
    }
}
