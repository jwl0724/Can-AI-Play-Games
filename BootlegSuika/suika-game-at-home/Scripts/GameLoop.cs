using Godot;
using System;

public partial class GameLoop : Node
{
	// Singleton
	public static GameLoop Instance { get; private set; }

	// Components (Need the components to set themselves since this loads way before them)
	public Player Player { get; private set; }
	public Box FruitContainer { get; private set; }
	public FruitBuilder Builder { get; private set; }

	// Events
	[Signal] public delegate void ScoreChangeEventHandler(int score);
	[Signal] public delegate void NextFruitPickedEventHandler(int fruitType); // Expected to typecast back to FruitType
	[Signal] public delegate void GameOverEventHandler(int score);
	[Signal] public delegate void PauseStateChangeEventHandler(bool paused);
	[Signal] public delegate void ComponentConnectedEventHandler(Node component);

	// Running variables
	[Export] public Vector2 BoxBorders { get; private set; }
	public bool Paused { get; private set; } = false;
	public int Score { get; private set; } = 0;
	public FruitType NextFruit { get; private set; }
	public Fruit CurrentFruit { get; private set; } = null;

	public override void _Ready()
	{
		Instance = this;
		// TODO: OH GOD FIND A WAY TO WAIT FOR EVERYTHING TO LOAD, THIS IS GENUINELY AWFUL
		Connect(SignalName.ComponentConnected, Callable.From((Node component) => {
			if (Player != null && FruitContainer != null && Builder != null) StartGame();
		}));
	}

	public void SetComponent(Node component)
	{
		if (component is Player player) Player = player;
		else if (component is Box box) FruitContainer = box;
		else if (component is FruitBuilder builder) Builder = builder;
		EmitSignal(SignalName.ComponentConnected, component);
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
		Fruit fruit = Builder.BuildFruit(NextFruit, Player.GetFruitSpawnPoint());
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

	// Resets the game to beginning state
	private void ResetGame()
	{
		EmitSignal(SignalName.GameOver, Score);
		EmitSignal(SignalName.ScoreChange, 0);
		Score = 0;
	}

	private void StartGame()
	{
		Player.Reset();
		CurrentFruit = null;
		ChooseNextFruit();
		SpawnFruit();
	}
}