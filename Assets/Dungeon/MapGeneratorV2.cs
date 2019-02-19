using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGeneratorV2 : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public int depth = 8;
    public float landMasses = 0.47f;

    public Marching marching = new MarchingCubeAlgorithm();

    [Range(0f, 26f)]
    public int numberOfAdjacent = 12;

    float[,,] map;

    public bool useRandomSeed = true;
    public string seed;

    public void GenerateMap()
    {
        Debug.Log("Start");
        map = new float[width, height, depth];


        marching.Surface = 0.0f;

        if (useRandomSeed)
            seed = DateTime.Now.ToString();

        var pseudoRandomGenerator = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    map[x, y, z] = pseudoRandomGenerator.NextDouble() > landMasses ? 0 : 1;
                    if ((x == 0 || x == width - 1) && (y == 0 || y == height - 1) && (z == 0 || z == depth - 1))
                    {
                        map[x, y, z] = 1;
                    }
                }
            }
        }

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        var verticies = new List<Vector3>();
        var indices = new List<int>();

        marching.Generate(map, width, height, depth, verticies, indices);

        //--- Building the Mesh

        var maxVerticesPerMesh = 30000;
        int numberOfMeshes = verticies.Count / maxVerticesPerMesh + 1;

        for (int i = 0; i < numberOfMeshes; i++)
        {
            var splitVertices = new List<Vector3>();
            var splitIndices = new List<int>();

            for (int j = 0; j < maxVerticesPerMesh; j++)
            {
                int index = i * maxVerticesPerMesh +j;

                if(index < verticies.Count)
                {
                    splitVertices.Add(verticies[index]);
                    splitIndices.Add(j);
                }
            }

            if (splitVertices.Count == 0) continue;

            Mesh mesh = new Mesh();
            mesh.SetVertices(splitVertices);
            mesh.SetTriangles(splitIndices, 0);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            GameObject go = new GameObject("Meeesh");
            go.transform.parent = transform;
            var meshFilter = go.AddComponent<MeshFilter>();
            var meshRenderer = go.AddComponent<MeshRenderer>();
            //  go.GetComponent<MeshRenderer>().material = new Material
            meshFilter.sharedMesh = mesh;

            go.transform.localPosition = new Vector3(-width / 2, -height / 2, -depth / 2);

        }

    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (NumberOfAdjacents(new Vector3Int(x, y, z)) > numberOfAdjacent)
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z)) < numberOfAdjacent)
                        map[x, y, z] = 0;
                }
            }
        }
    }

    private int NumberOfAdjacents(Vector3Int point)
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
                        if (x != point.x || y != point.y)
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

    public void OnDrawGizmos()
    {
        if (map == null)
            GenerateMap();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Gizmos.color = PickColor(map[x, y, z]);
                    if (map[x, y, z] > 0)
                        Gizmos.DrawCube(new Vector3(x, y, z), new Vector3(1, 1, 1));
                }
            }
        }
    }

    private Color PickColor(float v)
    {
        switch (v)
        {
            case 0: return Color.black;
            case 1: return Color.white;
            case 2: return Color.red;
            case 3: return Color.blue;
            default: return Color.cyan;
        }
    }
}
