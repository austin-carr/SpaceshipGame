using Godot;
using System;

public partial class AsteroidSpawner : Timer
{
	[Export]
	public float SpawnInterval { get; set; } = 1.0f;
	[Export]
	public PackedScene AsteroidScene { get; set; }

	private float _timeSinceLastSpawn = 0.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (AsteroidScene == null)
		{
			// TODO: Error handling
			return;
		}

		HandleAsteroidSpawn((delta));
	}

	// This could probably be better achieved by changing to a Timer node and using the timeout signal, but this works for now
	private void HandleAsteroidSpawn(double delta)
	{
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
