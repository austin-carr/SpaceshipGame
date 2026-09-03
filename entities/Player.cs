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
	
	//public const float JumpVelocity = -400.0f;
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 mousePosition = GetGlobalMousePosition();
		
		float deltaY = mousePosition.Y - GlobalPosition.Y;
		
		if (MathF.Abs(deltaY) > StopDistance)
		{
			// Move up or down based on the sign of deltaY
			velocity.Y = MathF.Sign(deltaY) * Speed;
		}
		else
		{
			// Stop moving if close enough
			velocity.Y = 0;
		}
		
		velocity.X = 0;

		Velocity = velocity;
		MoveAndSlide();
		
		Camera2D camera = GetViewport().GetCamera2D();

		if (camera != null)
		{
			// 2. Get the size of the viewport (screen)
			Vector2 viewportSize = GetViewportRect().Size;
			
			// Adjust for camera zoom if applicable
			Vector2 viewSize = viewportSize / camera.Zoom;

			// 3. Calculate top and bottom limits in global coordinates
			float cameraTop = camera.GlobalPosition.Y - (viewSize.Y / 2.0f);
			float cameraBottom = camera.GlobalPosition.Y + (viewSize.Y / 2.0f);

			// 4. Clamp the character's Y position within the limits (with padding)
			Vector2 clampedPosition = GlobalPosition;
			clampedPosition.Y = Mathf.Clamp(
				clampedPosition.Y, 
				cameraTop + ScreenPadding, 
				cameraBottom - ScreenPadding
			);

			GlobalPosition = clampedPosition;
		}

		// Add the gravity.
		//if (!IsOnFloor())
		//{
		//	velocity += GetGravity() * (float)delta;
		//}

		// Handle Jump.
		//if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		//{
		//	velocity.Y = JumpVelocity;
		//}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		//if (direction != Vector2.Zero)
		//{
		//	velocity.X = direction.X * Speed;
		//}
		//else
		//{
		//	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		//}

		//Velocity = velocity;
		//MoveAndSlide();
	}
}
