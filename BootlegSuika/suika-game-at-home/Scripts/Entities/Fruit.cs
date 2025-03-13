using Godot;
using System;

public partial class Fruit : RigidBody2D
{
	// Components
	private FruitBuilder builder;
	private CollisionShape2D body;
	private Sprite2D sprite;

	// Signals
	[Signal] public delegate void FirstCollisionEventHandler();

	// Running variables
	public FruitType Type { get; private set; }
	public bool Collided { get; private set; } = false;
	private int scoreAmount;

	public override void _Ready()
	{
		// Disable physics on first spawn
		Sleeping = true;
		Freeze = true;
		SetCollisionLayerValue(1, false);
		SetCollisionMaskValue(1, false);

		body = GetNode<CollisionShape2D>("Body");
		sprite = GetNode<Sprite2D>("Sprite");

		// Allow collision reporting
		ContactMonitor = true;
		MaxContactsReported = 8;
	}

	// TODO: Fix bug where fruits still interactable when player is holding it
    public override void _PhysicsProcess(double delta)
    {
		if (IsQueuedForDeletion()) return; // In case another fruit called queue free for the fruit

		// Spawn the next fruit on first collision
		if (Collided == false && GetContactCount() != 0)
		{
			Collided = true;
			EmitSignal(SignalName.FirstCollision);
		}
		// Handle fruit fusion
		foreach(Node2D body in GetCollidingBodies())
		{
			if (body is not Fruit fruit || fruit.IsQueuedForDeletion()) continue;
			if (Fuse(fruit)) break;
		}
    }

    public void SetAttributes(FruitBuilder builder, FruitType type, CompressedTexture2D sprite, int scoreAmount, float radius, bool collided)
	{
		// Set variables
		Type = type;
		this.builder = builder;
		this.scoreAmount = scoreAmount;

		// Set in-game properties
		this.sprite.Texture = sprite;
		CircleShape2D circleShape = body.Shape as CircleShape2D;
		circleShape.Radius = radius;
		Collided = collided;
		Mass = 1f + 0.5f * (int) Type;

		// Scale sprite to size of collision shape
		float scaleFactor = radius * 2 / Math.Max(sprite.GetSize().X, sprite.GetSize().Y) + 0.1f; // Add extra to account for image gap
		if (Type == FruitType.PINEAPPLE)
		{
			// Pineapple sprite completely off from the other sprites, need to account for it
		 	this.sprite.Position += Vector2.Up * 15;
			scaleFactor += 0.15f;
		}
		this.sprite.Scale = Vector2.One * scaleFactor;
	}

	public void Activate()
	{
		Freeze = false;
		Sleeping = false;
		SetCollisionLayerValue(1, true);
		SetCollisionMaskValue(1, true);
	}

	private bool Fuse(Fruit otherFruit)
	{
		if (Type != otherFruit.Type || Type == FruitType.WATERMELON || IsQueuedForDeletion()) return false;
		// Spawn new fruit between the two fruits
		Vector2 middlePoint = (Position + otherFruit.Position) / 2;
		builder.BuildFruit(Type + 1, middlePoint, true);

		// Emit signal for first collision if other fruit has not collided yet
		if (otherFruit.Collided == false) otherFruit.EmitSignal(SignalName.FirstCollision);
		GameLoop.Instance.AddScore(scoreAmount);

		// Delete fruits involved in fusion
		otherFruit.QueueFree();
		QueueFree();
		return true; // Indicate successful fusion
	}
}
