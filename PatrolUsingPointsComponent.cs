using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PatrolUsingPointsComponent : Node2D
{
	private Node2D positionsNode { get; set; }
	private Marker2D[] positions { get; set; }
	private Random rnd = new Random();
	private Stack<Marker2D>tempPositions;
	private Marker2D currentPosition;
	public Vector2 direction{ get; set; } = Vector2.Zero;
	private bool IsShuffle { get; set; } = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	public void Init(Node2D root)
	{
		positionsNode = root;
		var temp = positionsNode.GetChildren();
		foreach (Marker2D s in temp)
		{
			positions.Append(s);
		}
		ReloadPositions();
		LoadNextPosition();
	}
	public override void _PhysicsProcess(double delta)
	{
		if (positionsNode != null)
		{
			if (GlobalPosition.DistanceTo(currentPosition.Position) < 10)
			{
				ReloadPositions();
			}
		}
	}

	private void ReloadPositions()
	{
		foreach (Marker2D s in positions)
		{
			tempPositions.Append(s);
		}

		if (IsShuffle)
		{
			tempPositions.OrderBy(x => rnd.Next());
		}
	}

	private void LoadNextPosition()
	{
		if (tempPositions.First() == null)
		{
			ReloadPositions();
		}
		currentPosition = tempPositions.Pop();
		direction = ToLocal(currentPosition.Position).Normalized();

	}

	
}
