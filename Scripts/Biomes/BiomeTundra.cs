namespace Project2D;

public class BiomeTundra : Biome
{
	private World World { get; set; }

	public BiomeTundra(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#678071"));
	}
}
