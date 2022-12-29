namespace Project2D;

public class BiomeIce : Biome
{
	private World World { get; set; }

	public BiomeIce(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#FFFFFF"));
	}
}
