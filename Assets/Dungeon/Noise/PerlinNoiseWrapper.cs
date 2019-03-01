using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseWrapper : INoise
{
    public float Noise(int x, int y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    public float Noise(int x, int y, int z, int size)
    {
        return Mathf.PerlinNoise(Mathf.Floor(Mathf.PerlinNoise(x, y) * size), z);
    }

    public float Noise(int[] coordinates)
    {
        if (coordinates.Length != 2)
            throw new System.ArgumentException("Perlin noise only works in 2 Dimensions in Unity");

        return Mathf.PerlinNoise(coordinates[0], coordinates[1]);
    }
}
