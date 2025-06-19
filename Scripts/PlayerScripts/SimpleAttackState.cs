using Godot;
using System;

public partial class SimpleAttackState : PlayerState
{
	[Export] private int frameHit = 5;
	[Export] private NodePath collisionPath;
	[Export] private HitBoxComponent hitBox;
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
			hitBox.KnockbackVelocity = new Vector2(-Mathf.Abs(hitBox.KnockbackVelocity.X), hitBox.KnockbackVelocity.Y);

		}
		else
		{
			vec.X = Mathf.Abs(vec.X);
			hitBox.KnockbackVelocity = new Vector2(Mathf.Abs(hitBox.KnockbackVelocity.X), hitBox.KnockbackVelocity.Y);
		}
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
		velocity = parent.Velocity;
		velocity.X = (float)Mathf.MoveToward(velocity.X, 0, (float)runDecc * speed);
		//velocity.Y = 0; // Prevent falling during attack]
		parent.Velocity = velocity;
		parent.MoveAndSlide();
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
		base.Exit();
		collision.Disabled = true;

	}
	public override void _Ready()
	{

	}


	protected void OnAnimationStop()
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
