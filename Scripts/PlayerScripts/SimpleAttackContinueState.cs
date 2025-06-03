using Godot;
using System;

public partial class SimpleAttackContinueState : SimpleAttackState
{
	// Called when the node enters the scene tree for the first time.
	public override void Ready()
	{
		base.Ready();
	}
	public override void Enter()
	{
		base.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void PhysicsUpdate(double delta)
	{
		base.PhysicsUpdate(delta);
	}
	protected override void TransferChecks()
	{
		if (isFinished)
		{
			TransitionTo("Run");
		}
	}
	public override void Exit()
	{
		base.Exit();
	}
	



}
