using Godot;
using System;

public partial class RollState : DashState
{
	private enum transitionTo
	{
		Idle,
		Run,
		Jump,
		Fall,
		DashAttack,
	}
	private transitionTo transitionto = transitionTo.Idle;
	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		base.Enter();
		animations.Play(animationName);
		GD.Print("roll entered");
		parent.hurtBox.SetCollisionMaskValue(6, false); 
		transitionto = transitionTo.Idle; // Reset transition state
	}
	public override void PhysicsUpdate(double delta)
	{
		if (!animations.IsPlaying())
		{
			animations.Frame = 2;
			animations.Play();
		}
		base.PhysicsUpdate(delta);


		if (!parent.IsOnFloor())
			{
				GD.Print("dashToFall");
				GD.Print("dash has in exit to fall " + stateMachine.jumpsLeft + "jumps left");

				transitionto = transitionTo.Fall;
			}
			else if (IsWantSlash())
			{
				transitionto = transitionTo.DashAttack;
			}
			else if (IsWantJump() && stateMachine.jumpsLeft > 0)
			{
				transitionto = transitionTo.Jump;
			}
			else if (IsWantDown() || !parent.IsOnFloor())
			{
				transitionto = transitionTo.Fall;
			}
			else if (MoveComp.GetMovmentDirection() != 0 && transitionto != transitionTo.DashAttack)
			{
				transitionto = transitionTo.Run;
			}
			//else if () transitionto = transitionTo.Idle;
		
		
	}

	protected override void TransferChecks()
	{
		if (transitionto == transitionTo.DashAttack)
			{
				TransitionTo("DashAttack");
				return;
			}
		if (transitionto == transitionTo.Jump)
		{
			TransitionTo("Jump");

			return;
		}
		
		if (dashTimer <= 0 || dashDistance >= totalDashDistance || !parent.IsOnFloor())
		{
			if (!parent.IsOnFloor())
			{
				parent.MoveAndSlide();
			}
			switch (transitionto)
			{
				case transitionTo.DashAttack:
					TransitionTo("DashAttack");
					break;
				case transitionTo.Idle:
					TransitionTo("Idle");
					break;
				case transitionTo.Run:
					TransitionTo("Run");
					break;
				case transitionTo.Jump:
					TransitionTo("Jump");
					parent.Velocity = velocity;
					break;
				case transitionTo.Fall:
					stateMachine.jumpsLeft -=1;
					TransitionTo("CoyoteBuffer");
					break;

			}


		}
    }  
	
		
	
	public override void Exit()
	{
		base.Exit();
		parent.hurtBox.SetCollisionMaskValue(6, true); // Re-enable hurtbox collision
		GD.Print("roll exited");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

