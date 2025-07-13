using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;

public partial class PlayerStateMachine : Node
{
	[Export] public NodePath intialState;
	private Dictionary<string, PlayerState> states;
	private PlayerState currentState;
	[Export] public float Speed { get; set; } = 200;
	[Export] public double RunAcc { get; set; } = 0.4;
	[Export] public double RunDecc { get; set; } = 0.3;
	[Export] public double GravityPush { get; set; } = 10;
	[Export] public double JumpDecc { get; set; } = 0.6;
	[Export] public Camera2D camera;

	[Export] public double DashCoolDown { get; set; } = 3;
	public double DashCoolDownTimer;
	[Export] public int jumpsInAir = 2;
	private Player parent;
	private Vector2 vect = Vector2.Zero;
	private float speed;
	private float smoothSpeed = 1.7f;
	private bool isFirstTurnCameraFrame = true;
	private double pastDirection = 0;

	public int jumpsLeft { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		

		DashCoolDownTimer = DashCoolDown;
	}
	public void Init(Player parent, AnimatedSprite2D animations, MoveInterface moveComp)
	{
		this.parent = parent;
		states = new Dictionary<string, PlayerState>();
		foreach (Node node in GetChildren())
		{
			if (node is PlayerState s)
			{
				states.Add(node.Name, s);
				s.parent = parent;
				s.animations = animations;
				s.stateMachine = this;
				s.MoveComp = (PlayerMoveComponent)moveComp;

				s.Ready();
				s.Exit();
			}
		}
		GD.Print(intialState);
		currentState = GetNode<PlayerState>(intialState);
		currentState.Enter();

		speed = parent.smoothSpeed;
		smoothSpeed = parent.smoothSpeed;
		camera.Offset = vect;
		
	}

	public void TransitionTo(string key)
	{
		if (!states.ContainsKey(key) || currentState == states[key])
		{
			GD.PrintErr("Transition to state " + key + " failed. State not found or already in that state.");
			return;
		}
		GD.Print(key);

		currentState.Exit();
		currentState = states[key];
		currentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentState.Update((float)delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		currentState.PhysicsUpdate(delta);
		CameraHandler();
	}

	private void CameraHandler()
	{
		
		speed = smoothSpeed;
		/*if (parent.Velocity.X / (parent.Velocity.X) != (vect.X / Mathf.Abs(vect.X)) && vect.X * parent.Velocity.X != 0)
		{
			if (!isFirstTurnCameraFrame)
			{

				speed = smoothSpeed * parent.turnCameraSpeedMultX;
			}
		

		}*/

		if (parent.moveComponent.GetCameraDirectionX() != 0)
		{
			vect.X += (float)parent.moveComponent.GetCameraDirectionX() * parent.manualCameraSpeedX;
		}
		else if (parent.Velocity.X != 0)
		{
			vect.X += speed * (float)parent.Velocity.X / Mathf.Abs(parent.Velocity.X);
		}

		if (Mathf.Abs(vect.X) > parent.cameraOfset.X)
		{
			vect.X = parent.cameraOfset.X * (vect.X / Mathf.Abs(vect.X));
		}
		if (pastDirection == (float)parent.Velocity.X / Mathf.Abs(parent.Velocity.X))
		{
			isFirstTurnCameraFrame = false;
		}
		else
		{
			isFirstTurnCameraFrame = true;
		}

		pastDirection = (float)parent.Velocity.X / Mathf.Abs(parent.Velocity.X);

		camera.Offset = vect;

		//GD.Print("camera offset = " + camera.Offset);

	}
	public override void _UnhandledInput(InputEvent @event)
	{
		currentState.HandleInput(@event);
	}

	private void OnHit(int damage)
	{
		TransitionTo("Hit");
	}
}
