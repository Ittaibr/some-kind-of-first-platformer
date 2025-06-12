using Godot;
using System;

public partial class SimpleChaseComponent : Area2D
{
	[Export] MoveInterface moveInterface;
	public Vector2 direction{ get; set; } = Vector2.Zero;
	CharacterBody2D character;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D character)
		{
			this.character = character;
			moveInterface.SetWantChase(true);
			GD.Print("ChaseComponent: Detected CharacterBody2D: " + character.Name);
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _PhysicsProcess(double delta)
	{
		if (character == null)
		{
			return;
		}
		direction = ToLocal(character.Position).Normalized();
		if (!OverlapsBody(character))
		{
			direction = Vector2.Zero;
			moveInterface.SetWantChase(false);
			character = null;
		}

	}

}
