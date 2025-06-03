using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : State
{
	[Export]public NodePath intialState;
	protected Dictionary<string, State> states;
	protected State currentState;

	protected virtual void Init()
	{
		states = new Dictionary<string, State>();
		foreach (Node node in GetChildren()){
			if (node is State s){
				states.Add(node.Name,s);					
			}
		}
		GD.Print(intialState);
		currentState = GetNode<State>(intialState);
	}

	public override void TransitionTo(string key){
		if (!states.ContainsKey(key) || currentState == states[key]){
			return;
		}
		currentState.Exit();
		currentState = states[key];
		GD.Print(key);
		currentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void Update(double delta)
	{
		currentState.Update((float)delta);
		currentState.TransferChecksAndOperation();

	}
	public override void PhysicsUpdate(double delta)
	{
		currentState.PhysicsUpdate(delta);
		currentState.TransferChecksAndOperation();
	}

}
