namespace Project2D;

public class BiomeTemperateRainforest : Biome
{
	private World World { get; set; }

	public BiomeTemperateRainforest(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 6);
	}
}
