namespace Project2D;

public class BiomeDesert : Biome
{
	private World World { get; set; }

	public BiomeDesert(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#EDDB76"));
	}
}
