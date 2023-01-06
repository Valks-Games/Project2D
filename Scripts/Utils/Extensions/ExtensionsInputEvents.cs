﻿namespace Project2D;

public static class ExtensionsInputEvent
{
	// InputEventMouseButton
	public static bool IsLeftClickPressed(this InputEventMouseButton @event) => 
		@event.IsPressed(MouseButton.Left);

	public static bool IsLeftClickReleased(this InputEventMouseButton @event) =>
		@event.IsReleased(MouseButton.Left);

	public static bool IsRightClickPressed(this InputEventMouseButton @event) => 
		@event.IsPressed(MouseButton.Right);

	public static bool IsRightClickReleased(this InputEventMouseButton @event) =>
		@event.IsReleased(MouseButton.Right);

	// Private Helper Functions
	private static bool IsPressed(this InputEventMouseButton @event, MouseButton button) =>
		@event.ButtonIndex == button && @event.Pressed;

	private static bool IsReleased(this InputEventMouseButton @event, MouseButton button) =>
		@event.ButtonIndex == button && !@event.Pressed;
}
