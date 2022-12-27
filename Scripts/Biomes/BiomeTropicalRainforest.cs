namespace Project2D;

public class BiomeTropicalRainForest : Biome
{
	private World World { get; set; }

	public BiomeTropicalRainForest(World world)
	{
		World = world;
	}

	public override void Generate(int x, int z, float amplitude)
	{
		if (amplitude < 50)
			World.SetTile(x, z, 4);
		else if (amplitude is >= 50 and <= 200)
			World.SetTile(x, z, 5);
		else
			World.SetTile(x, z, 6);
	}
}
