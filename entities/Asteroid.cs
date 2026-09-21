using Godot;
using System;

public partial class Asteroid : Area2D
{
	[Export]
	public float Speed { get; set; } = 100.0f;

	public Vector2 Direction { get; set; } = Vector2.Left;
	
	private Label _label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		AreaEntered += OnAreaEntered;
		_label = GetNode<Label>("Label");
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
	
	// Just toggles the state of spawned ones currently, will probably remove later
	// If keeping, moving to setting check instead of per instance check
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("debug"))
		{
			_label.Visible = !_label.Visible;
		}
	}
}
