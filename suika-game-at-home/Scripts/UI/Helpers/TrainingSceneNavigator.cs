using Godot;
using System;

public partial class TrainingSceneNavigator : Camera2D
{
	private enum Direction { LEFT, RIGHT }
	private TrainingManager manager;

	// Running variables
	private bool IsTranisitioning = false;

	public override void _Ready()
	{
		Offset = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2);
		manager = Owner as TrainingManager;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is not InputEventKey keyEvent) return;
		if (keyEvent.IsActionReleased("Left")) ShowNextAgent(Direction.LEFT);
		if (keyEvent.IsActionReleased("Right")) ShowNextAgent(Direction.RIGHT);
	}

	private void ShowNextAgent(Direction direction)
	{
		if (IsTranisitioning) return;

		if (Position.X == 0 && direction == Direction.LEFT) return;
		else if (Position.X == (GetViewportRect().Size.X + 250) * (manager.AgentCount - 1) && direction == Direction.RIGHT) return;

		IsTranisitioning = true;
		Vector2 newPosition;
		if (direction == Direction.LEFT) newPosition = Position + Vector2.Left * (GetViewportRect().Size.X + 250);
		else newPosition = Position + Vector2.Right * (GetViewportRect().Size.X + 250);

		Tween tween = CreateTween();
		tween.TweenProperty(this, nameof(Position).ToLower(), newPosition, 0.1f);
		tween.TweenCallback(Callable.From(() => IsTranisitioning = false));
		tween.Play();
	}
}
