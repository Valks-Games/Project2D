namespace Project2D;

public class BiomeSavanna : Biome
{
	private World World { get; set; }

	public BiomeSavanna(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 1, 1);
	}
}
