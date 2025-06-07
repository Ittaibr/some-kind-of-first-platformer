using Godot;
using System;

public partial class MoveInterface : Node
{
	protected Vector2 knockbackVelocity{ get; set; } = Vector2.Zero;

	public virtual void SetKnockbackVelocity(Vector2 velocity)
	{
		knockbackVelocity = velocity;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	
	public virtual double GetMovmentDirection()
	{
		return 0;
	}

	public virtual bool IsWantDash()
	{
		return false;
	}
	public virtual bool IsWantSlash()
	{
		return false;
	}

	public virtual bool IsWantJump()
	{
		return false;
	}
	public virtual bool IsWantDown()
	{
		return false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
