using Godot;
using System;

public partial class JumpState : PlayerState
{
	private bool isWantJump = true;
	[Export] double timeToJump = 2;
	[Export] double jumpHeight = 120;
	[Export] double jumpTime = 0.5;
	[Export] double fallTime = 0.25;
	[Export] double jumpCutOff = 1.5;
	[Export] double turnSpeed = 300;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void Enter()
	{
		GD.Print("jump entered");

		base.Enter();
		isWantJump = true;
		stateMachine.jumpsLeft -= 1;
		GD.Print("jump has " + stateMachine.jumpsLeft + " jumps left");
		animations.Play(animationName);
		velocity = parent.velocityCalc.GetJumpVelocity(jumpHeight,jumpTime,fallTime,isWantJump,true);
		parent.Velocity = velocity;

		//velocity.Y = -jumpHeight;

	}
	public override void Update(double delta)
	{
		
	}

	public override void PhysicsUpdate(double delta)
	{
		

		animations.Play(animationName);

		stateMachine.DashCoolDownTimer -= delta;

		if (!Input.IsActionPressed("jump"))
		{
			isWantJump = false;
		}
		else{ isWantJump = true;}

		velocity.Y = parent.velocityCalc.GetJumpVelocity(jumpHeight,jumpTime,fallTime,isWantJump,false,jumpCutOff,delta).Y;
		parent.Velocity = velocity;

		var movment = GetMovmentDirection();
		if (movment != 0)
		{

			animations.FlipH = movment < 0;

		}
		velocity.X = (float)movment * speed;
		velocity.X = parent.velocityCalc.GetRunVelocityX(velocity, delta, turnSpeed, new Vector2((float)parent.airAcc, 0), new Vector2((float)parent.airDecc, 0)).X;
		parent.Velocity = velocity;

		parent.Velocity = velocity;

		parent.MoveAndSlide();
		velocity = parent.Velocity;
		TransferChecks();




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
		else if (IsWantDownAttack() && !parent.IsOnFloor() && parent.DownAttacksLeft > 0)
		{
			TransitionTo("DownAttack");
		}
		/*else if (IsWantDown() && !parent.IsOnFloor())
		{
			TransitionTo("DownAttack");
		}*/
		else if (velocity.X == 0 && parent.IsOnFloor() && movment == 0)
		{
			TransitionTo("Idle");
		}
		else if (velocity.Y >= 0)
		{
			TransitionTo("Fall");
		}
		else if (IsWantDash() && stateMachine.DashCoolDownTimer <= 0 && parent.dashsLeft >0)
		{
			TransitionTo("Dash");
		}

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
