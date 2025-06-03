using Game.Component;
using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
	[Signal] public delegate void ReceivedDamageEventHandler(int damage);
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
			health.SetHealth(health.GetHealth() - hitBox.Damage);
			EmitSignal(nameof(ReceivedDamage), hitBox.Damage);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
