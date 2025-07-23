using Godot;
using System;
using Game.Component;
using System.Linq; // Ensure this matches the namespace of HealthComponent
public partial class EnemyManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnCheckPointRespawn(Marker2D marker)
	{
		if (marker == null)
		{
			GD.PrintErr("Checkpoint marker is null!");
			return;
		}
		GD.Print("Checkpoint respawned at: " + marker.GlobalPosition);
		// Logic to handle respawning the enemy at the checkpoint
		foreach (Enemy enemy in GetNode<Node2D>("Bodies").GetChildren().Cast<Enemy>())
		{
			enemy.Health.SetToMaxHealth(); // Assuming HealthComponent has a method to reset health
			enemy.Enable(); // Re-enable the enemy
			enemy.animations.Play("Idle"); // Reset animation to idle or appropriate state
			GD.Print("Enemy respawned at: " + enemy.GlobalPosition);
			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
