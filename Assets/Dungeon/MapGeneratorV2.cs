using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGeneratorV2 : MonoBehaviour
{
    public int width = 32;
    public int height = 32;
    public int depth = 32;
    public int numberOfSmoothings = 5;
    public float landMasses = 0.47f;

    public Material material;

    public Marching marching = new MarchingCubeAlgorithm();

    public bool Visualize = true;

    [Range(0f, 26f)]
    public int numberOfAdjacent = 12;
    [Range(0f, 5f)]
    public int variance = 1;
    [Range(-1f, 1f)]
    public float surface = 0;
    [Range(0f, 8f)]
    public int seedSize = 0;

    float[,,] map;

    public bool useRandomSeed = true;
    public string seed;

    public void Init()
    {
        map = new float[width, height, depth];

        if (useRandomSeed)
            seed = DateTime.Now.ToString();

    }

    public void GenerateCrazyShit(int rule)
    {
        Init();
        if (rule == 0)
        {
            for (int x = width / 2 - width / 4; x < width / 2 + width / 4; x++)
            {
                for (int y = height / 2 - height / 4; y < height / 2 + height / 4; y++)
                {
                    for (int z = depth / 2 - depth / 4; z < depth / 2 + depth / 4; z++)
                    {
                        if (x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth)
                        {
                            map[x, y, z] = 1;
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (Vector3.Distance(new Vector3Int(width / 2, height / 2, depth / 2), new Vector3Int(x, y, z)) < 4)
                        {
                            map[x, y, z] = 1;
                        }
                        else
                            map[x, y, z] = 0;
                    }
                }
            }
        }
    }

    public void GenerateMap()
    {
        Init();

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
                        map[x, y, z] = (float)pseudoRandomGenerator.NextDouble();
                    }
                }
            }
        }

        for (int i = 0; i < numberOfSmoothings; i++)
        {
            SmoothMap(pseudoRandomGenerator);
        }

        for (int x = width / 2 - seedSize; x < width / 2 + seedSize; x++)
        {
            for (int y = height / 2 - seedSize; y < height / 2 + seedSize; y++)
            {
                for (int z = depth / 2 - seedSize; z < depth / 2 + seedSize; z++)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth)
                    {
                        map[x, y, z] = 1;
                    }
                }
            }
        }

    }

    public void CreateMesh()
    {
        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }

        marching.Surface = surface;
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
                int index = i * maxVerticesPerMesh + j;

                if (index < verticies.Count)
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
            go.GetComponent<MeshRenderer>().material = material;
            meshFilter.sharedMesh = mesh;

            // go.transform.localPosition = new Vector3(-width / 2, -height / 2, -depth / 2);
            var illuminationDevice = gameObject.AddComponent<Light>();
            illuminationDevice.color = new Color(160, 82, 45);
            illuminationDevice.transform.position = gameObject.transform.TransformPoint(new Vector3(-width / 2, -height / 2, -depth / 2));
        }
    }

    public void SmoothMap(System.Random random)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (NumberOfAdjacents(new Vector3Int(x, y, z)) > numberOfAdjacent + random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z)) < numberOfAdjacent + random.Next(-variance, variance))
                        map[x, y, z] = 0;
                }
            }
        }
    }

    public void SmoothMap2(System.Random random)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (NumberOfAdjacents(new Vector3Int(x, y, z)) > numberOfAdjacent + random.Next(-variance, variance))
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z)) > 5 && NumberOfAdjacents(new Vector3Int(x, y, z)) < 8)
                        map[x, y, z] = 1;
                    else if (NumberOfAdjacents(new Vector3Int(x, y, z)) < numberOfAdjacent + random.Next(-variance, variance))
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

    public void OnDrawGizmos()
    {
        if (map == null)
            GenerateMap();
        if (!Visualize)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (map[x, y, z] > 0)
                    {
                        Gizmos.color = PickColor(map[x, y, z]);
                        var posTransformed = gameObject.transform.TransformPoint(new Vector3(x, y, z));
                        Gizmos.DrawCube(posTransformed, new Vector3(1, 1, 1));

                    }
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
