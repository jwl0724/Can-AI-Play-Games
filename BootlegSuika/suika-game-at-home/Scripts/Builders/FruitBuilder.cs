using Godot;
using System;

public partial class FruitBuilder : Node
{
	public Fruit BuildFruit(FruitType type, Vector3 position)
	{
		int score = 0;
		float radius = 0;

		// TODO: Implement build logic and their appropriate values
		switch(type)
		{
			case FruitType.CHERRY:
				score = 0;
				radius = 0f;
				break;
			case FruitType.STRAWBERRY:
				score = 0;
				radius = 0f;
				break;
			case FruitType.GRAPE:
				score = 0;
				radius = 0f;
				break;
			case FruitType.DEKOPON:
				score = 0;
				radius = 0f;
				break;
			case FruitType.ORANGE:
				score = 0;
				radius = 0f;
				break;
			case FruitType.APPLE:
				score = 0;
				radius = 0f;
				break;
			case FruitType.PEAR:
				score = 0;
				radius = 0f;
				break;
			case FruitType.PEACH:
				score = 0;
				radius = 0f;
				break;
			case FruitType.PINEAPPLE:
				score = 0;
				radius = 0f;
				break;
			case FruitType.MELON:
				score = 0;
				radius = 0f;
				break;
			case FruitType.WATERMELON:
				score = 0;
				radius = 0f;
				break;
			default:
				GD.PushError($"Invalid fruit type inputted {type}");
				break;
		}
		return InstantiateFruit(type, position, score, radius);
	}

	private Fruit InstantiateFruit(FruitType type, Vector3 position, int score, float radius)
	{
		// TODO: Implement adding the fruit to the scene
		return new Fruit(type, score, radius);
	}
}
