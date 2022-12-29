namespace Project2D;

public class BiomeGrassland : Biome
{
	private World World { get; set; }

	public BiomeGrassland(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#A6E147"));
	}
}
