using Godot;
using System;

public partial class RollEffect : Node
{
    [Export] private float offsetDegrees = 30;
    [Export] private float speed = 1;
    private Control parent;
    private float originalRotation;
    private float theta = GD.Randf() * Mathf.Pi;

    public override void _Ready()
    {
        parent = GetParent<Control>();
        originalRotation = parent.Rotation;
        parent.PivotOffset = new Vector2(parent.Size.X / 2, parent.Size.Y / 2);
    }

    public override void _Process(double delta)
    {
        parent.RotationDegrees = offsetDegrees * Mathf.Sin(theta);
        theta = theta >= 2 * Mathf.Pi ? 0 : theta + (float) delta * speed;
    }
}
