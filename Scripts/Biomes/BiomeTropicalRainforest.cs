namespace Project2D;

public class BiomeTropicalRainForest : Biome
{
	private World World { get; set; }
	//private float[,] Noise { get; set; }

	public BiomeTropicalRainForest(World world)
	{
		World = world;
		//Noise = World.CalcNoise(0.02f);
	}

	public override void Generate(Color[] colors, int v)
	{
		World.ColorTile(colors, v, new Color("#427B00"));
	}

	/*public override void Generate(int x, int z)
	{
		var noiseValue = Noise[x, z];

		if (noiseValue < 50)
			World.SetTile(x, z, 4);
		else if (noiseValue is >= 50 and <= 200)
			World.SetTile(x, z, 5);
		else
			World.SetTile(x, z, 6);

		World.SetTile(x, z, 4);
	}*/
}
