using Godot;
using System;

public partial class PlayerMoveComponent : MoveInterface
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	
	public override double GetMovmentDirection()
	{
		if (Input.IsActionPressed("move_left"))
		{
			return -1;
		}
		else if (Input.IsActionPressed("move_right"))
		{
			return 1;
		}
		return 0;
	}
	public virtual bool IsWantDownAttack()
	{
		return Input.IsActionJustPressed("DownAttack");
	}
	public void SetKnockbackVelocity(double velocity)
	{
		knockbackVelocity = new Vector2((float)velocity, 0);
	}
	public double GetKnockbackVelocity()
	{
		return knockbackVelocity.X;
	}
	public override bool IsWantSlash()
	{
		return Input.IsActionJustPressed("slash");
	}
	public override bool IsWantDown()
	{
		return Input.IsActionPressed("move_down");
	}
	
	public override bool IsWantDash()
	{
		return Input.IsActionJustPressed("Dash");
	}


	public override bool IsWantJump()
	{

		return Input.IsActionPressed("jump");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
