namespace Project2D;

public class BiomeTundra : Biome
{
	private World World { get; set; }

	public BiomeTundra(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		World.SetTile(x, z, 0);
	}
}
