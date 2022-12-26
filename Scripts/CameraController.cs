namespace Project2D;

public partial class CameraController : Camera2D
{
	private float ZoomIncrement { get; set; } = 0.02f;
	private float TargetZoom { get; set; }
	private float MinZoom { get; set; } = 0.01f;
	private float MaxZoom { get; set; } = 1.0f;

	public override void _Ready()
	{
		TargetZoom = Zoom.x;
	}

	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("right"))
			velocity.x += 1;

		if (Input.IsActionPressed("up"))
			velocity.y -= 1;

		if (Input.IsActionPressed("down"))
			velocity.y += 1;

		var speed = 100;
		Position += velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Zoom = Lerp(Zoom, new Vector2(TargetZoom, TargetZoom), 0.25f);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseButton)
			return;

		if (!mouseButton.IsPressed())
			return;

        if (mouseButton.ButtonIndex == MouseButton.WheelUp)
			TargetZoom += ZoomIncrement;

        if (mouseButton.ButtonIndex == MouseButton.WheelDown)
			TargetZoom -= ZoomIncrement;

		TargetZoom = Mathf.Clamp(TargetZoom, MinZoom, MaxZoom);
	}

	private Vector2 Lerp(Vector2 v1, Vector2 v2, float t) =>
		new Vector2(Mathf.Lerp(v1.x, v2.x, t), Mathf.Lerp(v1.y, v2.y, t));
}
