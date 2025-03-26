using Godot;
using System;

public partial class TrainingSceneNavigator : Camera2D
{
	private enum Direction { LEFT, RIGHT }

	public override void _Ready()
	{
		Offset = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is not InputEventKey keyEvent) return;
		if (keyEvent.IsActionReleased("Left")) ShowNextAgent(Direction.LEFT);
		if (keyEvent.IsActionReleased("Right")) ShowNextAgent(Direction.RIGHT);
	}

	private void ShowNextAgent(Direction direction)
	{
		if (Position.X == 0 && direction == Direction.LEFT) return;
		else if (Position.X == GetViewportRect().Size.X * (TrainingManager.AgentCount - 1) && direction == Direction.RIGHT) return;
		else Position += direction == Direction.LEFT ? Vector2.Left * GetViewportRect().Size.X : Vector2.Right * GetViewportRect().Size.X;
	}
}
