using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private float[,,] map;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Depth { get; private set; }

    private IGenerator _generator;

    public Map(int x, int y, int z, IGenerator generator)
    {
        Width = x;
        Height = y;
        Depth = z;
        map = new float[Width, Height, Depth];
        _generator = generator;
    }

    public float Get(int x, int y, int z)
    {
        return map[x, y, z];
    }

    public float[,,] Get()
    {
        return map;
    }

    public void Fill()
    {
        _generator.Fill(ref map, Width, Height, Depth);
    }

    public void Border()
    {
        _generator.Border(map, Width, Height, Depth);
    }
}
