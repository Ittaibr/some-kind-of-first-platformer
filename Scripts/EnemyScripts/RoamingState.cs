using Godot;
using System;

public partial class RoamingState : EnemyState
{
	protected double walkSpeed = 100;
	[Export]private double totalWalkDistance = 100;
	private double walkDistance = 0;
	[Export] private double timeLengthToWalk;
	[Export] private HitBoxComponent hitBox;
	private double walkTimer = 0;
	private Vector2 direction;
	



	public override void Ready()
	{
		walkSpeed = parent.WalkSpeed;
	}
	public override void Enter()
	{ 
		
	}
	public override void Exit()
	{
		
	}
	public override void PhysicsUpdate(double delta)
	{
		direction = parent.patrolComponent.direction;
		if (direction.X < 0)
		{
			velocity.X = -(float)walkSpeed;
			var vect = hitBox.KnockbackVelocity;
			vect.X = -1 * Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;

		}
		else
		{
			var vect = hitBox.KnockbackVelocity;
			vect.X = Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;
			velocity.X = (float)walkSpeed;
		}
		parent.Velocity = velocity;
	

	}
	public override void Update(double delta)
	{
		
	}
	public override void HandleInput(InputEvent @event)
	{
		
	}
	public override void TransferChecksAndOperation()
	{
		
	}

	public override void _Process(double delta)
	{
	}
}
