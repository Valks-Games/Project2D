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
	private int Seed { get; set; } = 209323094;
	private float Frequency { get; set; } = 0.02f;

	public override void _Ready()
	{
		Noise.Seed = Seed;
		var noise = Noise.Calc2D(Size, Size, Frequency);

		for (int x = 0; x < Size; x++)
			for (int z = 0; z < Size; z++)
			{
				var amplitude = noise[x, z];

				if (amplitude < 50)
					SetTile(x, z);
				else if (amplitude >= 50 && amplitude <= 200)
					SetTile(x, z, 1);
				else
					SetTile(x, z, 3);
			}
	}

	private void SetTile(int worldX, int worldZ, int tileX = 0, int tileY = 0) =>
		SetCell(0, new Vector2i(-Size / 2, -Size / 2) + new Vector2i(worldX, worldZ), 0, new Vector2i(tileX, tileY));
}
