using Godot;
using System;

public partial class Checkpoint : Area2D
{
	[Export] Marker2D respownMarker;
	[Export] public CheckPointManager checkpointManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		

	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D player)
		{
			checkpointManager.SetCheckpoint(respownMarker);
			GD.Print("Checkpoint reached at: " + respownMarker.GlobalPosition);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
