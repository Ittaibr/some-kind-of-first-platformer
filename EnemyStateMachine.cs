using Godot;
using System;

public partial class EnemyStateMachine : StateMachine
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public void Init(Enemy parent, AnimatedSprite2D animations)
	{
		base.Init();
		foreach (EnemyState s in GetChildren())
		{
				s.parent = parent;
				s.animations = animations;
				s.stateMachine = this;
				s.Ready();
		}
		currentState.Enter();

	}
	public override void PhysicsUpdate(double delta)
	{
		base.PhysicsUpdate(delta);
		 }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
}
