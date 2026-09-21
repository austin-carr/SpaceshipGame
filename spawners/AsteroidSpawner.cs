using Godot;
using System;

public partial class AsteroidSpawner : Timer
{
	[Export]
	public PackedScene AsteroidScene { get; set; }
	[Export]
	public float SpawnInterval { get; set; } = 1.0f;

	private float _timeSinceLastSpawn = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (AsteroidScene == null)
		{
			return;
		}

		if (_timeSinceLastSpawn >= SpawnInterval)
		{
			SpawnAsteroid();
			_timeSinceLastSpawn = 0.0f;
		}
		else
		{
			_timeSinceLastSpawn += (float)delta;
		}
	}

	private void SpawnAsteroid()
	{
		Asteroid asteroidInstance = AsteroidScene.Instantiate<Asteroid>();
		
		var label = asteroidInstance.GetNode<Label>("Label");
		
		Camera2D camera = GetViewport().GetCamera2D();
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		Vector2 viewSize = viewportSize / camera.Zoom;
		float cameraTop = camera.GlobalPosition.Y - (viewSize.Y / 2.0f);
		float cameraBottom = camera.GlobalPosition.Y + (viewSize.Y / 2.0f);
		
		float randY = (float)GD.RandRange(cameraBottom - 30, cameraTop + 30);
		
		asteroidInstance.Position = new Vector2(
			GetViewport().GetVisibleRect().Size.X / 2 - 10f,
			randY
		);
		
		label.Text = $"X {GetViewport().GetVisibleRect().Size.X / 2 - 10f}, Y {randY}";

		GetTree().CurrentScene.AddChild(asteroidInstance);
	}
}
