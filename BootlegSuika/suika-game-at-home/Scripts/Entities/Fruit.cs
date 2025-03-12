using Godot;
using System;

public partial class Fruit : RigidBody2D
{
	// Components
	private FruitBuilder builder;
	private CollisionShape2D body;
	private Sprite2D sprite;

	// Running variables
	public FruitType Type { get; private set; }
	public bool Collided { get; private set; } = false;
	private int scoreAmount;

	public override void _Ready()
	{
		// Setup collision body
		body = GetNode<CollisionShape2D>("Body");

		// Setup sprite
		sprite = GetNode<Sprite2D>("Sprite");
	}

	public void SetAttributes(FruitBuilder builder, FruitType type, CompressedTexture2D sprite, int scoreAmount, float radius)
	{
		// Set variables
		Type = type;
		this.builder = builder;
		this.scoreAmount = scoreAmount;

		// Set in-game properties
		this.sprite.Texture = sprite;
		CircleShape2D circleShape = body.Shape as CircleShape2D;
		circleShape.Radius = radius;
	}

	public override void _Process(double delta)
	{
	}

	private void Fuse(Fruit otherFruit)
	{
		if (Type != otherFruit.Type || Type == FruitType.WATERMELON) return;
		// Spawn new fruit between the two fruits
		Vector2 middlePoint = (Position + otherFruit.Position) / 2;
		builder.BuildFruit(Type + 1, middlePoint);

		// Delete fruits involved in fusion
		otherFruit.QueueFree();
		QueueFree();
	}
}
