namespace Project2D;

public class BiomeDesert : Biome
{
	private World World { get; set; }

	public BiomeDesert(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z)
	{
		World.SetTile(x, z, 0, 1);
	}
}
