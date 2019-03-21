using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGeneratorV3 : MonoBehaviour
{
    public int width = 64;
    public int height = 64;
    public int depth = 32;

    public Material material;
    public Marching marching = new MarchingCubeAlgorithm();

    public bool visualize = true;

    public float surface = 0;

    public Map map;

    public void GenerateMap(IGenerator generator)
    {
        map = new Map(width, height, depth, generator);
        map.Fill();
        map.Border();
        visualize = true;
    }

    public void OnDrawGizmos()
    {
        if (map == null)
            return;
        if (!visualize)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (map.Get(x, y, z) > surface)
                    {
                        var posTransformed = gameObject.transform.TransformPoint(new Vector3(x, y, z));
                        Gizmos.DrawCube(posTransformed, new Vector3(1, 1, 1));
                    }
                }
            }
        }
    }

    public void CreateMesh()
    {
        if (material == null)
        {
            var shader = Shader.Find("Custom/StandardVertex");
            material = new Material(shader);
            material.SetColor("_Color", new Color(0.4622642f, 0.2678384f, 0.1940637f, 1));
        }
        visualize = false;

        marching.Surface = surface;
        var verticies = new List<Vector3>();
        var indices = new List<int>();

        marching.Generate(map.Get(), width, height, depth, verticies, indices);

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
        }
    }

    public enum MapGenerators
    {
        NoiseGenerator,
        RandomGenerator,
        Random2DGenerator,
        RandomOutOfPlaceGenerator,
        Random2DOutOfPlaceGenerator,
        NoiseRandomGenerator,

    }
}
