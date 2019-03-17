using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGeneratorV3 : MonoBehaviour
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

    public void GenerateMap()
    {

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
