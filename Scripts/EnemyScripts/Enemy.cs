using Godot;
using System;
using Game.Component;

public partial class Enemy : CharacterBody2D
{
	[Export] public Node2D PatrolMarkersRoot;
	[Export] public double WalkSpeed { get; set; }
	[Export]public PatrolUsingPointsComponent patrolComponent;
	[Export] public CharacterMoveComponent MoveComp;
	[Export] public HitBoxComponent HitBox { get; set; }
	[Export] public VelocityCalc2d VelocityCalc { get; set; }
	[Export] public HurtBoxComponent hurtBox;
	[Export] private Vector2 hurtKnockbackVelocityOutput = new Vector2(0, 400);
	[Export] public bool isFixedKnockbackDirection = false;
	[Export] public bool isFixedKnockbackLengthX = false;
	[Export] public bool isFixedKnockbackLengthY = false;
	[Export] public HealthComponent Health { get; set; }

	public AnimatedSprite2D animations;
	[Export]private EnemyStateMachine StateMachine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		if (PatrolMarkersRoot == null)  GD.PushError("PatrolMarkersRoot is not assigned!");

		patrolComponent.Init(PatrolMarkersRoot);
		animations = GetNode<AnimatedSprite2D>("EnemySprite");
		StateMachine.Init(this, animations);
		hurtBox.knockbackVelocity = hurtKnockbackVelocityOutput;
		hurtBox.isFixedKnockbackDirection = isFixedKnockbackDirection;
		hurtBox.isFixedKnockbackLengthX = isFixedKnockbackLengthX;
		hurtBox.isFixedKnockbackLengthY = isFixedKnockbackLengthY;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		StateMachine.PhysicsUpdate(delta);
		MoveAndSlide();

    }

	public void Enable()
	{
		HitBox.SetCollisionLayerValue(6,true);
		hurtBox.SetCollisionMaskValue(4, true);
		animations.Play("Idle"); 
		Visible = true; // Make the enemy visible
		GD.Print("Enemy enabled.");
	}
	private void OnHealthDepleted()
	{
		HitBox.SetCollisionLayerValue(6, false);
		hurtBox.SetCollisionMaskValue(4, false);
		Visible = false; // Hide the enemy

		animations.Play("Death");
		GD.Print("Enemy health depleted, hiding enemy.");
		//QueueFree();
	}
}
