using Godot;
using System;

public partial class SimpleAttackState : PlayerState
{
	[Export] private int frameHit = 5;
	[Export] private NodePath collisionPath;
	private CollisionShape2D collision;
	protected bool isFinished = false;
	private bool isWantSlash = false;
	
    public override void Ready()
    {
		collision = GetNode<CollisionShape2D>(collisionPath);
		isFinished = false;

    }

	public override void Enter()
	{
		base.Enter();
		var vec = collision.Position;
		if (animations.FlipH)
		{
			vec.X = -Mathf.Abs(vec.X);
		}
		else{vec.X = Mathf.Abs(vec.X);}
		collision.Position = vec;

		GD.Print("simpleAttackState entered");
		isFinished = false;
		isWantSlash = false;
		collision.Disabled = true;

	}
	public override void PhysicsUpdate(double delta)
	{
		if (animations.Frame >= frameHit)
		{
			collision.Disabled = false;
		}
		if (!isWantSlash)
		{
			isWantSlash = IsWantSlash();
		}

		TransferChecks();
	}
	protected override void TransferChecks()
	{
		if (isFinished && isWantSlash)
		{
			TransitionTo("SimpleAttackContinue");
		}
		else if (isFinished)
		{
			TransitionTo("Run");
		}

	}
	public override void Exit()
	{
		collision.Disabled = true;

	}
	public override void _Ready()
	{

	}


	private void OnAnimationStop()
	{
		isFinished = true;
	}
	
	public override void Update(double delta)
	{

		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
