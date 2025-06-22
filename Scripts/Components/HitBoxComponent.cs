using Godot;
using System;

public partial class HitBoxComponent : Area2D
{
	[Export]public int Damage { get; set; } = 1;
	[Export] public Vector2 KnockbackVelocity { get; set; } = Vector2.Zero;
	[Signal] public delegate void HitBodyEventHandler(Area2D body);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public void EmitHitBodySignal(Area2D body)
	{
		GD.Print("HitBoxComponent EmitHitBodySignal called");
		if (body == null)
		{
			GD.PrintErr("HitBoxComponent EmitHitBodySignal called with null body");
			return;
		}
		GD.Print("HitBoxComponent EmitHitBodySignal called with body: " + body.Name);
		EmitSignal(SignalName.HitBody, body);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
