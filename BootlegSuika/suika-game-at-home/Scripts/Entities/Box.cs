using Godot;
using System;
using System.Linq;

public partial class Box : Node
{
	public override void _Ready()
	{
		GameLoop.Instance.SetComponent(this);
		GameLoop.Instance.Connect(GameLoop.SignalName.GameOver, Callable.From((int score) => OnGameOver()));
	}

	public void Reset()
	{
		foreach(Node child in GetChildren()) child.QueueFree();
	}

	// Sends fruit flying out of box when game over
	private void OnGameOver()
	{
		foreach(Fruit fruit in GetChildren().Cast<Fruit>())
		{
			if (!fruit.InBounds || fruit == GameLoop.Instance.CurrentFruit) continue;

			// Activate physics and make them fall through box
			fruit.Activate();
			fruit.SetCollisionLayerValue(1, false);
			fruit.SetCollisionMaskValue(1, false);

			// Apply random force to fly off on game over
			uint forceScalar = GD.Randi() % 2000;
			fruit.ApplyImpulse(new Vector2(GD.RandRange(-1, 1), -1).Normalized() * forceScalar);
		}
	}
}
