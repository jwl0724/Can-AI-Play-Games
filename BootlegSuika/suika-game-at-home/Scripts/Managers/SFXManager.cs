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

        GameLoop game = Owner as GameLoop;
        game.Connect(GameLoop.SignalName.ScoreChange, Callable.From((int score) => OnFuse()));
        game.Connect(Node.SignalName.Ready, Callable.From(() => {
            game.Player.Connect(Player.SignalName.DropFruit, new Callable(this, nameof(OnDrop)));
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
