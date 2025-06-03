using Godot;
using System;

public partial class DoubleJumpState : JumpState
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public override void PhysicsUpdate(double delta)
	{
		base.PhysicsUpdate(delta);
	}
	protected override bool IsWantDown()
	{
		return base.IsWantDown();
	}
	protected override bool IsWantJump()
	{
		return false;
    }
	protected override void TransferChecks()
	{
		base.TransferChecks();
	}

	public override void Enter()
	{
		GD.Print("Double jump entered");
		base.Enter();
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
