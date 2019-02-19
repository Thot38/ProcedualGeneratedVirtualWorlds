using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Marching : IMarching
{
    public float Surface { get; set; }

    //--- the eight neighbours of the Point
    private float[] Cube { get; set; }


    //--- Weather a Triangel is wound up 0,1,2 or 2,1,0
    protected int[] WindingOrder { get; private set; }

    protected static readonly int[,] VertexOffsets = new int[,]
        {
            { 0, 0, 0 },{ 1, 0, 0 },{ 1, 1, 0 },{ 0, 1, 0 },
            { 0, 0, 1 },{ 1, 0, 1 },{ 1, 1, 1 },{ 0, 1, 1 },
        };

    public Marching(float surface = 0.5f)
    {
        Surface = surface;
        Cube = new float[8];
        WindingOrder = new int[] { 0, 1, 2 };
    }

    public virtual void Generate(float[,,] mapPoints, int width, int height, int depth, IList<Vector3> verticies, IList<int> indices)
    {
        if (Surface > 0f)
        {
            WindingOrder[0] = 0;
            WindingOrder[1] = 1;
            WindingOrder[2] = 2;
        }
        else
        {
            WindingOrder[0] = 2;
            WindingOrder[1] = 1;
            WindingOrder[2] = 0;
        }

        //--- Get the eight Neightbours and fill Cube
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                for (int z = 0; z < depth - 1; z++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        var _x = x + VertexOffsets[i, 0];
                        var _y = y + VertexOffsets[i, 1];
                        var _z = z + VertexOffsets[i, 2];

                        Cube[i] = mapPoints[_x, _y, _z];
                    }

                    March(x, y, z, Cube, verticies, indices);
                }
            }
        }
    }

    protected abstract void March(float x, float y, float z, float[] cube, IList<Vector3> vertices, IList<int> indices);

    protected virtual float GetOffset(float v1, float v2)
    {
        float delta = v2 - v1;
        return (delta == 0.0f) ? Surface : (Surface - v1) / delta;
    }
}
