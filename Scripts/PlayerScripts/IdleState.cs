using Godot;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class IdleState : PlayerState
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void Enter()
	{
		base.Enter();
		velocity.X = 0;
		stateMachine.jumpsLeft = stateMachine.jumpsInAir;
		GD.Print("Idle entered");
    }
	public override void PhysicsUpdate(double delta)
	{
		stateMachine.DashCoolDownTimer -= delta;

		parent.Velocity = velocity;
		parent.MoveAndSlide();
		TransferChecks();
	}
	protected override void TransferChecks()
	{
		var movment = GetMovmentDirection();
		if (movment != 0)
		{
			TransitionTo("Run");
		}
		else if (IsWantJump())
		{
			TransitionTo("Jump");
		}
		else if (IsWantDown() || !parent.IsOnFloor())
		{
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			GD.Print("idle to dash");
			TransitionTo("Dash");
		}
		if (IsWantSlash())
		{
			TransitionTo("SimpleAttack");
		}

		
	}
	public override void Update(double delta)
	{

		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
