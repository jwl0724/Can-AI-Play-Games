using Godot;
using System;

public partial class Box : Node
{
	public override void _Ready()
	{
		GameLoop.Instance.SetComponent(this);
	}

	public void Reset()
	{
		foreach (Node child in GetChildren()) child.QueueFree();
	}
}
