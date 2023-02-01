namespace Project2D;

public static class ExtensionsGodotMath
{
	public static Vector2 Lerp(this Vector2 v1, Vector2 v2, float t) =>
		new Vector2(Mathf.Lerp(v1.X, v2.X, t), Mathf.Lerp(v1.Y, v2.Y, t));
}
