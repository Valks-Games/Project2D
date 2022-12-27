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

namespace Project2D;

public enum BiomeType
{
	Desert,
	Savanna,
	TropicalRainforest,
	Grassland,
	Woodland,
	SeasonalForest,
	TemperateRainforest,
	BorealForest,
	Tundra,
	Ice
}

public partial class World : TileMap
{
	private FastNoiseLite HeatNoise { get; set; } = NoiseTextures.Voronoi;
	private FastNoiseLite MoistureNoise { get; set; } = NoiseTextures.Simplex;

	private WorldSettings WorldSettings { get; set; } = new() 
	{
		ChunkSize = 300, // required otherwise PreChunkSize will not work correctly for the first run
		// must be set to whatever chunkSize is set in the UI
		// not ideal, but this is how it has to be for now
	};
	private Dictionary<Vector2, Tile> Tiles { get; set; } = new();
	private BiomeType[,] BiomeTable { get; set; } = new BiomeType[6, 6]
	{
		// COLDEST              COLDER           COLD                      HOT                           HOTTER                       HOTTEST
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert            }, // DRYEST
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert            }, // DRYER
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna           }, // DRY
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna           }, // WET
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest}, // WETTER
		{ BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest}  // WETTEST
	};
	public int PrevChunkSize { get; set; }

	public void DeleteWorld()
	{
		for (int x = 0; x < PrevChunkSize; x++)
			for (int z = 0; z < PrevChunkSize; z++)
				SetCell(0, new Vector2i(-PrevChunkSize / 2, -PrevChunkSize / 2) + new Vector2i(x, z));
	}

	public void Generate(WorldSettings settings)
	{
		DeleteWorld();

		PrevChunkSize = WorldSettings.ChunkSize;
		WorldSettings = settings;

		MoistureNoise.Frequency = settings.MoistureFrequency;
		MoistureNoise.Seed = settings.MoistureSeed.GetHashCode();

		HeatNoise.Frequency = settings.TemperatureFrequency;
		HeatNoise.Seed = settings.TemperatureSeed.GetHashCode();

		var biomes = new Dictionary<BiomeType, Biome>
		{
			{ BiomeType.Tundra,              new BiomeTundra(this)              },
			{ BiomeType.Ice,                 new BiomeIce(this)                 },
			{ BiomeType.Grassland,           new BiomeGrassland(this)           },
			{ BiomeType.Woodland,            new BiomeWoodland(this)            },
			{ BiomeType.BorealForest,        new BiomeBorealForest(this)        },
			{ BiomeType.SeasonalForest,      new BiomeSeasonalForest(this)      },
			{ BiomeType.TemperateRainforest, new BiomeTemperateRainforest(this) },
			{ BiomeType.TropicalRainforest,  new BiomeTropicalRainForest(this)  },
			{ BiomeType.Desert,              new BiomeDesert(this)              },
			{ BiomeType.Savanna,             new BiomeSavanna(this)             }
		};

		for (int x = 0; x < settings.ChunkSize; x++)
			for (int z = 0; z < settings.ChunkSize; z++)
			{
				var moistureValue = Mathf.Clamp(MoistureNoise.GetNoise2d(x, z) + 1 + settings.MoistureIntensity   , 0, 1);
				var heatValue     = Mathf.Clamp(    HeatNoise.GetNoise2d(x, z) + 1 + settings.TemperatureIntensity, 0, 1);

				// Generate the tiles
				var biome = GetBiome(moistureValue, heatValue);
				biomes[biome].Generate(x, z);

				// Store information about each tile
				var tile = new Tile();
				tile.Moisture = moistureValue;
				tile.Heat = heatValue;
				tile.BiomeType = biome;

				Tiles[new Vector2(x, z)] = tile;
			}
	}

	private BiomeType GetBiome(float moistureNoise, float heatNoise)
	{
		var moistureType = moistureNoise.Remap(0, 1, 0, BiomeTable.GetLength(0) - 1);
		var heatType = heatNoise.Remap(0, 1, 0, BiomeTable.GetLength(0) - 1);

		return BiomeTable[(int)moistureType, (int)heatType];
	}

	public void SetTile(int worldX, int worldZ, int tileX = 0, int tileY = 0) =>
		SetCell(0, new Vector2i(-WorldSettings.ChunkSize / 2, -WorldSettings.ChunkSize / 2) + new Vector2i(worldX, worldZ), 0, new Vector2i(tileX, tileY));
}
