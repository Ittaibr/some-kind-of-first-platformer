using Godot;
using System;
using System.Linq.Expressions;

public partial class RunState : PlayerState
{
	[Export]private double timer = 10;
	private double tempTimer = 0;
	private double knockbackVelocity = 0;
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		GD.Print("run entered");

		base.Enter();
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
		var movment = GetMovment();

		if (movment != 0)
		{
			animations.FlipH = movment < 0;
			velocity.X = (float)Mathf.MoveToward(velocity.X, movment, speed * runAcc);
		}
		else { velocity.X = Mathf.MoveToward(velocity.X, 0, (float)runDecc * speed); }
		if (velocity.X > speed){velocity.X = speed;}
		
		
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
			stateMachine.jumpsLeft -=1;
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			TransitionTo("Roll");
		}
		else if (IsWantSlash())
		{
			TransitionTo("SimpleAttack");
		}
		else if (!parent.IsOnFloor())
		{
			stateMachine.jumpsLeft -=1;
			TransitionTo("CoyoteBuffer");
		}
	}
}
