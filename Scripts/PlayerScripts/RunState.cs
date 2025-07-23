using Godot;
using System;
using System.Linq.Expressions;

public partial class RunState : PlayerState
{
	[Export]private double timer = 10;
	[Export] private double turnSpeed = 50;
	[Export] private float maxSpeed = 300;
	private double tempTimer = 0;
	private double knockbackVelocity = 0;
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		GD.Print("run entered");
		parent.dashsLeft = parent.dashsInAir;
		base.Enter();
		parent.DownAttacksLeft = parent.totaDdownAttacks;
		stateMachine.jumpsLeft = stateMachine.jumpsInAir;

	}
	public override void Exit()
	{ 
		base.Exit();
	}

	protected virtual double GetMovment()
	{
		return GetMovmentDirection() * speed;

	} 
	public override void PhysicsUpdate(double delta)
	{
		/*float gravity = parent.GetGravity().Y;
		velocity.Y += (float)(delta * gravity);*/
		stateMachine.DashCoolDownTimer -= delta;
		velocity = parent.Velocity;

		if (velocity.X > maxSpeed) velocity.X = maxSpeed;
		var movment = GetMovmentDirection();

		if (movment != 0)
		{
			animations.FlipH = movment < 0;
		}
		velocity.X = (float)movment * speed;
		
		//if (velocity.X > speed) { velocity.X = speed; }

		velocity = parent.velocityCalc.GetRunVelocityX(velocity, delta, turnSpeed, new Vector2((float)runAcc, 0), new Vector2((float)runDecc, 0));

		parent.Velocity = velocity;
		parent.MoveAndSlide();
		TransferChecks();
		}
	public override void Update(double delta)
	{
		
    }



	protected override void TransferChecks()
	{
		var movment = GetMovmentDirection() * speed;

		if (velocity.X == 0 && movment == 0)
		{
			TransitionTo("Idle");
		}
		else if (IsWantJump() && parent.IsOnFloor())
		{
			stateMachine.TransitionTo("Jump");
		}
		else if (IsWantDown() && !parent.IsOnFloor())
		{
			stateMachine.jumpsLeft -= 1;
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0 && parent.dashsLeft > 0)
		{
			TransitionTo("Roll");
		}
		else if (IsWantSlash())
		{
			TransitionTo("SimpleAttack");
		}
		else if (!parent.IsOnFloor())
		{
			TransitionTo("CoyoteBuffer");
			return;
		}
	}
}
