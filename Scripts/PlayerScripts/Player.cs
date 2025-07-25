using Game.Component;
using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D animations;
	private PlayerStateMachine StateMachine;
	public PlayerMoveComponent moveComponent;
	[Export] public CheckPointManager checkpointManager;
	[Export] public HealthComponent healthComponent;
	[Export] public HitBoxComponent hitBox;
	[Export] public int dashsInAir = 1;
	public int dashsLeft { get; set; }

	[Export] public HurtBoxComponent hurtBox;
	[Export] public int totaDdownAttacks = 1;
	[Export] public Vector2 knockbackJumpVelocity = new Vector2(0, 0);
	[Export] public VelocityCalc2d velocityCalc;
	[Export] public double downMult = 1.1;
	[Export] public double gravityScale = 1;
	[Export] public double jumpHeight = 300;
	[Export] public double jumpCutOff = 0.1;
	[Export] public double airAcc = 300;
	[Export] public double airDecc = 400;
	[Export] public Vector2 cameraOfset = new Vector2(30,0);
	[Export] public float smoothSpeed = 1.7f;
	[Export] public float manualCameraSpeedX = 12f;
	[Export] public float turnCameraSpeedMultX = 2.5f;
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
		//QueueFree();
		//CallDeferred(nameof(ReloadSceneSafely));	
		SetCollisionMaskValue(2, true);
		GD.Print("Player health depleted, reloading scene...");
		animations.Play("Death");
		SetCollisionLayerValue(6, false); // Disable collision layer for death animation
		SetCollisionLayerValue(5, false); // Disable collision layer for death animation

		// Wait for the death animation to finish before reloading the scene		
		animations.AnimationFinished += OnDeathAnimationFinished;
		

	}

	private void OnDeathAnimationFinished()
	{
		animations.AnimationFinished -= OnDeathAnimationFinished; // Unsubscribe from the signal
		healthComponent.SetToMaxHealth();
		Position = checkpointManager.GetLastCheckpoint();
		MoveAndSlide();
		SetCollisionLayerValue(5, true); 
		SetCollisionLayerValue(6, true); 


	}
	private void ReloadSceneSafely()
	{
		GetTree().ReloadCurrentScene();
	}
}
