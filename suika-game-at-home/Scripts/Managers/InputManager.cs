using Godot;
using System;

public partial class InputManager : Node
{
 	// Components
	private Player player;

	public override void _Ready()
	{
		player = GetOwner<Player>();
	}

	public override void _Process(double delta)
	{
		if (player.InputDisabled) return;
		ProcessManualMode(delta);
	}

	private void ProcessManualMode(double delta)
	{
		float moveDirection = 0;
		if (Input.IsActionPressed("Left")) moveDirection -= 1;
		if (Input.IsActionPressed("Right")) moveDirection += 1;
		player.Move(moveDirection, delta);

		if (Input.IsActionJustPressed("Drop")) player.DropHeldFruit();
		if (Input.IsActionJustPressed("Pause")) player.EmitSignal(Player.SignalName.PausePressed);
	}
}
