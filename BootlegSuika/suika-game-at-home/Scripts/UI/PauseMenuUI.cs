using Godot;
using System;

public partial class PauseMenuUI : Control
{
    private Button resumeButton;
    private Button backButton;
    private Vector2 defaultPosition;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        defaultPosition = Position;
        Position = new Vector2(defaultPosition.X, -1000);

        resumeButton = GetNode<Button>("Resume");
        backButton = GetNode<Button>("Back");

        resumeButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(OnResume)));
        backButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(OnBack)));
        resumeButton.Disabled = true;
        backButton.Disabled = true;
    }

    public void ShowMenu()
    {
        resumeButton.Disabled = false;
        backButton.Disabled = false;
        Position = new Vector2(defaultPosition.X, -1000);
        Tween showTween = CreateTween();
        showTween.TweenProperty(this, nameof(Position).ToLower(), defaultPosition, 0.25);
        showTween.Play();
    }

    // Hides pause menu on resume
    private void OnResume()
    {
        resumeButton.Disabled = true;
        backButton.Disabled = true;
        Tween hideTween = CreateTween();
        hideTween.TweenProperty(this, nameof(Position).ToLower(), new Vector2(defaultPosition.X, 2000), 0.25);
        hideTween.TweenCallback(Callable.From(() => (Owner as GameLoop).ResumeGame()));
        hideTween.Play();
    }

    private void OnBack()
    {
        PackedScene mainMenu = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/MainMenu.tscn");
        GetTree().ChangeSceneToPacked(mainMenu);
    }
}
