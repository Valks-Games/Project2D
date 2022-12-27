namespace Project2D;

public class BiomeGrassland : Biome
{
	private World World { get; set; }

	public BiomeGrassland(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z)
	{
		World.SetTile(x, z, 4);
	}
}
