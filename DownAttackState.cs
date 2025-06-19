using Godot;
using System;

public partial class DownAttackState : PlayerState
{
	[Export] private float downAttackForce = 300f;
	[Export] private double downAttackDuration = 0.2;
	private double downAttackTimer = 0;
	// Called when the node enters the scene tree for the first time.

	public override void Enter()
	{
		parent.DownAttacksLeft -= 1;
		base.Enter();
		GD.Print("Down Attack State Entered");
		animations.Play(animationName);
		//parent.SetCollisionMaskValue(2, false); // Disable collision with the floor
		downAttackTimer = downAttackDuration; // Reset the timer
	}
	public override void PhysicsUpdate(double delta)
	{
		downAttackTimer -= delta;
		parent.Velocity = velocity;
		velocity.X = 0;
		stateMachine.DashCoolDownTimer -= delta;
		velocity.Y = (float)(( downAttackForce));
		parent.MoveAndSlide();
		TransferChecks();

		// Optionally, you can set a timer to end the down attack after a certain duration

	}
	protected override void TransferChecks()
	{
		if (downAttackTimer <= 0)
		{
			GD.Print("Down Attack State Exiting");
			// Transition to the next state, e.g., Idle or Run
			if (parent.IsOnFloor())
			{
				TransitionTo("Idle");
			}
			else
			{
				TransitionTo("Fall");
			}
		}
		else if (IsWantJump() && stateMachine.jumpsLeft > 0)
		{
			GD.Print("Transitioning to Jump from Down Attack");
			TransitionTo("DoubleJump");
		}
		else if (parent.IsOnFloor())
		{
			TransitionTo("Idle");
		}
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
