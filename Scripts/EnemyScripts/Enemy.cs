using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public double WalkSpeed { get; set; }
	public AnimatedSprite2D animations;
	private EnemyStateMachine StateMachine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animations = GetNode<AnimatedSprite2D>("EnemySprite");
		StateMachine = GetNode<EnemyStateMachine>("StateMachine");
		StateMachine.Init(this, animations);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnHealthDepleted()
	{
		QueueFree();
	}
}
