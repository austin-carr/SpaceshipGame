using Godot;
using System;

public partial class Asteroid : Area2D
{
	[Export]
	public float Speed { get; set; } = 100.0f;

	public Vector2 Direction { get; set; } = Vector2.Left;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	private void OnBodyEntered(Node2D body)
	{
		QueueFree();
	}

	private void OnAreaEntered(Area2D area)
	{
		QueueFree();
	}
}
