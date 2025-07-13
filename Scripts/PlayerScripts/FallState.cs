using Godot;
using System;

public partial class FallState : PlayerState
{
	[Export] double downExtraPush = 10;
	[Export] float maxFallSpeed = 1000f;
	[Export] float turnSpeed = 30.0f;
	[Export] float maxFallSpeedX = 260;
	
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
		if (velocity.X > maxFallSpeedX)
		{
			velocity.X = maxFallSpeedX;
		}
		velocity = parent.Velocity;
		if (Input.IsActionPressed("move_down"))
		{
			parent.SetCollisionMaskValue(2, false);
			velocity.Y += (float)downExtraPush;
		}
		else
		{
			parent.SetCollisionMaskValue(2, true);
		}
		//velocity.Y += (float)(delta * gravity);

		velocity.Y = parent.velocityCalc.GetFallVelocity(false, parent.jumpCutOff, parent.downMult, delta).Y;
		parent.Velocity = velocity;
		var movment = GetMovmentDirection();
		velocity.X = (float)movment * speed;

		if (movment != 0)
		{
			animations.FlipH = movment < 0;
			//velocity.X = (float)Mathf.MoveToward(velocity.X, movment, speed * runAcc);

		}
		
		velocity.X = parent.velocityCalc.GetRunVelocityX(velocity, delta, turnSpeed, new Vector2((float)parent.airAcc, 0), new Vector2((float)parent.airDecc, 0)).X;
		parent.Velocity = velocity;

			//velocity = parent.velocityCalc.GetRunVelocityX(velocity, delta, turnSpeed, new Vector2((float)runAcc, 0), new Vector2((float)runDecc, 0));

		if (!parent.IsOnFloor())
		{
			animations.Play(animationName);
		}

			if (velocity.Y > maxFallSpeed)
			{
				velocity.Y = maxFallSpeed;
			}
			parent.Velocity = velocity;
			parent.MoveAndSlide();

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
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0 && parent.dashsLeft > 0)
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
