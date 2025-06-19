using Godot;
using System;

public partial class DashAttackState : SimpleAttackState
{
	[Export] protected float horizontalSlideSpeed = 60;
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		base.Enter();
		animations.Play(animationName);
		GD.Print("dash attack entered");
		if (!animations.IsConnected("animation_finished", new Callable(this, nameof(OnAnimationStop))))
		{
			animations.Connect("animation_finished", new Callable(this, nameof(OnAnimationStop)));
		}
		//parent.hurtBox.SetCollisionMaskValue(6, false); // Disable hurtbox collision
	}
	public override void Exit()
	{
		base.Exit();
		//parent.hurtBox.SetCollisionMaskValue(6, true); // Re-enable hurtbox collision
	}
	public override void PhysicsUpdate(double delta)
	{
		base.PhysicsUpdate(delta);
		if (animations.FlipH)
		{
			velocity.X = -horizontalSlideSpeed;
		}
		else
		{
			velocity.X = horizontalSlideSpeed;
		}
		parent.Velocity = velocity;
		parent.MoveAndSlide();
	}

	protected override void TransferChecks()
	{
		if (isFinished) { TransitionTo("Run"); }

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
