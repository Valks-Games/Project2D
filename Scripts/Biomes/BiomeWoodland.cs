namespace Project2D;

public class BiomeWoodland : Biome
{
	private World World { get; set; }

	public BiomeWoodland(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorSquare(colors, v, new Color("#8DB049"));
	}
}
