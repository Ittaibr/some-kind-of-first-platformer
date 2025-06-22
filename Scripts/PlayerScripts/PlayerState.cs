using Godot;
using System;

public partial class PlayerState : Node
{
	[Export] protected string animationName ="Run";
	protected float speed = 200;
	protected double runAcc;
	protected double runDecc;
	protected double gravityPush;
	protected double jumpDecc;
	protected Vector2 velocity;
	public PlayerStateMachine stateMachine;
	public AnimatedSprite2D animations { get; set;}
	public PlayerMoveComponent MoveComp { get; set;}

	public Player parent;

	public virtual void Enter()
	{
		speed = stateMachine.Speed;
		runAcc = stateMachine.RunAcc;
		runDecc = stateMachine.RunDecc;
		gravityPush = stateMachine.GravityPush;
		jumpDecc = stateMachine.JumpDecc;
		velocity = parent.Velocity;
		animations.Play(animationName);
		

	}
	public virtual void Exit()
	{
		animations.Stop();
	}

	protected virtual double GetMovmentDirection()
	{
		return MoveComp.GetMovmentDirection();
	}
	protected virtual void SetKnockbackVelocity(Vector2 velocity)
	{
		MoveComp.SetKnockbackVelocity(velocity);
	}
	protected virtual Vector2 GetKnockbackVelocity()
	{
		return MoveComp.GetKnockbackVelocity();
	}
	protected virtual bool IsWantDown()
	{
		return MoveComp.IsWantDown();
	}
	protected virtual bool IsWantJump()
	{
		return MoveComp.IsWantJump();
	}
	protected virtual bool IsWantSlash()
	{
		return MoveComp.IsWantSlash();
	}

	protected virtual bool IsWantDownAttack()
	{
		return MoveComp.IsWantDownAttack();
	}
	protected virtual bool IsWantDash()
	{
		return MoveComp.IsWantDash();
	}


	
	public virtual void Ready()
	{

	}
	public virtual void Update(double delta)
	{

	}
	public virtual void PhysicsUpdate(double delta)
	{

	}
	public virtual void HandleInput(InputEvent @event)
	{

	}
	protected void TransitionTo(string key)
	{
		stateMachine.TransitionTo(key);

	}

	protected virtual void TransferChecks()
	{
		
	}

}
