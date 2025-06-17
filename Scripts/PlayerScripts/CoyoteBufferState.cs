using Godot;
using System;

public partial class CoyoteBufferState : FallState
{
	[Export] private double bufferTime = 0.2; // Time in seconds to buffer the roll
	private double bufferTimer = 0.0;

	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		bufferTimer = bufferTime; // Initialize the buffer timer
		base.Enter();
		GD.Print("Coyote entered");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{
		bufferTimer -= delta; // Decrease the buffer timer
		base.PhysicsUpdate(delta);

	}


	protected override void TransferChecks()
	{
		if (MoveComp.IsWantJump())
		{
			TransitionTo("Jump");
			return;
		}
		if (bufferTimer <= 0)
		{
			TransitionTo("Fall");
		}
		
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
