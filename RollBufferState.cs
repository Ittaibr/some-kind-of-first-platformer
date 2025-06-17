using Godot;
using System;

public partial class RollBufferState : PlayerState
{
	[Export] private double bufferTime = 0.2; // Time in seconds to buffer the roll
	private double bufferTimer = 0.0;
	private enum transitionTo
	{
		Idle,
		Run,
		Jump,
		Fall,
		DashAttack,
	}
	private transitionTo transitionto = transitionTo.Idle; // Default transition state

	// Called when the node enters the scene tree for the first time.
	public override void Enter()
	{
		bufferTimer = bufferTime; // Initialize the buffer timer
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{

		parent.MoveAndSlide();
		if (bufferTimer > 0)
		{
			bufferTimer -= delta; // Decrease the buffer timer
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
			else transitionto = transitionTo.Idle;

		}
		if (bufferTimer <= 0)
		{
			switch (transitionto)
			{
				case transitionTo.Idle:
					TransitionTo("Idle");
					break;
				case transitionTo.Run:
					TransitionTo("Run");
					break;
				case transitionTo.Jump:
					TransitionTo("Jump");
					break;
				case transitionTo.Fall:
					TransitionTo("Fall");
					break;
				case transitionTo.DashAttack:
					TransitionTo("DashAttack");
					break;
			}
		}
	}
	
	
}
