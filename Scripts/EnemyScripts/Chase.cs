using Godot;
using System;

public partial class Chase : EnemyState
{
	[Export] private SimpleChaseComponent chaseComponent;
	private Vector2 direction;
	private HitBoxComponent hitBox;
	// Called when the node enters the scene tree for the first time.
	public override void Ready()
	{
		hitBox = parent.HitBox;
	}
	public override void Exit()
	{
		GD.Print("Exiting Chase State");
		animations.Stop();
    }

	public override void Enter()
	{
		GD.Print("Entering Chase State");
		animations.Play(animationName);
	}
	public override void PhysicsUpdate(double delta)
	{

		direction = chaseComponent.direction;
		velocity = direction * (float)parent.WalkSpeed;
		parent.Velocity = velocity;
		if (direction.X < 0)
		{
			animations.FlipH = false;
			var vect = hitBox.KnockbackVelocity;
			vect.X = Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;

		}
		else if (direction.X > 0)
		{
			animations.FlipH = true;
			var vect = hitBox.KnockbackVelocity;
			vect.X = -Mathf.Abs(hitBox.KnockbackVelocity.X);
			hitBox.KnockbackVelocity = vect;

		}


	}

	public override void TransferChecksAndOperation()
	{
		if (!parent.MoveComp.isWantChase())
		{
			TransitionTo("Roaming");
		}
	
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
