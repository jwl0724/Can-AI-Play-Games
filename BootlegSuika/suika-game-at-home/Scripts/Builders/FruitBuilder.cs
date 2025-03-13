using Godot;
using System;

public partial class FruitBuilder : Node
{
	[Export] private PackedScene fruitScene;
	private static readonly float radiusLevelFactor = 1.5f;

	public override void _Ready()
	{
		GameLoop.Instance.SetComponent(this);
	}

	// Returns a reference to the instantiated fruit on the scene
	public Fruit BuildFruit(FruitType type, Vector2 position, bool fromFusion)
	{
		int score = 0;
		float radius = 5f;
		CompressedTexture2D spriteTexture = null;
		// TODO: Modify the hitbox sizes to match closer to game sizes
		switch(type)
		{
			case FruitType.CHERRY:
				score = 1;
				spriteTexture = Images.Cherry;
				break;
			case FruitType.STRAWBERRY:
				score = 3;
				spriteTexture = Images.Strawberry;
				break;
			case FruitType.GRAPE:
				score = 6;
				spriteTexture = Images.Grape;
				break;
			case FruitType.DEKOPON:
				score = 10;
				spriteTexture = Images.Dekopon;
				break;
			case FruitType.ORANGE:
				score = 15;
				spriteTexture = Images.Orange;
				break;
			case FruitType.APPLE:
				score = 21;
				spriteTexture = Images.Apple;
				break;
			case FruitType.PEAR:
				score = 28;
				spriteTexture = Images.Pear;
				break;
			case FruitType.PEACH:
				score = 36;
				spriteTexture = Images.Peach;
				break;
			case FruitType.PINEAPPLE:
				score = 45;
				spriteTexture = Images.Pineapple;
				break;
			case FruitType.MELON:
				score = 55;
				spriteTexture = Images.Melon;
				break;
			case FruitType.WATERMELON:
				score = 66;
				spriteTexture = Images.Watermelon;
				break;
			default:
				GD.PushError($"Invalid fruit type inputted: {type}");
				break;
		}
		return InstantiateFruit(type, spriteTexture, position, score, radius * ((int) type + 1) * radiusLevelFactor, fromFusion);
	}

	private Fruit InstantiateFruit(FruitType type, CompressedTexture2D sprite, Vector2 position, int score, float radius, bool fromFusion)
	{
		Fruit fruit = fruitScene.Instantiate<Fruit>();
		GameLoop.Instance.FruitContainer.AddChild(fruit);
		fruit.SetAttributes(this, type, sprite, score, radius, fromFusion);
		fruit.Position = position;

		// Don't pause physics if spawned from a fusion
		if (fromFusion)
		{
			fruit.Sleeping = false;
			fruit.Freeze = false;
		}
		return fruit;
	}
}
