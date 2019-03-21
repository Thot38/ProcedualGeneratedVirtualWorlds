using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : IGenerator
{
    private System.Random _random;
    private float landMasses;
    private int numberOfAdjacents;
    private int variance;
    private int numberOfSmoothings;

    public RandomGenerator(float landMasses, int numberOfAdjacents, int variance, int numberOfSmoothings, string seed)
    {
        _random = new System.Random(seed.GetHashCode());
        this.landMasses = landMasses;
        this.numberOfAdjacents = numberOfAdjacents;
        this.variance = variance;
        this.numberOfSmoothings = numberOfSmoothings;
    }

    public void Border(float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
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
                    map[x, y, z] = _random.NextDouble() > landMasses ? 0 : 1;
                    //if ((x == 0 || x == width - 1) && (y == 0 || y == height - 1) && (z == 0 || z == depth - 1))
                    //{
                    //    map[x, y, z] = (float)_random.NextDouble();
                    //}
                }
            }
        }

        for (int i = 0; i < numberOfSmoothings; i++)
        {
            Smooth(ref map, width, height, depth);
        }
    }

    private void Smooth(ref float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) > numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) < numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 0;
                }
            }
        }
    }

    private void SameSmoothDifferentIteration(float[,,] map, int width, int height, int depth)
    {
        var debug = 0;
        for (int x = width / 2 + 1; x != width / 2; x++)
        {
            x = x % width;
            for (int y = height / 2 + 1; y != height / 2; y++)
            {
                y = y % height;
                for (int z = depth / 2 + 1; z != depth / 2; z++)
                {
                    z = z % depth;
                    debug++;
                    if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) > numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) < numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 0;
                }
            }
        }
    }

    private void StillTheSameSmooth(float[,,] map, int width, int height, int depth)
    {
        var debug = 0;
        var x = _random.Next(width);
        var xCount = 0;
        while (xCount < width)
        {
            x = x % width;
            var y = _random.Next(height);
            var yCount = 0;
            while (yCount < height)
            {
                var z = _random.Next(depth);
                var zCount = 0;
                while (zCount < depth)
                {
                    debug++;
                    if (debug > height * width * depth * 2)
                        return;
                    if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) > numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) < numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 0;
                    zCount++;
                    z++;
                    z = z % depth;
                }
                yCount++;
                y++;
                y = y % height;
            }
        }
    }

    private void YetAnotherSmoothingAlgorithm(float[,,] map, int width, int height, int depth)
    {
        for (int x = 0; x < width; x++)
        {
            var y = _random.Next(height);
            var yCount = 0;
            while (yCount < height)
            {
                var z = _random.Next(depth);
                var zCount = 0;
                while (zCount < depth)
                {
                    if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) > numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) < numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 0;
                    zCount++;
                    z++;
                    z = z % depth;
                }
                yCount++;
                y++;
                y = y % height;
            }
        }
    }

    private void TheSmoothTheSmoothAndNothingButTheSmooth(float[,,] map, int width, int height, int depth)
    {
        var debug = 0;
        var x = _random.Next(width);
        var xCount = 0;
        while (xCount < width)
        {
            x = x % width;
            for (int y = 0; y < height; y++)
            {
                var z = _random.Next(depth);
                var zCount = 0;
                while (zCount < depth)
                {
                    debug++;
                    if (debug > height * width * depth * 2)
                        return;
                    if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) > numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z), map, width, height, depth) < numberOfAdjacents + _random.Next(-variance, variance))
                        map[x, y, z] = 0;
                    zCount++;
                    z++;
                    z = z % depth;
                }
            }
        }
    }

    private int NumberOfAdjacents(Vector3Int point, float[,,] map, int width, int height, int depth)
    {
        var result = 0;
        for (int x = point.x - 1; x <= point.x + 1; x++)
        {
            for (int y = point.y - 1; y <= point.y + 1; y++)
            {
                for (int z = point.z - 1; z <= point.z + 1; z++)
                {

                    if (x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth)
                    {
                        if (x != point.x || y != point.y || z != point.z)
                        {
                            result += (int)map[x, y, z];
                        }
                    }
                    else
                    {
                        result++;
                    }
                }
            }
        }
        return result;
    }
}
