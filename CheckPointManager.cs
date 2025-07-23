using Godot;
using System;

public partial class CheckPointManager : Node
{
	[Signal]public delegate void CheckpointReachedEventHandler(Marker2D marker);
	[Signal]public delegate void CheckpointRespawnEventHandler(Marker2D marker);
	[Export] Player player;
	private Marker2D RespawnMarker;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RespawnMarker = new Marker2D();
		RespawnMarker.Position = player.GlobalPosition; // Initialize with player's current position
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetCheckpoint(Marker2D marker)
	{
		if (marker != null)
		{
			RespawnMarker = marker;
			GD.Print("Checkpoint set at: " + RespawnMarker.GlobalPosition);
		}
		else
		{
			GD.Print("Invalid checkpoint marker.");
		}
	}

	public Vector2 GetLastCheckpoint()
	{
		EmitSignal(SignalName.CheckpointRespawn,RespawnMarker );
		return RespawnMarker.GlobalPosition;
	}

   

}
