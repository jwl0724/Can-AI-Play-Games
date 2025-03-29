using Godot;
using System;

public partial class MainMenuUI : Control
{
    private static PackedScene gameScene = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/GameScene.tscn");
    private static PackedScene watchScene = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/ReplayScene.tscn");

    private Button playButton;
    private Button quitButton;
    private Button watchButton;

    public override void _Ready()
    {
        playButton = GetNode<Button>("Play");
        watchButton = GetNode<Button>("Watch");
        quitButton = GetNode<Button>("Quit");

        playButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(OnPlay)));
        watchButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(OnWatch)));
        quitButton.Connect(Button.SignalName.Pressed, new Callable(this, nameof(OnQuit)));
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }

    private void OnPlay()
    {
        GetTree().ChangeSceneToPacked(gameScene);
    }

    private void OnWatch()
    {
        GetTree().ChangeSceneToPacked(watchScene);
    }
}
