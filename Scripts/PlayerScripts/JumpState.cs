using Godot;
using System;

public partial class JumpState : PlayerState
{
	[Export] float jumpForce = 400;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void Enter()
	{
		GD.Print("jump entered");

		base.Enter();
		stateMachine.jumpsLeft -= 1;
		GD.Print("jump has " +stateMachine.jumpsLeft + " jumps left");
		animations.Play(animationName);


		velocity.Y = -jumpForce;

    }
	public override void Update(double delta)
	{
		
	}

	public override void PhysicsUpdate(double delta)
	{
		animations.Play(animationName);

		stateMachine.DashCoolDownTimer -= delta;

		float gravity = parent.GetGravity().Y;
		velocity.Y += (float)(delta * gravity);

		if (velocity.Y < 0 && IsWantDown())
		{
			velocity.Y *= (float)jumpDecc;
		}


		var movment = GetMovmentDirection() * speed;
		if (movment != 0)
		{

			animations.FlipH = movment < 0;
			velocity.X = (float)Mathf.MoveToward(velocity.X, movment, speed * runAcc);

		}
		else { velocity.X = Mathf.MoveToward(velocity.X, 0, (float)runDecc * speed); }


		parent.Velocity = velocity;

		parent.MoveAndSlide();
		TransferChecks();
		velocity = parent.Velocity;



	}
    protected override bool IsWantDown()
    {
		return Input.IsActionJustReleased("jump");
    }
    protected override bool IsWantJump()
    {
		return Input.IsActionJustPressed("jump");
    }


	protected override void TransferChecks()
	{
		var movment = GetMovmentDirection() * speed;
		if (parent.IsOnFloor() && velocity.Y >= 0)
		{
			stateMachine.TransitionTo("Run");
		}
		else if (IsWantJump() && stateMachine.jumpsLeft > 0)
		{
			TransitionTo("DoubleJump");
		}
		else if (velocity.X == 0 && parent.IsOnFloor() && movment == 0)
		{
			TransitionTo("Idle");
		}
		else if (velocity.Y >= 0)
		{
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0)
		{
			TransitionTo("Dash");
		}

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
