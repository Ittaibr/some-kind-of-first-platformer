using Godot;
using System;

public partial class RunState : PlayerState
{
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		GD.Print("run entered");

		base.Enter();
		stateMachine.jumpsLeft = stateMachine.jumpsInAir;

	}
	public override void Exit() { }

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
		if (IsWantJump() && parent.IsOnFloor())
		{
			stateMachine.TransitionTo("Jump");
		}
		if (IsWantDown() || !parent.IsOnFloor())
		{
			TransitionTo("Fall");
		}
		if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			TransitionTo("Dash");
		}
		if (IsWantSlash())
		{
			TransitionTo("SimpleAttack");
		}
	}
}
