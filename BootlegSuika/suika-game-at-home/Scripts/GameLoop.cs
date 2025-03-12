using Godot;
using System;

public partial class GameLoop : Node
{
	// Singleton
	public static GameLoop Instance { get; private set; }

	// Components
	[Export] private FruitBuilder builder;
	[Export] private Player player;

	// Events
	[Signal] public delegate void ScoreChangeEventHandler(int score);
	[Signal] public delegate void NextFruitPickedEventHandler(int fruitType); // Expected to typecast back to FruitType
	[Signal] public delegate void GameOverEventHandler(int score);
	[Signal] public delegate void PauseStateChangeEventHandler(bool paused);

	// Running variables
	public bool Paused { get; private set; } = false;
	public int Score { get; private set; } = 0;
	public FruitType NextFruit { get; private set; }
	public Fruit CurrentFruit { get; private set; } = null;

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{

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
		Fruit fruit = builder.BuildFruit(NextFruit, player.GetFruitSpawnPoint());
		player.GiveFruit(fruit);
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

	// Resets the game to beginning state
	private void ResetGame()
	{
		Score = 0;
		EmitSignal(SignalName.ScoreChange, Score);
	}
}