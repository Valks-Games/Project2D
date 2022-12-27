namespace Project2D;

public class BiomeTundra : Biome
{
	private World World { get; set; }

	public BiomeTundra(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z)
	{
		World.SetTile(x, z, 0);
	}
}
