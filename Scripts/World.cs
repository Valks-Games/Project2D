global using Godot;
global using System;
global using System.Collections.Generic;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Runtime.CompilerServices;
global using System.Threading;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
global using System.Linq;

using Noise = SimplexNoise.Noise;

namespace Project2D;

public partial class World : TileMap
{
	private int Size { get; set; } = 100;
	private int SeedTiles { get; set; } = 209323094;
	private int SeedBiomes { get; set; } = 309333032;
	private float FrequencyForAmplitude { get; set; } = 0.02f;
	private float FrequencyForBiome { get; set; } = 0.01f;

	public override void _Ready()
	{
		var noiseAmplitude = CalcNoise(SeedTiles, FrequencyForAmplitude);
		var noiseBiome = CalcNoise(SeedBiomes, FrequencyForBiome);

		var biomeTropicalRainForest = new BiomeTropicalRainForest(this);
		var biomeOcean = new BiomeOcean(this);

		for (int x = 0; x < Size; x++)
			for (int z = 0; z < Size; z++)
			{
				var amplitude = noiseAmplitude[x, z];
				var biome = GetBiome(noiseBiome[x, z]);

				if (biome == Biome.TropicalRainForest)
				{
					// Biome 1
					biomeTropicalRainForest.Generate(x, z, amplitude);
				}
				else if (biome == Biome.Ocean)
				{
					// Biome 2
					biomeOcean.Generate(x, z, amplitude);
				}
			}
	}

	private Biome GetBiome(float noise)
	{
		if (noise < 100)
			return Biome.TropicalRainForest;
		else
			return Biome.Ocean;
	}

	private enum Biome
	{
		TropicalRainForest,
		Ocean
	}

	private float[,] CalcNoise(int seed, float frequency)
	{
		Noise.Seed = seed;
		return Noise.Calc2D(Size, Size, frequency);
	}

	public void SetTile(int worldX, int worldZ, int tileX = 0, int tileY = 0) =>
		SetCell(0, new Vector2i(-Size / 2, -Size / 2) + new Vector2i(worldX, worldZ), 0, new Vector2i(tileX, tileY));
}
