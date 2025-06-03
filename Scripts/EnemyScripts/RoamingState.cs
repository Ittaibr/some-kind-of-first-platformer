using Godot;
using System;

public partial class RoamingState : EnemyState
{
	[Export]double walkSpeed = 100;
	[Export]private double totalWalkDistance = 100;
	private double walkDistance = 0;
	[Export] private double timeLengthToWalk;
	private double walkTimer =0;
	private Vector2 pastPos;



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
	private void ChangeDirection()
	{
		pastPos = parent.Position;
		velocity.X *= -1;


	}

	public override void PhysicsUpdate(double delta)
	{
		walkDistance += Mathf.Abs(parent.Position.DistanceTo(pastPos));
		walkTimer -= delta;
		velocity = parent.Velocity;
		velocity.X = (float)walkSpeed;
		if (parent.IsOnWall())
		{
			ChangeDirection();
		}
		parent.Velocity = velocity;
		parent.MoveAndSlide();

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
