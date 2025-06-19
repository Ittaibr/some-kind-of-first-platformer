using Godot;
using System;

public partial class WallSlideState : PlayerState
{
	[Export] private float wallSlideSpeed = 100f; // Speed at which the player slides down the wall
	[Export] private float jumpXvelocity = 200f; // Horizontal velocity when jumping off the wall
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void PhysicsUpdate(double delta)
	{
		velocity.Y = wallSlideSpeed;
		if (animations.FlipH)
		{
			velocity.X = -1; // Adjust horizontal velocity based on facing direction
		}
		else
		{
			velocity.X = 1; // Adjust horizontal velocity based on facing direction
		}
		parent.Velocity = velocity; // Update the player's velocity
		parent.MoveAndSlide(); // Move the player down the wall
		TransferChecks(); // Check for transitions to other states
		//animations.Play(animationName);


	}
	public override void Enter()
	{
		GD.Print("Wall slide entered");
		base.Enter();
		
		velocity.Y = 0; // Set the vertical velocity to slide down the wall
		parent.Velocity = velocity; // Update the player's velocity


	}
	public override void Exit()
	{
		GD.Print("Wall slide exited");
		base.Exit();
		stateMachine.jumpsLeft = stateMachine.jumpsInAir; // Reset jumps left when exiting wall slide
		velocity.Y = 0; // Reset vertical velocity when exiting wall slide
		parent.Velocity = velocity; // Update the player's velocity
		
	}
	protected override void TransferChecks()
	{
		if (IsWantJump())
		{
			if (animations.FlipH)
			{
				velocity.X = jumpXvelocity;
			}
			else
			{
				velocity.X = -jumpXvelocity;
			}
			parent.Velocity = velocity; // Update the player's velocity
			TransitionTo("Jump");
		}
		else if (IsWantDown())
		{
			TransitionTo("Fall");
		}
		else if (IsWantDash())
		{
			parent.animations.FlipH = !animations.FlipH; // Maintain the current facing direction during dash
			TransitionTo("Dash");
		}
		else if (parent.IsOnFloor())
		{

			TransitionTo("Idle");
		}
		else if (!parent.IsOnWall())
		{
			GD.Print("Wall slide to fall");
			TransitionTo("Fall");
		}
		
		
	

	}
}
