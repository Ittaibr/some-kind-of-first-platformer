using Godot;
using System;

public partial class SlideJumpState : JumpState
{
	[Export] float slideHorizontalForce = 200;
	[Export] double slideGravityDec = 0.5;


	public override void Enter()
	{
		//stateMachine.jumpsLeft += 1;
		base.Enter();
		velocity.X = slideHorizontalForce * (animations.FlipH ? -1 : 1);
	}

	public override void PhysicsUpdate(double delta)
	{
		animations.Play(animationName);

		stateMachine.DashCoolDownTimer -= delta;

		float gravity = parent.GetGravity().Y;
		velocity.Y += (float)(delta * gravity * slideGravityDec);

		var movment = GetMovmentDirection() * speed;
		if (movment != 0)
		{

			animations.FlipH = movment < 0;
			//velocity.X = (float)Mathf.MoveToward(velocity.X, movment, speed * runAcc);

		}
		//else { velocity.X = Mathf.MoveToward(velocity.X, 0, (float)runDecc * speed); }


		parent.Velocity = velocity;

		parent.MoveAndSlide();
		velocity = parent.Velocity;
		TransferChecks();




	}
	protected override void TransferChecks()
	{
		if (parent.IsOnWall() && !parent.IsOnFloor())
		{
			TransitionTo("WallSlide");
			return;
		}
		var movment = GetMovmentDirection() * speed;
		if (parent.IsOnFloor() && velocity.Y >= 0)
		{
			stateMachine.TransitionTo("Run");
		}
		/*else if (IsWantJump() && stateMachine.jumpsLeft > 0)
		{
			TransitionTo("DoubleJump");
		}*/
		else if (velocity.X == 0 && parent.IsOnFloor() && movment == 0)
		{
			TransitionTo("Idle");
		}

		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			TransitionTo("Dash");
		}
		else if (IsWantDownAttack() && !parent.IsOnFloor() && parent.DownAttacksLeft > 0)
		{
			TransitionTo("DownAttack");
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
