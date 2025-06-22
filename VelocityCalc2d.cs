using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class VelocityCalc2d : Node

{
	[Export] CharacterBody2D character;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

public Vector2 GetRunVelocityX(Vector2 desiredVelocity, double delta, double turnSpeed, Vector2 Acc, Vector2 Decc)
	{
		var velocity = character.Velocity;
		double maxSpeedChangeX = 0;

		if (desiredVelocity.X != 0)
		{
			if (Mathf.Sign(desiredVelocity.X) != Mathf.Sign(velocity.X))
			{
				maxSpeedChangeX = turnSpeed * delta;
			}
			else
			{
				maxSpeedChangeX = Acc.X * delta;
			}
		}
		else
		{
			maxSpeedChangeX = Decc.X * delta;
		}

		


		float x = Mathf.MoveToward(velocity.X, desiredVelocity.X, (float)maxSpeedChangeX);
		velocity.X = x;
		//velocity.Y = character.Velocity.Y;

		GD.Print(velocity);
		return velocity;



	}
	public Vector2 GetRunVelocity(Vector2 desiredVelocity, double delta, double turnSpeed, Vector2 Acc, Vector2 Decc)
	{
		var velocity = character.Velocity;
		double maxSpeedChangeX = 0;
		double maxSpeedChangeY = 0;

		if (desiredVelocity.X != 0)
		{
			if (Mathf.Sign(desiredVelocity.X) != Mathf.Sign(velocity.X))
			{
				maxSpeedChangeX = turnSpeed * delta;
			}
			else
			{
				maxSpeedChangeX = Acc.X * delta;
			}
		}
		else
		{
			maxSpeedChangeX = Decc.X * delta;
		}

		if (desiredVelocity.Y != 0)
		{
			if (Mathf.Sign(desiredVelocity.Y) != Mathf.Sign(velocity.Y))
			{
				maxSpeedChangeY = turnSpeed * delta;
			}
			else
			{
				maxSpeedChangeY = Acc.Y * delta;
			}
		}
		else
		{
			maxSpeedChangeY = Decc.Y * delta;
		}

		float y = Mathf.MoveToward(velocity.Y, desiredVelocity.Y, (float)maxSpeedChangeY);

		float x = Mathf.MoveToward(velocity.X, desiredVelocity.X, (float)maxSpeedChangeX);
		velocity = new Vector2(x, y);
		velocity.X = x;


		return velocity;



	}
	public Vector2 GetFallVelocity(
	bool isWantJump,
	double jumpCutOff,
	double fallGravMult,
	double delta
)
	{
		var velocity = character.Velocity;

		// Use the current gravity from the engine or your character
		float gravity = character.GetGravity().Y;

		// Calculate fall gravity multiplier (for faster fall)
		// You can tune this formula as needed for your game feel
		float fallGravityMultiplier = (float)fallGravMult;

		float appliedGravity = gravity;

		// If falling, use fall gravity multiplier for faster fall
		if (velocity.Y > 0.01f)
		{
			appliedGravity *= fallGravityMultiplier;
		}
		// If rising and jump released, use jump cut-off multiplier
		else if (velocity.Y < -0.01f && !isWantJump)
		{
			appliedGravity *= (float)jumpCutOff;
		}

		velocity.Y += appliedGravity * (float)delta;

		// Preserve X velocity (handled elsewhere)
		GD.Print("fall velocity " + velocity);
		return velocity;
	}
	


public Vector2 GetJumpVelocity(
   	double jumpHeight = 120,
  	double jumpTime = 0.5,
  	double fallTime = 0.3,
  	bool isWantJump = true,
 	bool jumpJustPressed = true,
 	double jumpCutOff = 1.5,
	double delta =0
	)
{
    var velocity = character.Velocity;

    // Calculate gravity and jump speed based on desired jump height and time to apex
    float gravity = (float)(2 * jumpHeight / (jumpTime * jumpTime));
    float jumpSpeed = (float)(gravity * jumpTime);

    // Calculate fall gravity multiplier
    float fallGravityMultiplier = (float)((jumpTime * jumpTime) / (fallTime * fallTime));

    // Handle jump initiation
    if (jumpJustPressed)
    {
        velocity.Y = -jumpSpeed; // Negative for upward movement in Godot
    }
    else
    {
        // Apply gravity
        float appliedGravity = gravity;

        // If falling, use fall gravity multiplier for faster fall
        if (velocity.Y > 0.01f)
        {
            appliedGravity *= fallGravityMultiplier;
        }
        // If rising and jump released, use jump cut-off multiplier
        else if (velocity.Y < -0.01f && !isWantJump)
        {
            appliedGravity *= (float)jumpCutOff;
        }

        velocity.Y += appliedGravity * (float)delta;
    }

		// Preserve X velocity (handled elsewhere)
	GD.Print("jump velocity " + velocity);

    return velocity;
}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
