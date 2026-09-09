using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 375.0f;
	[Export]
	public float StopDistance { get; set; } = 5.0f;
	[Export]
	public float ScreenPadding { get; set; } = 32.0f; 
	[Export]
	public PackedScene ProjectileScene { get; set; }
	[Export]
	public float FireRate { get; set; } = 0.5f;
	
	private Marker2D _muzzle;
	private bool _canShoot = true;
	private Timer _shootCooldownTimer;
	
	public override void _Ready()
	{
		_muzzle = GetNode<Marker2D>("MuzzleMarker2D");
		
		_shootCooldownTimer = new Timer();
		_shootCooldownTimer.WaitTime = FireRate;
		_shootCooldownTimer.OneShot = true;
		_shootCooldownTimer.Timeout += () => _canShoot = true;
		
		AddChild(_shootCooldownTimer);
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("shoot") && _canShoot)
		{
			Shoot();
		}
	}
	
	private void Shoot()
	{
		if (ProjectileScene == null)
		{
			// TODO: Error
			return;
		}
		
		_canShoot = false;
		_shootCooldownTimer.Start();
		
		Projectile projectileInstance = ProjectileScene.Instantiate<Projectile>();
		projectileInstance.GlobalPosition = _muzzle.GlobalPosition;
		
		Vector2 spawnDirection = Vector2.Right.Rotated(GlobalRotation);
		projectileInstance.Direction = spawnDirection;
		
		GetTree().CurrentScene.AddChild(projectileInstance);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 mousePosition = GetGlobalMousePosition();
		
		float deltaY = mousePosition.Y - GlobalPosition.Y;
		
		if (MathF.Abs(deltaY) > StopDistance)
		{
			velocity.Y = MathF.Sign(deltaY) * Speed;
		}
		else
		{
			velocity.Y = 0;
		}
		
		velocity.X = 0;

		Velocity = velocity;
		MoveAndSlide();
		
		Camera2D camera = GetViewport().GetCamera2D();

		if (camera != null)
		{
			Vector2 viewportSize = GetViewportRect().Size;
			Vector2 viewSize = viewportSize / camera.Zoom;
			
			float cameraTop = camera.GlobalPosition.Y - (viewSize.Y / 2.0f);
			float cameraBottom = camera.GlobalPosition.Y + (viewSize.Y / 2.0f);
			
			Vector2 clampedPosition = GlobalPosition;
			clampedPosition.Y = Mathf.Clamp(
				clampedPosition.Y, 
				cameraTop + ScreenPadding, 
				cameraBottom - ScreenPadding
			);

			GlobalPosition = clampedPosition;
		}
	}
}
