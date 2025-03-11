using Godot;
using System;

public partial class Fruit : RigidBody2D
{
	public FruitType type { get; private set; }
	private int scoreAmount;
	private float radius;
	private Sprite2D sprite;

	public Fruit(FruitType type, int scoreAmount, float radius)
	{
		this.type = type;
		this.scoreAmount = scoreAmount;
		this.radius = radius;
	}

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	private void Fuse(Fruit otherFruit)
	{
		if (this.type != otherFruit.type) return;

	}
}
