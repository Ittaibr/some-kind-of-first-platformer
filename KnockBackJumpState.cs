using Godot;
using System;

public partial class KnockBackJumpState : PlayerState
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public override void Enter()
	{
		GD.Print("knockback jump entered");
		velocity = parent.knockbackJumpVelocity;
    }
	public override void PhysicsUpdate(double delta)
	{
		parent.Velocity = velocity;
		parent.MoveAndSlide();
		velocity = parent.Velocity;
		TransferChecks();
	}

	protected override void TransferChecks()
	{
		if (parent.IsOnFloor())
		{
			GD.Print("knockback jump finished");
			TransitionTo("Idle");
		}
		else
		{
			TransitionTo("Fall");
		}
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
