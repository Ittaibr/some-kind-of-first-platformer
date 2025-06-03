using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerStateMachine : Node
{
	[Export] public NodePath intialState;
	private Dictionary<string, PlayerState> states;
	private PlayerState currentState;
	[Export] public float Speed { get; set; } = 200;
	[Export(PropertyHint.Range, "0,1")] public double RunAcc { get; set; } = 0.4;
	[Export(PropertyHint.Range, "0,1")] public double RunDecc { get; set; } = 0.3;
	[Export(PropertyHint.Range, "1,2")] public double GravityPush { get; set; } = 10;
	[Export(PropertyHint.Range, "0,1")] public double JumpDecc { get; set; } = 0.6;
	[Export] public double DashCoolDown { get; set; } = 3;
	public double DashCoolDownTimer;
	[Export] public int jumpsInAir = 2;
	public int jumpsLeft { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DashCoolDownTimer = DashCoolDown;
	}
	public void Init(CharacterBody2D parent, AnimatedSprite2D animations, MoveInterface moveComp)
	{
		states = new Dictionary<string, PlayerState>();
		foreach (Node node in GetChildren())
		{
			if (node is PlayerState s)
			{
				states.Add(node.Name, s);
				s.parent = parent;
				s.animations = animations;
				s.stateMachine = this;
				s.MoveComp = moveComp;

				s.Ready();
				s.Exit();
			}
		}
		GD.Print(intialState);
		currentState = GetNode<PlayerState>(intialState);
		currentState.Enter();
	}

	public void TransitionTo(string key)
	{
		if (!states.ContainsKey(key) || currentState == states[key])
		{
			return;
		}
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
