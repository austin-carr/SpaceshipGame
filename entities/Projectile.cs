using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public CollisionShape2D CollisionShape2D { get; set; }
	[Export]
	public Sprite2D Sprite2D { get; set; }
	[Export]
	public float Speed { get; set; } = 200.0f;
	
	public Vector2 Direction { get; set; } = Vector2.Right;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		QueueFree(); // Destroy the bullet on impact
	}
}
