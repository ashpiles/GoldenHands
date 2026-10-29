using Godot;
using System;

public partial class Main : Node2D
{
	
	[Signal]
	public delegate void OnToggleSceneFilterEventHandler(bool toggle);
	public override void _Ready()
	{ 
		Control SceneFilter = GetNode<Control>("%SceneFilter");
	}

	public override void _Process(double delta)
	{
	}
}
