using Godot;
using System;

public partial class EnemyMoveComponent : MoveInterface
{
	private double direction = 1;
	private Timer directionSwitchTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		directionSwitchTimer = GetNode<Timer>("Timer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _PhysicsProcess(double delta)
	{
	}
	public override double GetMovmentDirection()
	{
		return direction;
	}

	private void OnSwitchTimerTimeout()
	{
		direction *= -1;
	}
}
