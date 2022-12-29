namespace Project2D;

public static class NoiseTextures
{
	public static FastNoiseLite Voronoi { get; } = LoadNoise("Voronoi");
	public static FastNoiseLite Simplex1 { get; } = LoadNoise("Simplex1");
	public static FastNoiseLite Simplex2 { get; } = LoadNoise("Simplex2");

	private static FastNoiseLite LoadNoise(string path) =>
		GD.Load<FastNoiseLite>($"res://FastNoiseLite/{path}.tres");
}
