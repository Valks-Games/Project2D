namespace Project2D;

public static class NoiseTextures
{
	public static FastNoiseLite Voronoi { get; } = LoadNoise("Voronoi");
	public static FastNoiseLite Simplex1 { get; } = LoadNoise("Simplex1");
	public static FastNoiseLite Simplex2 { get; } = LoadNoise("Simplex2");
	public static FastNoiseLite Simplex3 { get; } = LoadNoise("Simplex3");
	public static FastNoiseLite Simplex4 { get; } = LoadNoise("Simplex4");
	public static FastNoiseLite Simplex5 { get; } = LoadNoise("Simplex5");
	public static FastNoiseLite Simplex6 { get; } = LoadNoise("Simplex6");

	private static FastNoiseLite LoadNoise(string path) =>
		GD.Load<FastNoiseLite>($"res://FastNoiseLite/{path}.tres");
}
