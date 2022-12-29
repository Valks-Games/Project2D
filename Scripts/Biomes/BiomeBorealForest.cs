namespace Project2D;

public class BiomeBorealForest : Biome
{
	private World World { get; set; }

	public BiomeBorealForest(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#5E7438"));
	}
}
