using Godot;
using System;

public partial class CharacterMoveComponent : MoveInterface
{
	// Called when the node enters the scene tree for the first time.
	public override double GetMovmentDirection()
	{
		return 0;
	}
	public void SetKnockbackVelocity(double velocity)
	{
		knockbackVelocity = new Vector2((float)velocity, 0);
	}
	public Vector2 GetKnockbackVelocity()
	{
		return knockbackVelocity;
	}
}
