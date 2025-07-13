using Godot;
using System;

public partial class DownAttackState : PlayerState
{
	[Export] private float downAttackForce = 300f;
	[Export] private double downAttackDuration = 0.2;
	[Export] protected CollisionShape2D collision;
	[Export] protected Vector2 defaultKnockback = new Vector2(100,300);
	private double downAttackTimer = 0;
	private bool isHit = false; // Flag to check if the player has hit an enemy
	// Called when the node enters the scene tree for the first time.

	public override void Enter()
	{
		//isHit = false; // Reset the hit flag when entering the state
		collision.Disabled = false; // Enable the collision shape
		parent.DownAttacksLeft -= 1;
		parent.dashsLeft = parent.dashsInAir;

		base.Enter();
		GD.Print("Down Attack State Entered");
		animations.Play(animationName);
		//parent.SetCollisionMaskValue(2, false); // Disable collision with the floor
		downAttackTimer = downAttackDuration; // Reset the timer
	}
	
	public override void Exit()
	{
		collision.SetDeferred("disabled", true); // Disable the collision shape using deferred call

		base.Exit();
		GD.Print("Down Attack State Exiting");
		
		//collision.Disabled = true; // Disable the collision shape
		//parent.SetCollisionMaskValue(2, true); // Re-enable collision with the floor
	}
	public override void PhysicsUpdate(double delta)
	{
		//downAttackTimer -= delta;
		parent.Velocity = velocity;
		stateMachine.DashCoolDownTimer -= delta;

		if (!isHit)
		{
			velocity.X = 0;
			velocity.Y = (float)((downAttackForce));
		}
		velocity.X = 0;
		velocity.Y = (float)((downAttackForce));
		parent.MoveAndSlide();
		TransferChecks();

		// Optionally, you can set a timer to end the down attack after a certain duration

	}
	protected override void TransferChecks()
	{
		if(false){}
		if (isHit)
		{
			isHit = false; // Prevent repeated transitions
			parent.DownAttacksLeft++;
			stateMachine.jumpsLeft = stateMachine.jumpsInAir;
			TransitionTo("KnockBackJump");
			GD.Print("Transitioning to KnockbackJump from Down Attack");
			return;
		} 
		else if (IsWantDash() && parent.dashsLeft > 0)
		{
			TransitionTo("Dash");
		}

		else if (parent.IsOnFloor())
		{
			TransitionTo("Idle");
			return;
		}
		else if (downAttackTimer <= 0)
		{
			GD.Print("Down Attack State Exiting");
			// Transition to the next state, e.g., Idle or Run
			if (parent.IsOnFloor())
			{
				TransitionTo("Idle");
			}
			else
			{
				TransitionTo("Fall");
			}
		}
		else if (IsWantJump() && stateMachine.jumpsLeft > 0)
		{
			GD.Print("Transitioning to Jump from Down Attack");
			TransitionTo("DoubleJump");
		}
		else if (parent.IsOnFloor())
		{
			TransitionTo("Idle");
		}
	}

	public void OnHitBody(HurtBoxComponent body)
	{
		GD.Print("Hit an enemy with Down Attack");
		var knocback = body.knockbackVelocity;

        if (!body.isFixedKnockbackLengthX && GetMovmentDirection() != 0)
		{
			knocback.X = defaultKnockback.X * (float)GetMovmentDirection();
		}
		if (!body.isFixedKnockbackLengthY)
		{
			knocback.Y = defaultKnockback.Y;
		}

		knocback.Y *= -1;

		parent.dashsLeft = parent.dashsInAir;

		/*if (!body.isFixedKnockbackDirection && parent.animations.FlipH)
		{
			knocback.X *= -1;
		}*/
		isHit = true; // Set the flag to true when hitting an enemy

		//if (body.Position.X - collision.Position.X < 0) knocback.X *= -1; // Flip the velocity if the hitbox is to the left of the player
		//if (body.Position.Y - collision.Position.Y < 0) knocback.Y *= -1; // Flip the velocity if the hitbox is above the player
		parent.knockbackJumpVelocity = knocback; // Set the knockback velocity for the player
		collision.CallDeferred("set_disabled", true); // <-- Use deferred call here
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
