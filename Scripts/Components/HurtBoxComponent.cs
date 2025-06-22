using Game.Component;
using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
	[Export] MoveInterface moveInterface;
	[Export] HealthComponent health;
	[Export] public Vector2 knockbackVelocity = Vector2.Zero;
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
			if (moveInterface != null) moveInterface.SetKnockbackVelocity(hitBox.KnockbackVelocity);
			if (health != null) health.SetHealth(health.GetHealth() - hitBox.Damage);
			hitBox.EmitHitBodySignal(this);
		}
	}

	public void SetDisabled(bool disabled)
	{
		if (disabled)
		{
			AreaEntered -= OnAreaEnterd;
		}
		else
		{
			AreaEntered += OnAreaEnterd;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
