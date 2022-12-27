namespace Project2D;

public class BiomeWoodland : Biome
{
	private World World { get; set; }

	public BiomeWoodland(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 3);
	}
}
