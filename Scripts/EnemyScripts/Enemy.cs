using Godot;
using System;

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


	private void OnHealthDepleted()
	{
		QueueFree();
	}
}
