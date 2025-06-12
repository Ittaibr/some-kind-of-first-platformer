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
		GD.Print("RoamingState entered");
		GD.Print("patrolComponent direction is " + parent.patrolComponent.direction);
		animations.Play(animationName);
	}
	public override void Exit()
	{
		
	}
	public override void PhysicsUpdate(double delta)
	{
		direction = parent.patrolComponent.direction;
		velocity = direction * (float)parent.WalkSpeed;
		parent.Velocity = velocity;
		if (direction.X < 0)
		{
			var vect = hitBox.KnockbackVelocity;
			vect.X = -1 * Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;
			animations.FlipH = false;
		}
		else if (direction.X > 0)
		{
			var vect = hitBox.KnockbackVelocity;
			vect.X = Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;
			animations.FlipH = true;
		}
		else
		{
			velocity = Vector2.Zero;
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
		if (parent.MoveComp.isWantChase())
		{
			TransitionTo("Chase");
		}
	}

	public override void _Process(double delta)
	{
	}
}
