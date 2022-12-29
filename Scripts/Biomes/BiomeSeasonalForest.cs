namespace Project2D;

public class BiomeSeasonalForest : Biome
{
	private World World { get; set; }

	public BiomeSeasonalForest(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorSquare(colors, v, new Color("#4C6415"));
	}
}
