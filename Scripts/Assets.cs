namespace Project2D;

public static class NoiseTextures
{
	public static FastNoiseLite Voronoi { get; } = LoadNoise("Voronoi");
	public static FastNoiseLite Simplex1 { get; } = LoadNoise("Simplex1");
	public static FastNoiseLite Simplex2 { get; } = LoadNoise("Simplex2");
	public static FastNoiseLite Simplex3 { get; } = LoadNoise("Simplex3");
	public static FastNoiseLite Simplex4 { get; } = LoadNoise("Simplex4");

	private static FastNoiseLite LoadNoise(string path) =>
		GD.Load<FastNoiseLite>($"res://FastNoiseLite/{path}.tres");
}
