namespace Project2D;

public class BiomeWater : Biome
{
	private World World { get; set; }

	public BiomeWater(World world)
	{
		World = world;
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, Colors.CornflowerBlue);
	}
}
