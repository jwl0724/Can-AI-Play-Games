using Godot;
using System;

public partial class GameLoop : Node
{
	// Singleton
	public static GameLoop Instance { get; private set; }

	// Components


	// Events
	[Signal] public delegate void ScoreChangeEventHandler(int score);

	// Running variables
	public bool Paused { get; private set; } = false;
	public int Score { get; private set; } = 0;
	public FruitType NextFruit { get; private set; } = null;
	public FruitType CurrentFruit { get; private set; } = null;

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
		EmitSignal(SignalName.ScoreChangeEventHandler, Score);
	}

	public void PauseGame(pause)
	{
		Paused = pause;
	}

	// Randomly select the next fruit that will spawn (up to oranges)
	private void ChooseNextFruit()
	{

	}

	// Resets the game to beginning state
	private void ResetGame()
	{
		Score = 0;
		EmitSignal(SignalName.ScoreChangeEventHandler, Score);
	}
}