using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

public partial class PatrolUsingPointsComponent : Node2D
{
	private Node2D positionsNode { get; set; }
	private List<Marker2D> positions { get; set; } = new List<Marker2D>();
	private Random rnd = new Random();
	private Stack<Marker2D>tempPositions = new Stack<Marker2D>();
	private Marker2D currentPosition;
	public Vector2 direction{ get; set; } = Vector2.Zero;
	[Export ]private bool IsShuffle { get; set; } = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	public void Init(Node2D root)
	{
		if (root == null)
		{
			GD.PrintErr("PatrolUsingPointsComponent.Init called with null root!");
			return;
		}
		positionsNode = root;
		var temp = positionsNode.GetChildren();
		foreach (var s in temp)
		{
			if (s is Marker2D marker)
			{
				positions.Add(marker);
			}
		}
		ReloadPositions();
		LoadNextPosition();
	}
	public override void _PhysicsProcess(double delta)
	{
		if (positionsNode != null && currentPosition != null)
		{
			float distance = GlobalPosition.DistanceTo(currentPosition.Position);
			if (distance < 2f)
			{
				// Snap to the marker to avoid jitter
				GlobalPosition = currentPosition.Position;
				direction = Vector2.Zero;
				if (positions.Count == 0)
				{
					ReloadPositions();
				}
				LoadNextPosition();
			}
			direction = ToLocal(currentPosition.Position).Normalized();


		}
	}

	private void ReloadPositions()
	{
		foreach (Marker2D s in positions)
		{
			tempPositions.Push(s);
		}

		if (IsShuffle)
		{
			tempPositions.OrderBy(x => rnd.Next());
		}
	}

	private void LoadNextPosition()
	{
		if (tempPositions.Count == 0)
		{
			ReloadPositions();
		}
		if (tempPositions.Count != 0)
		{
			currentPosition = tempPositions.Pop();
			direction = ToLocal(currentPosition.Position).Normalized();
		}

	}

	
}
