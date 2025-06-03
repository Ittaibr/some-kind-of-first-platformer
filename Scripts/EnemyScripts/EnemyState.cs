using Godot;
using System;

public partial class EnemyState : State
{
	protected Vector2 velocity;
	public EnemyStateMachine stateMachine;
	public AnimatedSprite2D animations { get; set;}
	public Enemy parent;
	[Export] protected string animationName;
	public override void Enter()
	{ 
		velocity = parent.Velocity;
		animations.Play(animationName);
	}
}
