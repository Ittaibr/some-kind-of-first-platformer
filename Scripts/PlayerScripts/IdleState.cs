using Godot;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class IdleState : PlayerState
{
	
	[Export]private double timer = 10;
	private double tempTimer = 0;
	private double knockbackVelocity = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void Enter()
	{
		base.Enter();

		parent.DownAttacksLeft = parent.totaDdownAttacks;
		stateMachine.jumpsLeft = stateMachine.jumpsInAir;
		parent.dashsLeft = parent.dashsInAir;

		velocity.X = 0;
		animations.Frame = 0;
		animations.Stop();
		animations.Play(animationName);
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
			stateMachine.jumpsLeft -=1;
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0 && parent.dashsLeft > 0)
		{
			GD.Print("idle to Roll");
			TransitionTo("Roll");
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
