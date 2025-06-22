using Godot;
using System;

public partial class WallSlideState : PlayerState
{
	[Export] private float wallSlideSpeed = 100f; // Speed at which the player slides down the wall
	[Export] private float jumpXvelocity = 200f; // Horizontal velocity when jumping off the wall
	[Export] private double wallDownSlideMult = 2.4;
	protected int totalNumOfWallCheckTicks = 4;
	protected int numOfWallCheckTicks = 0;

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
		velocity = Vector2.Zero; // Reset velocity to zero at the start of each frame

		if (IsWantDown())
		{
			velocity.Y = (float)(wallSlideSpeed * wallDownSlideMult);
		}
		//	else if (Input.IsActionPressed("move_up"))
		else if (parent.GetLastSlideCollision() != null && parent.GetLastSlideCollision().GetColliderVelocity() != Vector2.Zero)
		{
			velocity.X = parent.GetLastSlideCollision().GetColliderVelocity().X * 1.00f;
			velocity.Y = parent.GetLastSlideCollision().GetColliderVelocity().Y * 1.00f; // Reset vertical v
		}
		else
		{
			velocity = Vector2.Zero;
			
		} // Reset vertical velocity when moving up

		/*if (animations.FlipH && velocity.X > 0)
		{
			velocity.X = 0; // Reset horizontal velocity when facing left
		}
		else if (!animations.FlipH && velocity.X < 0)
		{
			velocity.X = 0; // Reset horizontal velocity when facing right
		}*/


		if (animations.FlipH)
		{
			velocity.X += -40; // Adjust horizontal velocity based on facing direction
			//velocity.X *= 1.3f; // Adjust horizontal velocity to maintain sliding speed
			GD.Print("Wall slide left");
		}
		else
		{
			velocity.X += 40; // Adjust horizontal velocity based on facing direction
			//velocity.X *= 1.3f; // Adjust horizontal velocity to maintain sliding speed
			GD.Print("Wall slide right");
		}
		
		//velocity.X *= 1.3f; // Adjust horizontal velocity to maintain sliding speed

		
	
		if (!parent.IsOnWall())
		{
			numOfWallCheckTicks++;

			velocity.Y = 0; // Reset vertical velocity when not on a wall
			if (animations.FlipH)
			{
				velocity.X += -40; // Adjust horizontal velocity based on facing direction
				//velocity.X *= 1.3f; // Adjust horizontal velocity to maintain sliding speed
				GD.Print("Wall slide left");
			}
			else
			{
				velocity.X += 40; // Adjust horizontal velocity based on facing direction
				//velocity.X *= 1.3f; // Adjust horizontal velocity to maintain sliding speed
				GD.Print("Wall slide right");
			}
			
		}
		parent.Velocity = velocity; // Update the player's velocity
		parent.MoveAndSlide(); // Ensure the player continues to slide down if not on a wall
		TransferChecks(); // Check for transitions to other states
		//animations.Play(animationName);


	}
	public override void Enter()
	{
		GD.Print("Wall slide entered");
		base.Enter();
		numOfWallCheckTicks = 0; // Reset the wall check ticks counter
		
		//velocity.Y = 0; // Set the vertical velocity to slide down the wall
		//parent.Velocity = velocity; // Update the player's velocity


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
			return;
		}
		/*else if (IsWantDown())
		{
			TransitionTo("Fall");
		}*/
		else if (IsWantDash())
		{
			parent.animations.FlipH = !animations.FlipH; // Maintain the current facing direction during dash
			TransitionTo("Dash");
		}
		else if (parent.IsOnFloor())
		{

			TransitionTo("Idle");
		}
		else if (!parent.IsOnWall() && numOfWallCheckTicks > totalNumOfWallCheckTicks)
		{
			GD.Print("Wall slide to fall");
			TransitionTo("Fall");
		}
		
		
	

	}
}
