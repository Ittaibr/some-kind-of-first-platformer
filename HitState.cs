using Game.Component;
using Godot;
using System;

public partial class HitState :PlayerState
{
	protected bool isFinished = false;
	[Export] HealthComponent health;
	[Export] double invincibilityTimer = 1.5;

	[Export]private double timer = 10;
	private double tempTimer = 0;
	private double knockbackVelocity = 0;
	public override void Ready()
	{

	}
	public override void Enter()
	{
		base.Enter();
		GD.Print("HitState entered");
		isFinished = false;
		health.SetTemporaryImmortalityTimer(invincibilityTimer);
		velocity.X = (float)GetKnockbackVelocity();
		SetKnockbackVelocity(Vector2.Zero);
		GD.Print("knockback velocity is " + GetKnockbackVelocity());
		
	}
	public override void Exit()
	{

	}
	public override void PhysicsUpdate(double delta)
	{
		velocity.Y += (float)(delta * parent.GetGravity().Y);
		
		base.PhysicsUpdate(delta);
		parent.Velocity = velocity;
		parent.MoveAndSlide();
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
