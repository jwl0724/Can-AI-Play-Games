using Godot;
using System;

public partial class ScoreUI : Control
{
    // Components
    [Export] private Label currentScoreLabel;
    [Export] private Label bestScoreLabel;

    // Running variables
    private int currentScore = 0;
    private int bestScore = 0;

    public override void _Ready()
    {
        // Set text
        currentScoreLabel.Text = currentScore.ToString();
        bestScoreLabel.Text = bestScore.ToString();

        // Connect signal
        GameLoop.Instance.Connect(GameLoop.SignalName.GameOver, Callable.From((int score) => OnGameOver(score)));
        GameLoop.Instance.Connect(GameLoop.SignalName.ScoreChange, Callable.From((int score) => OnScoreChange(score)));
    }

    private void OnScoreChange(int newScore)
    {
        currentScore = newScore;
        currentScoreLabel.Text = currentScore.ToString();
    }

    private void OnGameOver(int endingScore)
    {
        bestScore = endingScore > bestScore ? endingScore : bestScore;
        bestScoreLabel.Text = bestScore.ToString();
    }
}
