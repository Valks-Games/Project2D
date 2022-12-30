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

public class MoistureHeat
{
	public float Moisture { get; set; }
	public float Heat { get; set; }
}

public class Pos
{
	public int X { get; set; }
	public int Z { get; set; }
}

public partial class World : TileMap
{
	private FastNoiseLite HeatNoise { get; set; } = NoiseTextures.Simplex1;
	private FastNoiseLite MoistureNoise { get; set; } = NoiseTextures.Simplex2;
	private FastNoiseLite RiverNoise { get; set; } = NoiseTextures.Simplex3;

	private WorldSettings WorldSettings { get; set; }
	private Dictionary<Vector2, Tile> Tiles { get; set; } = new();

	private Dictionary<BiomeType, Biome> Biomes { get; set; }
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

	public override void _Ready()
	{
		Biomes = new Dictionary<BiomeType, Biome>
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
	}

	public void Generate(WorldSettings settings)
	{
		WorldSettings = settings;

		if (!int.TryParse((string)WorldSettings.Values["SpawnSize"], out int spawnSize))
			return;

		if (!int.TryParse((string)WorldSettings.Values["ChunkSize"], out int chunkSize))
			return;

		DeleteWorld();

		for (int x = -spawnSize; x <= spawnSize; x++)
			for (int z = -spawnSize; z <= spawnSize; z++)
				GenerateChunk(new Vector2(x, z), chunkSize, GenerateBiomeData(new Vector2(x, z), settings));
	}

	public void DeleteWorld()
	{
		foreach (Node child in GetChildren())
			child.QueueFree();
	}

	public void ColorTile(Color[] colors, int v, Color color)
	{
		colors    [v] = color;
		colors[v + 1] = color;
		colors[v + 2] = color;
		colors[v + 3] = color;
	}

	public void GenerateChunk(Vector2 chunkCoords, int size, BiomeType[,] biomeData)
	{
		var vertices = new Vector3[4 * size * size];
		//var normals  = new Vector3[4 * size * size];
		//var uvs      = new Vector2[4 * size * size];
		var colors   = new   Color[4 * size * size];
		var indices  = new     int[6 * size * size];

		var s = 32; // hard coded size
		var w = s * 2; // width

		var chunkSize = w * size;
		var tileChunkPos = chunkCoords * chunkSize; // (6400, 6400)
		var chunkPos = chunkCoords * size; // (100, 100)
		
		// Adding s adds hardcoded offset to align with godots grid
		// Also offset by (-chunkSize / 2) to center chunk
		var posVec3 = new Vector3(s + (-chunkSize / 2) + tileChunkPos.x, s + (-chunkSize / 2) + tileChunkPos.y, 0);

		var i = 0;
		var v = 0;

		for (int z = 0; z < size; z++)
		{
			for (int x = 0; x < size; x++)
			{
				Biomes[biomeData[x, z]].Generate(colors, v);

				vertices    [v] = new Vector3(-s, -s, 0) + posVec3;
				//normals     [v] = new Vector3( 0, 0,  s);
				//uvs         [v] = new Vector2( 0, 0    );
																	   
				vertices[v + 1] = new Vector3(-s, s, 0 ) + posVec3;
				//normals [v + 1] = new Vector3( 0, 0, s );
				//uvs     [v + 1] = new Vector2( 0, s    );

				vertices[v + 2] = new Vector3( s, s, 0 ) + posVec3;
				//normals [v + 2] = new Vector3( 0, 0, s );
				//uvs     [v + 2] = new Vector2( s, s    );	

				vertices[v + 3] = new Vector3( s, -s, 0) + posVec3;
				//normals [v + 3] = new Vector3( 0, 0, s );
				//uvs     [v + 3] = new Vector2( s, 0    );

				indices[i]     = v;
				indices[i + 1] = v + 1;
				indices[i + 2] = v + 2;

				indices[i + 3] = v + 2;
				indices[i + 4] = v + 3;
				indices[i + 5] = v + 0;

				v += 4;
				i += 6;

				// Move down column by 1
				posVec3 += new Vector3(w, 0, 0);
			}

			// Reset column and move down 1 row
			posVec3 += new Vector3(-w * size, w, 0);
		}

		v = 0;

		for (int z = 0; z < size; z++)
			for (int x = 0; x < size; x++)
			{
				// Increasing the fractal weighted strength to the maximum value (1) seems
				// to get rid of any river loops, mini lakes and other artifacts
				var noise = RiverNoise.GetNoise2d(chunkPos.x + x, chunkPos.y + z);

				if (noise is > 0f and < 0.05f)
					ColorTile(colors, v, Colors.CornflowerBlue);

				v += 4;
			}

		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);
		arrays[(int)Mesh.ArrayType.Vertex] = vertices;
		//arrays[(int)Mesh.ArrayType.Normal] = normals;
		//arrays[(int)Mesh.ArrayType.TexUv] = uvs;
		arrays[(int)Mesh.ArrayType.Color] = colors;
		arrays[(int)Mesh.ArrayType.Index] = indices;

		var mesh = new ArrayMesh();
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

		var meshInstance = new MeshInstance2D { Mesh = mesh };
		AddChild(meshInstance);
	}

	private int CalculateSeed(string seedString)
	{
		var seed = 0;

		// Convert each character to a number and add them up
		seedString.ForEach(c => {
			var num = c + 0;
			seed += num;
		});

		return seed;
	}

	private BiomeType[,] GenerateBiomeData(Vector2 chunkCoords, WorldSettings settings)
	{
		// GenerateBiomeData function is called in a loop
		// Looking up all these values from Values dictionary seems unoptimal
		// Especially calculating the world seed everytime
		var chunkSize = int.Parse((string)settings.Values["ChunkSize"]);
		var biomeData = new BiomeType[chunkSize, chunkSize];

		var seed = CalculateSeed((string)settings.Values["Seed"]);

		MoistureNoise.Frequency = (float)settings.Values["MoistureFrequency"];
		MoistureNoise.FractalOctaves = Convert.ToInt32(settings.Values["MoistureOctaves"]);
		MoistureNoise.Seed = seed;
		MoistureNoise.DomainWarpEnabled = true;
		MoistureNoise.DomainWarpAmplitude = (float)settings.Values["MoistureDomainWarpAmplitude"];

		HeatNoise.Frequency = (float)settings.Values["TemperatureFrequency"];
		HeatNoise.FractalOctaves = Convert.ToInt32(settings.Values["TemperatureOctaves"]);
		HeatNoise.Seed = seed + 1000;
		HeatNoise.DomainWarpEnabled = true;
		HeatNoise.DomainWarpAmplitude = (float)settings.Values["TemperatureDomainWarpAmplitude"];

		var chunkPos = chunkCoords * chunkSize;
		var moistureWet = (float)settings.Values["MoistureWet"];
		var moistureDry = (float)settings.Values["MoistureDry"];
		var moistureStrength = (float)settings.Values["MoistureStrength"];

		var temperatureHot = (float)settings.Values["TemperatureHot"];
		var temperatureCold = (float)settings.Values["TemperatureCold"];
		var temperatureStrength = (float)settings.Values["TemperatureStrength"];

		var moistureHeatData = new MoistureHeat[chunkSize, chunkSize];

		for (int x = 0; x < chunkSize; x++)
			for (int z = 0; z < chunkSize; z++)
			{
				var moistureValue = Mathf.Clamp
					(
						(MoistureNoise.GetNoise2d(chunkPos.x + x, chunkPos.y + z) + 1 
							+ moistureWet
							- moistureDry) * (2 - moistureStrength)
							, 0, 1
					);

				var heatValue = Mathf.Clamp
					(
						(HeatNoise.GetNoise2d(chunkPos.x + x, chunkPos.y + z) + 1 
							+ temperatureHot
							- temperatureCold) * (2 - temperatureStrength)
							, 0, 1
					);

				// Generate the tiles
				var biome = GetBiome(moistureValue, heatValue);
				biomeData[x, z] = biome;

				moistureHeatData[x, z] = new MoistureHeat
				{
					Moisture = moistureValue,
					Heat = heatValue
				};

				// Store information about each tile
				/*var tile = new Tile();
				tile.Moisture = moistureValue;
				tile.Heat = heatValue;
				tile.BiomeType = biome;

				Tiles[new Vector2(x, z)] = tile;*/
			}

		return biomeData;
	}

	private BiomeType GetBiome(float moistureNoise, float heatNoise)
	{
		var moistureType = moistureNoise.Remap(0, 1, 0, BiomeTable.GetLength(0) - 1);
		var heatType = heatNoise.Remap(0, 1, 0, BiomeTable.GetLength(0) - 1);

		return BiomeTable[(int)moistureType, (int)heatType];
	}
}
