using Godot;
using System;

public partial class FallState : PlayerState
{
	[Export(PropertyHint.Range, "1,2")] double downExtraPush = 10;
	[Export] float maxFallSpeed = 1000f;
	// Called when the node enters the scene tree for the first time.

	public override void Enter()
	{

		GD.Print("fall entered");
		speed = stateMachine.Speed;
		runAcc = stateMachine.RunAcc;
		runDecc = stateMachine.RunDecc;
		gravityPush = stateMachine.GravityPush;
		jumpDecc = stateMachine.JumpDecc;

		velocity = parent.Velocity;

	}
	public override void Exit()
	{
		base.Exit();
		parent.SetCollisionMaskValue(2, true);
	}
	public override void Update(double delta)
	{

	}

	public override void PhysicsUpdate(double delta)
	{
		stateMachine.DashCoolDownTimer -= delta;

		velocity = parent.Velocity;
		float gravity = (float)(parent.GetGravity().Y * gravityPush);
		if (Input.IsActionPressed("move_down"))
		{
			gravity = (float)(downExtraPush * parent.GetGravity().Y * gravityPush);
			parent.SetCollisionMaskValue(2, false);
		}
		else
		{
			parent.SetCollisionMaskValue(2, true);
		}
		velocity.Y += (float)(delta * gravity);


		var movment = GetMovmentDirection() * speed;

		if (movment != 0)
		{
			animations.FlipH = movment < 0;
			velocity.X = (float)Mathf.MoveToward(velocity.X, movment, speed * runAcc);

		}
		else { velocity.X = Mathf.MoveToward(velocity.X, 0, (float)runDecc * speed); }


		parent.Velocity = velocity;
		parent.MoveAndSlide();
		if (!parent.IsOnFloor())
		{
			animations.Play(animationName);
		}

		if (velocity.Y > maxFallSpeed)
		{
			velocity.Y = maxFallSpeed;
		}
		parent.Velocity = velocity;
		TransferChecks();

	}

	protected override void TransferChecks()
	{
		if (parent.IsOnFloor())
		{
			GD.Print("fall on floor");
		}
		if (parent.IsOnFloor() && GetMovmentDirection() != 0)
		{
			GD.Print("fall to run");
			TransitionTo("Run");
		}
		else if (parent.IsOnFloor() && GetMovmentDirection() == 0)
		{
			GD.Print("fall to idle");
			TransitionTo("Idle");
		}
		else if (!parent.IsOnFloor() && parent.IsOnWall())
		{
			GD.Print("fall to wall slide");
			TransitionTo("WallSlide");
		}
		else if (parent.IsOnFloor() && velocity.Y >= 0)
		{
			GD.Print("fall to run on floor");
			TransitionTo("Run");
		}
		else if (IsWantDownAttack() && !parent.IsOnFloor() && parent.DownAttacksLeft > 0)
		{
			TransitionTo("DownAttack");		}

		else if (IsWantJump() && stateMachine.jumpsLeft > 0)
		{
			GD.Print("fall has in transition to jump" + stateMachine.jumpsLeft + "jumps left");

			TransitionTo("DoubleJump");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			GD.Print("fall to dash");
			TransitionTo("Dash");
		}
	}
	protected override bool IsWantJump()
	{
		return Input.IsActionJustPressed("jump");
	}
	



	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
