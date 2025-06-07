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
		foreach (Node node in GetChildren())
		{
			if (node is EnemyState s)
			{
				s.parent = parent;
				s.animations = animations;
				s.stateMachine = this;
				s.Ready();
			}
		}
		currentState.Enter();
	}
	public override void PhysicsUpdate(double delta)
	{
		base.PhysicsUpdate(delta);
	}
	private void OnHit(int damage)
	{
		TransitionTo("Hit");
	}
    public override void TransitionTo(string key)
    {
		GD.Print("Transitioning to state: " + key);
        base.TransitionTo(key);
    }



	// Called every frame. 'delta' is the elapsed time since the previous frame.

}
