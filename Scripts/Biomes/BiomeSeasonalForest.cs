namespace Project2D;

public class BiomeSeasonalForest : Biome
{
	private World World { get; set; }

	public BiomeSeasonalForest(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 7);
	}
}
