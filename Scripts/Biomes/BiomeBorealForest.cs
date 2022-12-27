namespace Project2D;

public class BiomeBorealForest : Biome
{
	private World World { get; set; }

	public BiomeBorealForest(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 2);
	}
}
