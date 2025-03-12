using Godot;
using System;

public partial class SFXManager : Node
{
    public override void _Ready()
    {
        GameLoop.Instance.Connect(GameLoop.SignalName.ScoreChange, Callable.From((int score) => OnFuse()));
        GameLoop.Instance.Connect(GameLoop.SignalName.ComponentConnected, Callable.From((Node component) => {
            if (component is Player) GameLoop.Instance.Player.Connect(Player.SignalName.DropFruit, Callable.From(() => OnDrop()));
        }));
    }

    private void OnFuse()
    {
        // TODO: Implement setting the audio stream to fuse sound, then play it
    }

    private void OnDrop()
    {
        // TODO: Implement setting the audio stream to drop sound, then play it
    }
}
