using Godot;
using System;

public partial class GameLoop : Node
{
	// Components (Need the components to set themselves since this loads way before them)
	public Player Player { get; private set; }
	public Box FruitContainer { get; private set; }
	public FruitBuilder Builder { get; private set; }

	// Events
	[Signal] public delegate void ScoreChangeEventHandler(int score);
	[Signal] public delegate void NextFruitPickedEventHandler(int fruitType); // Expected to typecast back to FruitType
	[Signal] public delegate void GameOverEventHandler(int score);
	[Signal] public delegate void GameStartEventHandler();
	[Signal] public delegate void PauseStateChangeEventHandler(bool paused);

	// Running variables
	public Vector2 BoxBorders { get; private set; } = new Vector2(265, 795);
	public bool Paused { get; private set; } = false;
	public bool Playing { get; private set; } = false;
	public int Score { get; private set; } = 0;
	public FruitType NextFruit { get; private set; }
	public Fruit CurrentFruit { get; private set; } = null;

	public override void _Ready()
	{
		FruitContainer = GetNode<Box>("Box");
		Builder = GetNode<FruitBuilder>("Managers/FruitBuilder");
		Player = GetNode<Player>("Player");
		StartGame();
	}

	public Godot.Collections.Array<Fruit> GetContainerFruits() {
		return FruitContainer.GetFruitsInBox();
	}

	// Only called in the training scene
	public void DisablePlayerInput(bool disabled)
	{
		Player.DisablePlayerInput(disabled);
	}

	public void AddScore(int score)
	{
		Score += score;
		EmitSignal(SignalName.ScoreChange, Score);
	}

	public void PauseGame(bool pause)
	{
		Paused = pause;
		EmitSignal(SignalName.PauseStateChange, pause);
	}

	// Spawn the fruit into player's hand
	public void SpawnFruit()
	{
		if (Player.IsHoldingFruit) return;

		Fruit fruit = Builder.BuildFruit(NextFruit, Player.GetFruitSpawnPoint(), false);
		CurrentFruit = fruit;
		CurrentFruit.Connect(Fruit.SignalName.FirstCollision, new Callable(this, nameof(SpawnFruit)));

		Player.GiveFruit(fruit);
		ChooseNextFruit();
	}

	// Randomly select the next fruit that will spawn (up to oranges)
	private void ChooseNextFruit()
	{
		uint randomInt = GD.Randi() % 5;
		FruitType randomType = (FruitType) randomInt; // Will typecast to corresponding fruit type
		NextFruit = randomType;
		EmitSignal(SignalName.NextFruitPicked, (int) NextFruit);
	}

	public virtual void EndGame()
	{
		EmitSignal(SignalName.GameOver, Score);
		Playing = false;
	}

	// Resets the game to beginning state
	public void ResetGame()
	{
		EmitSignal(SignalName.ScoreChange, 0);
		FruitContainer.Reset();
		Score = 0;
		CallDeferred(nameof(StartGame));
	}

	private void StartGame()
	{
		Playing = true;
		Player.Reset();
		CurrentFruit = null;
		ChooseNextFruit();
		SpawnFruit();
		EmitSignal(SignalName.GameStart);
	}
}