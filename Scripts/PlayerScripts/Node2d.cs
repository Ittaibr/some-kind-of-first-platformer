using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public partial class Node2d : CharacterBody2D
{
	[Export]
	public double Speed = 250f;
	[Export]
	public double maxSpeed = 500.0f;
	[Export]
	public double JumpVelocity = -400.0f;
	[Export]
	public double gravityPush = 100f;
	[Export] public double movmentMultiplayer = 1;
	[Export] private float dashSpeed = 1000;
	[Export] private float dashMaxdistance =300;
	[Export] private Curve dashCurve;
	[Export] private double dashCooldown =10;
	[Export(PropertyHint.Range, "0,1,")] private double acceleration;
	[Export(PropertyHint.Range, "0,1,")] private double decceleration;
	[Export(PropertyHint.Range, "0,1,")] private double deccelerationOnJumpRelease;

	private Node StateMachine;

	private double dashTimer = 0;


	private enum States { Idle, Running, Jumping, Falling, Dashing, Rolling, Attacking };
	private States playerState = States.Idle;
	private Vector2 velocity;
	private AnimatedSprite2D playerSprite;
	private float dashStartPosition = 0;

	

	private void UpdateAnimation()
	{
		if (playerState != States.Dashing && playerState != States.Rolling)
		{
			if (velocity.X != 0)
			{
				playerSprite.FlipV = false;
				playerSprite.FlipH = velocity.X < 0;
				playerState = States.Running;
			}
			else
			{
				playerState = States.Idle;
			}
		}
		switch (playerState)
		{
			case States.Idle: playerSprite.Animation = "Idle"; break;
			case States.Running: playerSprite.Animation = "Run"; break;
			case States.Jumping: break;
			case States.Falling: break;
			case States.Dashing: playerSprite.Animation = "Roll"; break;
			case States.Rolling: playerSprite.Animation = "Roll"; break;
			case States.Attacking: break;
		}
		playerSprite.Play();
	}
	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		if (Input.IsActionJustPressed("Dash") && playerState != States.Dashing && dashTimer <= 0)
		{
			playerState = States.Dashing;
			dashStartPosition = Position.X;
			dashTimer = dashCooldown;
		}
		//HandleInput();
		playerSprite = GetNode<AnimatedSprite2D>("playerSprite");

		if (playerState != States.Dashing && playerState != States.Rolling)
		{

			if (Input.IsActionPressed("jump") && IsOnFloor())
			{
				//GetNode<AudioStreamPlayer2D>("jumpSound").Play(1); ;
				velocity.Y = (float)(JumpVelocity * movmentMultiplayer);
				playerState = States.Jumping;
			}


			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
				if (Input.IsActionJustReleased("jump") && velocity.Y < 0)
				{
					velocity.Y *= (float)(deccelerationOnJumpRelease * movmentMultiplayer);
					playerState = States.Falling;
				}
				if (!Input.IsActionPressed("jump") || velocity.Y > 0)
				{
					velocity.Y += (float)gravityPush;
				}
			}
			else
			{
				SetCollisionMaskValue(2, !Input.IsActionPressed("move_down"));
				playerState = States.Falling;
			}
			
			var direction = Input.GetAxis("move_left","move_right");

			if (direction != 0)
			{
				velocity.X = (float)Mathf.MoveToward(velocity.X, direction * maxSpeed, acceleration * maxSpeed);
				playerState = States.Running;
			}
			else
			{
				velocity.X = (float)Mathf.MoveToward(velocity.X, 0, decceleration * maxSpeed);

			}



			if (velocity.X > maxSpeed && playerState != States.Dashing && playerState != States.Rolling) { velocity.X = (float)maxSpeed; }
			if (velocity.X < -maxSpeed && playerState != States.Dashing && playerState != States.Rolling) { velocity.X = (float)-maxSpeed; }
		}

		

		if (playerState == States.Dashing)
		{
			float currentDistance = MathF.Abs(Position.X - dashStartPosition);
			if (currentDistance >= Mathf.Abs(dashMaxdistance) || IsOnWall())
			{
				playerState = States.Running;
				dashTimer = dashCooldown;
			}
			else
			{
				velocity.X = dashSpeed * dashCurve.Sample(currentDistance / dashMaxdistance);
				velocity.Y = 0;
			}

		}
		if (dashTimer >= 0)
		{
			dashTimer -= delta;
		}
		if (velocity.X == 0) {playerState = States.Idle;}
		Velocity = velocity;
		UpdateAnimation();
		MoveAndSlide();
	}
}
