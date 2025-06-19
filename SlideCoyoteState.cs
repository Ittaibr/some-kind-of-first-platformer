using Godot;
using System;

public partial class SlideCoyoteState : CoyoteBufferState
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	protected override void TransferChecks()
	{
		if (MoveComp.IsWantJump())
		{
			TransitionTo("SlideJump");
			return;
		}
		if (bufferTimer <= 0)
		{
			TransitionTo("Fall");
		}
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
