using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : State
{
	[Export]public NodePath intialState;
	private Dictionary<string, State> states;
	protected State currentState;

	protected virtual void Init()
	{
		states = new Dictionary<string, State>();
		foreach (Node node in GetChildren()){
			if (node is State s){
				states.Add(node.Name,s);					
				s.Ready();
				s.Exit();
			}
		}
		GD.Print(intialState);
		currentState = GetNode<State>(intialState);
	}

	public void TransitionTo(string key){
		if (!states.ContainsKey(key) || currentState == states[key]){
			return;
		}
		currentState.Exit();
		currentState = states[key];
		currentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public virtual void Process(double delta)
	{
		currentState.Update((float)delta);
		currentState.TransferChecksAndOperation();

	}
	public virtual void PhysicsProcess(double delta)
	{
		currentState.PhysicsUpdate(delta);
		currentState.TransferChecksAndOperation();
	}
	public virtual void UnhandledInput(InputEvent @event)
	{
		currentState.HandleInput(@event);
		currentState.TransferChecksAndOperation();

	}
}
