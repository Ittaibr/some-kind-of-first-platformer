using Godot;
using System;

public partial class EnemyHitState : EnemyState
{
	protected bool isFinished = false;
	[Export]private double timer = 1.5;
	[Export]private NodePath roamingStatePath;
	private double tempTimer = 0;



	public override void Enter()
	{
		base.Enter();
		tempTimer = timer;
		GD.Print("EnemyHitState entered");
		velocity.X = (float)parent.MoveComp.GetKnockbackVelocity();
		GD.Print("knockback velocity is " + parent.MoveComp.GetKnockbackVelocity());
		parent.MoveComp.SetKnockbackVelocity(Vector2.Zero);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void PhysicsUpdate(double delta)
	{
		GD.Print("enemyHit velocity is:" + velocity);
		tempTimer -= delta;

		parent.Velocity = velocity;

    }
	public override void TransferChecksAndOperation()
	{
		if (tempTimer <= 0)
		{
			GD.Print("EnemyHitState finished");
			TransitionTo("Roaming");
		}
    }



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
