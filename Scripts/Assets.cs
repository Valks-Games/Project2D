namespace Project2D;

public static class NoiseTextures
{
	public static FastNoiseLite Voronoi { get; } = LoadNoise("Voronoi");
	public static FastNoiseLite Simplex { get; } = LoadNoise("Simplex");

	private static FastNoiseLite LoadNoise(string path) =>
		GD.Load<FastNoiseLite>($"res://FastNoiseLite/{path}.tres");
}
