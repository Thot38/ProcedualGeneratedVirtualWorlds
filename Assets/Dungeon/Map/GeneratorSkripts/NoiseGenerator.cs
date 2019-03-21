using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : IGenerator
{
    private float increment;
    private System.Random _random;
    private OpenSimplexNoise _openSimplexNoise;

    public NoiseGenerator(float increment, string seed)
    {
        _random = new System.Random(seed.GetHashCode());
        _openSimplexNoise = new OpenSimplexNoise(seed.GetHashCode());
        this.increment = increment;
    }

    public void Border(float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if ((x == 1 || x ==width - 2) || (y == 1 || y == height - 2) || (z == 1 || z == depth - 2))
                        map[x, y, z] = (float)_openSimplexNoise.eval((x + z) * increment, (y + z) * increment, _random.NextDouble() * increment);
                    if ((x == 0 || x == width - 1) || (y == 0 || y == height - 1) || (z == 0 || z == depth - 1))
                        map[x, y, z] = 1;
                }
            }
        }
    }

    public void Fill(ref float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    var randomNoisiator = _random.NextDouble() * increment * 2;
                    var n = (float)_openSimplexNoise.eval(x * increment, y * increment, z * increment, randomNoisiator);
                    map[x, y, z] = n > 0 ? n : 0;
                }
            }
        }
    }
}
