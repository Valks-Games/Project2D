namespace Project2D;

public class BiomeSavanna : Biome
{
	private World World { get; set; }

	public BiomeSavanna(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorSquare(colors, v, new Color("#B2D15B"));
	}
}
