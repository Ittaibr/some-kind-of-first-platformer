using Godot;
using System;

public partial class Platform : Path2D
{
	[Export]
	private bool isLooping = true;
	[Export]
	private float speed = 2.0f;
	[Export]
	private bool isOneWayCollision = true;
	[Export]
	private float speedScale = 1.0f;
	[Export]
	private uint setCollisionLayer = 2;
	[Export]
	public bool isRotating = false;
	private CollisionShape2D collsion;
	private PathFollow2D path;
	private AnimationPlayer animation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		path = GetNode<PathFollow2D>("PathFollow2D");
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		collsion = GetNode<CollisionShape2D>("AnimatableBody2D/CollisionShape2D");
		collsion.OneWayCollision = isOneWayCollision;
		GetNode<AnimatableBody2D>("AnimatableBody2D").CollisionLayer = setCollisionLayer;
		path.Rotates = isRotating;

		if (Curve == null)
		{
			GD.PrintErr("PathFollow2D has no curve assigned. Please assign a curve to the PathFollow2D node.");
			SetProcess(false);
			return;
		}
		if (isLooping)
		{
			animation.Play("move");
			animation.SpeedScale = (float)speedScale;
			SetProcess(false);
		}
		path.Progress = 0.0f;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		path.Progress += (float)delta * speedScale;
		animation.Play("move");
		animation.SpeedScale = speedScale;


	}

	public override void _PhysicsProcess(double delta)
	{
		
    }



}
