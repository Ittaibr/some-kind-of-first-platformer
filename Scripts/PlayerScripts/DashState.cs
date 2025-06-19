using Godot;
using System;

public partial class DashState : PlayerState
{
	[Export]protected double DashStrength;
	[Export] protected double timeLengthToDash;
	protected double dashTimer =0;
	[Export]protected double totalDashDistance = 100;
	protected double dashDistance = 0;
	protected Vector2 pastPos;

	// Called when the node enters the scene tree for the first time.

	public override void Enter()
	{
		animations.Frame = 0;

		base.Enter();
		GD.Print("dash entered");



		pastPos = parent.Position;
		DashStrength *= GetMovmentDirection();
		velocity.X = (float)DashStrength;
		velocity.Y = 0;
		dashTimer = timeLengthToDash;
		GD.Print("dash has" + stateMachine.jumpsLeft + "jumps left");
		animations.Play(animationName);

		


	}

	public override void Exit()
	{
		dashDistance = 0;
		stateMachine.DashCoolDownTimer = stateMachine.DashCoolDown;
		DashStrength = Mathf.Abs(DashStrength);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{
		dashDistance += Mathf.Abs(parent.Position.DistanceTo(pastPos));
		if (parent.IsOnWall())
		{
			dashTimer -= delta; // double the dash timer if hit a wall
		}

		parent.Velocity = velocity;
		parent.MoveAndSlide();
		TransferChecks();
	}


	protected override void TransferChecks()
	{
		if (IsWantDownAttack() && !parent.IsOnFloor() && parent.DownAttacksLeft > 0)
		{
			TransitionTo("DownAttack");
		}
		if (dashTimer <= 0 || dashDistance >= totalDashDistance)
		{
			GD.Print("ExitingDash");
			if (dashTimer <= 0)
			{
				GD.Print("dashTimerRunout");
			}
			if (!parent.IsOnFloor() && parent.IsOnWall())
			{
				TransitionTo("WallSlide");
				GD.Print("dash to wall slide");
				return;
			}
			else if (IsWantDownAttack() && !parent.IsOnFloor() && parent.DownAttacksLeft > 0)
			{
				TransitionTo("DownAttack");
			}
			else if (!parent.IsOnFloor())
			{
				GD.Print("dashToFall");
				GD.Print("dash has in exit to fall " + stateMachine.jumpsLeft + "jumps left");

				TransitionTo("Fall");
			}
			else if (MoveComp.GetMovmentDirection() != 0)
			{
				TransitionTo("Run");
			}
			else TransitionTo("Idle");
		}
	}


	protected override double GetMovmentDirection()
	{
		/*if (velocity.X < 0)
		{
			GD.Print("left");
			return -1;
		}
		if (velocity.X > 0)
		{
			GD.Print("right");
			return -1;
		} */
		if (animations.FlipH)
		{
			GD.Print("Flip");
			return -1;
		}
		GD.Print("no flip");
		return 1;
		
	}

	
	

}
