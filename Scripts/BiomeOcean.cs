namespace Project2D;

public class BiomeOcean
{
	private World World { get; set; }

	public BiomeOcean(World world)
	{
		World = world;
	}

	public void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 3);
	}
}
