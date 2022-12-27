namespace Project2D;

public static class ExtensionsGodotMath
{
	public static Vector2 Lerp(this Vector2 v1, Vector2 v2, float t) =>
		new Vector2(Mathf.Lerp(v1.x, v2.x, t), Mathf.Lerp(v1.y, v2.y, t));
}
