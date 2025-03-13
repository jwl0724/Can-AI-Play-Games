using Godot;

public partial class DropperPreviewUI : Line2D
{
	private RayCast2D ray;
	private Player player;

	public override void _Ready()
	{
		player = GetOwner<Player>();
		ray = GetNode<RayCast2D>("Raycast");
	}

	public override void _PhysicsProcess(double delta)
	{
		Visible = false; // Hide the line by default
		if (!GameLoop.Instance.Playing || !player.IsHoldingFruit) return;
		CreateLine();
	}

	private void CreateLine()
	{
		if (!ray.IsColliding()) return; // Theoretically should never run
		Visible = true;
		Points = new Vector2[]{ Vector2.Zero, new(0, ray.GetCollisionPoint().Y - GlobalPosition.Y) };
	}
}
