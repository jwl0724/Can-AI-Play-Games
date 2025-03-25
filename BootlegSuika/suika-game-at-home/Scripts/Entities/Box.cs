using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Box : Node
{
	public bool AllowFusion => (Owner as GameLoop).Playing;

	public override void _Ready()
	{
		GameLoop game = Owner as GameLoop;
		game.Connect(GameLoop.SignalName.GameOver, Callable.From((int score) => OnGameOver()));
	}

	public void Reset()
	{
		foreach(Node child in GetChildren()) child.QueueFree();
	}

	public Godot.Collections.Array<Fruit> GetFruitsInBox() {
		LinkedList<Fruit> fruits = new(); // LL because faster insertion
		foreach(Fruit fruit in GetChildren().Cast<Fruit>()) {
			// Skip the fruit that's being held by the player
			if (fruit == (Owner as GameLoop).Player.HeldFruit) continue;
			fruits.AddLast(fruit);
		}
		return new Godot.Collections.Array<Fruit>(fruits);
	}

	public void AddScore(int score)
	{
		(Owner as GameLoop).AddScore(score);
	}

	// Sends fruit flying out of box when game over
	private void OnGameOver()
	{
		foreach(Fruit fruit in GetChildren().Cast<Fruit>())
		{
			if (!fruit.InBounds || fruit == (Owner as GameLoop).CurrentFruit) continue;

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
