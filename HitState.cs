using Game.Component;
using Godot;
using System;

public partial class HitState : PlayerState
{
	protected bool isFinished = false;
	[Export] HealthComponent health;
	[Export] double invincibilityTimer = 1.5;

	public override void Ready()
	{

	}
	public override void Enter()
	{
		base.Enter();
		GD.Print("HitState entered");
		isFinished = false;
		health.SetTemporaryImmortalityTimer(invincibilityTimer);
		
	}
	public override void Exit()
	{

	}
	public override void PhysicsUpdate(double delta)
	{
		TransferChecks();


	}
	private void OnAnimationStop()
	{
		isFinished = true;
	}
	protected override void TransferChecks()
	{
		
		if (isFinished)
		{
			TransitionTo("Run");
		}

	}
}
