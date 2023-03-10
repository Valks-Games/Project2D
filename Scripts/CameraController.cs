namespace Project2D;

public partial class CameraController : Camera2D
{
	// Zooming
	private float ZoomIncrementDefault { get; set; } = 0.02f;
	private float ZoomIncrement { get; set; } = 0.02f;
	private float TargetZoom { get; set; }
	private float MinZoom { get; set; } = 0.01f;
	private float MaxZoom { get; set; } = 1.0f;
	private float SmoothFactor { get; set; } = 0.25f;

	// Panning
	private Vector2 InitialPanPosition { get; set; }
	private bool Panning { get; set; }

	public override void _Ready()
	{
		// Set the initial target zoom value on game start
		TargetZoom = Zoom.X;
	}

	public override void _Process(double delta)
	{
		// Not sure if the below code should be in _PhysicsProcess or _Process

		// Arrow keys and WASD move camera around
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("left"))
			velocity.X -= 1;

		if (Input.IsActionPressed("right"))
			velocity.X += 1;

		if (Input.IsActionPressed("up"))
			velocity.Y -= 1;

		if (Input.IsActionPressed("down"))
			velocity.Y += 1;
		
		if (Panning)
			Position = InitialPanPosition - (GetViewport().GetMousePosition() / Zoom.X);

		// Arrow keys and WASD movement are added onto the panning position changes
		var speed = 50;
		Position += velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Prevent zoom from becoming too fast when zooming out
		ZoomIncrement = ZoomIncrementDefault * Zoom.X;

		// Lerp to the target zoom for a smooth effect
		Zoom = Zoom.Lerp(new Vector2(TargetZoom, TargetZoom), SmoothFactor);
	}

	// Not sure if this should be done in _Input or _UnhandledInput
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
			InputEventMouseButton(mouseButton);
	}

	private void InputEventMouseButton(InputEventMouseButton @event)
	{
		HandlePan(@event);
		HandleZoom(@event);
	}

	private void HandlePan(InputEventMouseButton @event)
	{
		// Left click to start panning the camera
		if (@event.ButtonIndex != MouseButton.Left)
			return;
		
		// Is this the start of a left click or is this releasing a left click?
		if (@event.IsPressed())
		{
			// Save the intial position
			InitialPanPosition = Position + (GetViewport().GetMousePosition() / Zoom.X);
			Panning = true;
		}
		else
			// Only stop panning once left click has been released
			Panning = false;
	}

	private void HandleZoom(InputEventMouseButton @event)
	{
		// Not sure why or if this is required
		if (!@event.IsPressed())
			return;

		// Zoom in
        if (@event.ButtonIndex == MouseButton.WheelUp)
			TargetZoom += ZoomIncrement;

		// Zoom out
        if (@event.ButtonIndex == MouseButton.WheelDown)
			TargetZoom -= ZoomIncrement;

		// Clamp the zoom
		TargetZoom = Mathf.Clamp(TargetZoom, MinZoom, MaxZoom);
	}
}
