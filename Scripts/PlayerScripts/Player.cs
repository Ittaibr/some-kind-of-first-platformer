using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D animations;
	private PlayerStateMachine StateMachine;
	public PlayerMoveComponent moveComponent;
	[Export] public HitBoxComponent hitBox;
	[Export] public HurtBoxComponent hurtBox;
	[Export] public int totaDdownAttacks = 1;
	public int DownAttacksLeft { get; set; } = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		moveComponent = GetNode<PlayerMoveComponent>("PlayerMoveComp");
		animations = GetNode<AnimatedSprite2D>("playerSprite");
		StateMachine = GetNode<PlayerStateMachine>("StateMachine");
		StateMachine.Init(this, animations, moveComponent);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _PhysicsProcess(double delta)
	{
	}

	private void OnHealthDepleted()
	{
		QueueFree();
		CallDeferred(nameof(ReloadSceneSafely));	}
	private void ReloadSceneSafely()
	{
		GetTree().ReloadCurrentScene();
	}
}
