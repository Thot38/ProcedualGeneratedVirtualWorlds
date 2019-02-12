using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private static float _sideLength = 1;

    public Vector3 center = Vector3.zero;
    public List<Vector3> corners = new List<Vector3>();

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Mesh mesh;

    public Material material;
    public Texture texture;

    public Cube()
    {
        corners.Clear();
        // --- ForntFace of Cube from Bottom Left,Top Left, Bottom Right, Top Right
        corners.Add(center + new Vector3(+_sideLength / 2, -_sideLength / 2, -_sideLength / 2));
        corners.Add(center + new Vector3(+_sideLength / 2, +_sideLength / 2, -_sideLength / 2));
        corners.Add(center + new Vector3(+_sideLength / 2, -_sideLength / 2, +_sideLength / 2));
        corners.Add(center + new Vector3(+_sideLength / 2, +_sideLength / 2, +_sideLength / 2));

        // --- Back face of Cube
        corners.Add(center + new Vector3(-_sideLength / 2, -_sideLength / 2, -_sideLength / 2));
        corners.Add(center + new Vector3(-_sideLength / 2, +_sideLength / 2, -_sideLength / 2));
        corners.Add(center + new Vector3(-_sideLength / 2, -_sideLength / 2, +_sideLength / 2));
        corners.Add(center + new Vector3(-_sideLength / 2, +_sideLength / 2, +_sideLength / 2));

    }

    public List<Vector3> Corners
    {
        get
        {
            return corners;
        }
    }

    void OnDrawGizmos()
    {
        var cornersTransformed = TransformPosition(corners);

        foreach (var c in cornersTransformed)
        {
            Gizmos.DrawSphere(c, 0.1f);
        }

        for (int i = 0; i < 2; i++)
        {
            Gizmos.DrawLine(cornersTransformed[0 + i * 4], cornersTransformed[1 + i * 4]);
            Gizmos.DrawLine(cornersTransformed[3 + i * 4], cornersTransformed[2 + i * 4]);
            Gizmos.DrawLine(cornersTransformed[1 + i * 4], cornersTransformed[3 + i * 4]);
            Gizmos.DrawLine(cornersTransformed[2 + i * 4], cornersTransformed[0 + i * 4]);

        }

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(cornersTransformed[i], cornersTransformed[i + 4]);
        }

        CreateMesh();

    }

    public void CreateMesh()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        //--- vom MeshFilter zu Mesh
        mesh = meshFilter.sharedMesh;
        if (mesh == null)
            mesh = new Mesh { name = "Cube" };

        //--- MeshRenderer holen
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        material = new Material(Shader.Find("Standard"));
        texture = Resources.Load<Texture>("texture");

        material.mainTexture = texture;

        meshRenderer.sharedMaterials = new Material[] { material };

        //--- Mesh zusammenstellen
        //--- Vertices/Points
        var verticies = new List<Vector3>
        {
            //corners[7],
            //corners[3],

            //corners[2],

            //corners[3],
            //corners[7],
            //corners[6],
            //corners[7],
            //corners[5],
            //corners[4],
            //corners[5],
            //corners[1],
            //corners[0],
            //corners[1],
            //corners[5],
            NewVector(corners[0]),
            NewVector(corners[1]),
            NewVector(corners[2]),
            NewVector(corners[3]),
            NewVector(corners[4]),
            NewVector(corners[5]),
            NewVector(corners[6]),
            NewVector(corners[7]),

            NewVector(corners[5]),
            NewVector(corners[1]),
            NewVector(corners[4]),
            NewVector(corners[0]),

            //---
            NewVector(corners[6]),
            NewVector(corners[2]),
            NewVector(corners[7]),
            NewVector(corners[3]),

            NewVector(corners[4]),
            NewVector(corners[0]),
            NewVector(corners[6]),
            NewVector(corners[2]),
            //---
            NewVector(corners[1]),
            NewVector(corners[5]),
            NewVector(corners[3]),
            NewVector(corners[7])
        };

        var uvs = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
        };
        //--- Triangles
        var triangles = new List<int>();
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(2);
        triangles.Add(1);
        triangles.Add(3);

        triangles.Add(6);
        triangles.Add(7);
        triangles.Add(4);
        triangles.Add(4);
        triangles.Add(7);
        triangles.Add(5);

        triangles.Add(0 + 8);
        triangles.Add(1 + 8);
        triangles.Add(2 + 8);
        triangles.Add(2 + 8);
        triangles.Add(1 + 8);
        triangles.Add(3 + 8);

        triangles.Add(4 + 8);
        triangles.Add(5 + 8);
        triangles.Add(6 + 8);
        triangles.Add(6 + 8);
        triangles.Add(5 + 8);
        triangles.Add(7 + 8);

        triangles.Add(0 + 16);
        triangles.Add(1 + 16);
        triangles.Add(2 + 16);
        triangles.Add(2 + 16);
        triangles.Add(1 + 16);
        triangles.Add(3 + 16);

        triangles.Add(4 + 16);
        triangles.Add(5 + 16);
        triangles.Add(6 + 16);
        triangles.Add(6 + 16);
        triangles.Add(5 + 16);
        triangles.Add(7 + 16);


        //triangles.Add(5);
        //triangles.Add(4);
        //triangles.Add(7);
        //triangles.Add(7);
        //triangles.Add(4);
        //triangles.Add(6);

        //triangles.Add(4);
        //triangles.Add(0);
        //triangles.Add(6);
        //triangles.Add(6);
        //triangles.Add(0);
        //triangles.Add(2);

        //triangles.Add(0);
        //triangles.Add(1);
        //triangles.Add(2);
        //triangles.Add(2);
        //triangles.Add(1);
        //triangles.Add(3);

        //triangles.Add(1);
        //triangles.Add(5);
        //triangles.Add(3);
        //triangles.Add(3);
        //triangles.Add(5);
        //triangles.Add(7);

        //triangles.Add(5);
        //triangles.Add(1);
        //triangles.Add(4);
        //triangles.Add(4);
        //triangles.Add(1);
        //triangles.Add(0);

        //triangles.Add(6);
        //triangles.Add(2);
        //triangles.Add(7);
        //triangles.Add(7);
        //triangles.Add(2);
        //triangles.Add(3);

        //--- Mesh befüllen
        mesh.Clear();
        //--- Vertices zuweisen
        mesh.vertices = verticies.ToArray();
        mesh.uv = uvs;

        //--- Jonas stinkt
        //--- Triangles zuweisen
        mesh.triangles = triangles.ToArray();
        //--- Mesh dem MeshFilter zuweisen

        meshFilter.sharedMesh = mesh;

        //triangles.Add(0);
        //triangles.Add(1);
        //triangles.Add(2);
        //triangles.Add(2);
        //triangles.Add(1);
        //triangles.Add(3);

    }

    private Vector3 NewVector(Vector3 old)
    {
        return new Vector3(old.x, old.y, old.z);
    }

    public List<Vector3> TransformPosition(List<Vector3> positions)
    {
        var result = new List<Vector3>();
        foreach (var pos in positions)
        {
            result.Add(transform.TransformPoint(pos));
        }
        return result;
    }
}
