using Godot;

public partial class GameOverUI : Control
{
	private Button retryButton;
	private Label score;
	private Vector2 defaultPosition;

	public override void _Ready()
	{
		defaultPosition = Position;
		score = GetNode<Label>("Score");
		retryButton = GetNode<Button>("Retry");

		Position = new Vector2(defaultPosition.X, -1000);
		retryButton.Disabled = true;

		retryButton.Connect("pressed", new Callable(this, nameof(OnRetry)));
		(Owner as GameLoop).Connect(GameLoop.SignalName.GameOver, new Callable(this, nameof(OnGameOver)));
	}

	private void OnGameOver(int score)
	{
		retryButton.Disabled = false;
		Position  = new Vector2(defaultPosition.X, -1000);
		this.score.Text = score.ToString();
		Tween showTween = CreateTween();
		showTween.TweenProperty(this, nameof(Position).ToLower(), defaultPosition, 0.5);
		showTween.Play();
	}

	private void OnRetry()
	{
		retryButton.Disabled = true;
		Tween hideTween = CreateTween();
		hideTween.TweenProperty(this, nameof(Position).ToLower(), new Vector2(defaultPosition.X, 2000), 0.5);
		hideTween.Play();
		(Owner as GameLoop).ResetGame();
	}
}
