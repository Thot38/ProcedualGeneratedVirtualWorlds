using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random2DOutOfPlaceGenerator : IGenerator
{
    private float landMasses;
    private int numberOfSmoothings;
    private int numberOfAdjacentsIn2D;
    private float increment;
    private System.Random _random;
    private OpenSimplexNoise _openSimplexNoise;

    public Random2DOutOfPlaceGenerator(float increment, int numberOfAdjacentsIn2D, int numberOfSmoothings, float landMasses, string seed)
    {
        this.landMasses = landMasses;
        this.numberOfSmoothings = numberOfSmoothings;
        this.numberOfAdjacentsIn2D = numberOfAdjacentsIn2D;
        _random = new System.Random(seed.GetHashCode());
        this.increment = increment;
        _openSimplexNoise = new OpenSimplexNoise(seed.GetHashCode());
    }

    public void Border(float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if ((x == 1 || x == width - 2) || (y == 1 || y == height - 2) || (z == 1 || z == depth - 2))
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
                    map[x, y, z] = _openSimplexNoise.eval(x * increment, y * increment, z * increment) + 1 > landMasses ? 0 : 1;
                }
            }
        }

        for (int i = 0; i < numberOfSmoothings; i++)
        {
           map = Smooth2D(ref map, width, height, depth);
        }
    }

    private float[,,] Smooth2D(ref float[,,] map, int width, int height, int depth)
    {
        var resultMap = new float[width, height, depth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (NumberOfAdjacents2D(new Vector3Int(x, y, 0), map, width, height, depth) > numberOfAdjacentsIn2D)
                        resultMap[x, y, z] = 1;
                    else if (NumberOfAdjacents2D(new Vector3Int(x, y, 0), map, width, height, depth) < numberOfAdjacentsIn2D)
                        resultMap[x, y, z] = 0;
                }
            }
        }
        return resultMap;
    }

    private int NumberOfAdjacents2D(Vector3Int point, float[,,] map, int width, int height, int depth)
    {
        var result = 0;
        for (int x = point.x - 1; x <= point.x + 1; x++)
        {
            for (int y = point.y - 1; y <= point.y + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != point.x || y != point.y)
                    {
                        result += (int)map[x, y, point.z];
                    }
                }
                else
                {
                    result++;
                }
            }
        }
        return result;
    }
}
