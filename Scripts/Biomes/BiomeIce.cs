namespace Project2D;

public class BiomeIce : Biome
{
	private World World { get; set; }

	public BiomeIce(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 1);
	}
}
