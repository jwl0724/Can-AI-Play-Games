using Godot;
using System;

public partial class SFXManager : Node
{
    // Components
    private AudioStreamPlayer drop;
    private AudioStreamPlayer fuse;

    public override void _Ready()
    {
        drop = GetNode<AudioStreamPlayer>("Drop");
        fuse = GetNode<AudioStreamPlayer>("Fuse");

        GameLoop.Instance.Connect(GameLoop.SignalName.ScoreChange, Callable.From((int score) => OnFuse()));
        GameLoop.Instance.Connect(GameLoop.SignalName.ComponentConnected, Callable.From((Node component) => {
            if (component is Player) GameLoop.Instance.Player.Connect(Player.SignalName.DropFruit, Callable.From(() => OnDrop()));
        }));
    }

    private void OnFuse()
    {
        fuse.PitchScale = (float) GD.RandRange(0.9, 1.1);
        fuse.Play();
    }

    private void OnDrop()
    {
        drop.PitchScale = (float) GD.RandRange(0.9, 1.1);
        drop.Play();
    }
}
