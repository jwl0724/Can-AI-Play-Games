using Godot;
using System;

public partial class InputManager : Node
{
	// Either manual input or AI input (from neural net integration later)
	public enum InputMode{ MANUAL, AI }

 	// Components
	public InputMode Mode = InputMode.MANUAL;
	private Player player;

	public override void _Ready()
	{
		player = GetOwner<Player>();
	}

	public override void _Process(double delta)
	{
		if (Mode == InputMode.MANUAL) ProcessManualMode(delta);
		else ProcessAIMode(delta);
	}

	private void ProcessManualMode(double delta)
	{
		float moveDirection = 0;
		if (Input.IsActionPressed("Left")) moveDirection -= 1;
		if (Input.IsActionPressed("Right")) moveDirection += 1;
		player.Move(moveDirection, delta);

		if (Input.IsActionJustPressed("Drop")) player.DropHeldFruit();
	}

	private void ProcessAIMode(double delta)
	{
		// TODO: Implement way later after a good idea of how AI is supposed to integrate with game
	}
}
