using Godot;

public partial class FloatEffect : Node
{
    [Export] private float offsetAmount = 20f;
    [Export] private float speed = 1;
    private Control parent;
    private float baseline;
    private float theta = GD.Randf() * Mathf.Pi * 2; // Offsets from other float effects

    public override void _Ready()
    {
        parent = GetParent<Control>();
        baseline = parent.Position.Y;
    }

    public override void _Process(double delta)
    {
        parent.Position = new Vector2(parent.Position.X, baseline + offsetAmount * Mathf.Sin(theta));
        theta = theta >= 2 * Mathf.Pi ? 0 : theta + (float) delta * speed;
    }
}
