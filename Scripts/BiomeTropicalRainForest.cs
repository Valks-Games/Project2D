namespace Project2D;

public class BiomeTropicalRainForest
{
	private World World { get; set; }

	public BiomeTropicalRainForest(World world)
	{
		World = world;
	}

	public void Generate(int x, int z, float amplitude)
	{
		if (amplitude < 50)
			World.SetTile(x, z);
		else if (amplitude is >= 50 and <= 200)
			World.SetTile(x, z, 1);
		else
			World.SetTile(x, z, 3);
	}
}
