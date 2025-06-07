using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : State
{
	[Export]public Node intialState;
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
		currentState = (State)intialState;
	}

	public override void TransitionTo(string key){
		if (!states.ContainsKey(key) || currentState == states[key]){
			GD.Print("State not found or already in that state: " + key);
			GD.Print("Current state: " + currentState);
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
		GD.Print(intialState);

		GD.Print("PhysicsUpdateEntered");
		GD.Print(currentState);
		currentState.PhysicsUpdate(delta);
		currentState.TransferChecksAndOperation();
	}
	

}
