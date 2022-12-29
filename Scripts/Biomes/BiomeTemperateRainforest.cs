namespace Project2D;

public class BiomeTemperateRainforest : Biome
{
	private World World { get; set; }

	public BiomeTemperateRainforest(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#1E482A"));
	}
}
