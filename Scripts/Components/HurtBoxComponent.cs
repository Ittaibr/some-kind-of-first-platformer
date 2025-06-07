using Game.Component;
using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
	[Export] MoveInterface moveInterface;
	[Export] HealthComponent health;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEnterd;
		
	}

	private void OnAreaEnterd(Area2D body)
	{
		if (body is HitBoxComponent)
		{
			HitBoxComponent hitBox = (HitBoxComponent)body;
			moveInterface.SetKnockbackVelocity(hitBox.KnockbackVelocity);
			health.SetHealth(health.GetHealth() - hitBox.Damage);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
