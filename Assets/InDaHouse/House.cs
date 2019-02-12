using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public float width = 1;
    public float height = 1;
    public float depth = 2;
    public float p = 0.5f;

    private Vector3 origin = Vector3.zero;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Mesh mesh;

    public Material[] materials;

    void OnDrawGizmos()
    {
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
            mesh = new Mesh { name = "HouseParty" };

        //--- MeshRenderer holen
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (materials == null)
        {
            materials = new Material[3];
            materials[0] = new Material(Shader.Find("Standard"));
            materials[0].mainTexture = Resources.Load("nyan") as Texture;
            materials[1] = new Material(Shader.Find("Standard"));
            materials[1].mainTexture = Resources.Load("nyan") as Texture;
            materials[2] = new Material(Shader.Find("Standard"));
            materials[2].mainTexture = Resources.Load("texture") as Texture;

        }

        meshRenderer.sharedMaterials = materials;

        /* Mesh zusammenstellen */
        //--- Vertices/Points

        mesh.Clear();
        mesh.subMeshCount = 3;

        var vertices = new List<Vector3>();

        var walls = new List<Vector3> {
            //--- Left
            VectorFromOrigin(new Vector3(0, 0, depth)),
            VectorFromOrigin(new Vector3(0, height * p, depth)),
            origin,
            VectorFromOrigin(new Vector3(0, height * p, 0)),
            //--- Right
            VectorFromOrigin(new Vector3(width, 0, 0)),
            VectorFromOrigin(new Vector3(width, height * p, 0)),
            VectorFromOrigin(new Vector3(width, 0, depth)),
            VectorFromOrigin(new Vector3(width, height * p, depth))
        };

        var wallTriangles = new List<int> {
            0, 1, 2,
            2, 1, 3,
            4, 5, 6,
            6, 5, 7
        };
        int numbersUsed = walls.Count;

        var roof = new List<Vector3> {
            //--- Left Roof
            VectorFromOrigin(new Vector3(0, height * p, depth)),
            VectorFromOrigin(new Vector3(width/2, height, depth)),
            VectorFromOrigin(new Vector3(0, height * p, 0)),
            VectorFromOrigin(new Vector3(width/2, height, 0)),
            //---Right Roof
            VectorFromOrigin(new Vector3(width, height * p, 0)),
            VectorFromOrigin(new Vector3(width/2, height, 0)),
            VectorFromOrigin(new Vector3(width, height * p, depth)),
            VectorFromOrigin(new Vector3(width/2, height, depth))
        };

        var roofTriangles = new List<int> {
            0 + numbersUsed, 1 + numbersUsed, 2 + numbersUsed,
            2 + numbersUsed, 1 + numbersUsed, 3 + numbersUsed,
            4 + numbersUsed, 5 + numbersUsed, 6 + numbersUsed,
            6 + numbersUsed, 5 + numbersUsed, 7 + numbersUsed,
        };

        numbersUsed += roof.Count;

        var pediment = new List<Vector3> {
            origin,
            VectorFromOrigin(new Vector3(0, height * p, 0)),
            VectorFromOrigin(new Vector3(width, 0, 0)),
            VectorFromOrigin(new Vector3(width, height * p, 0)),
            VectorFromOrigin(new Vector3(width/2, height, 0)),

            VectorFromOrigin(new Vector3(width, 0, depth)),
            VectorFromOrigin(new Vector3(width, height * p, depth)),
            VectorFromOrigin(new Vector3(0, 0, depth)),
            VectorFromOrigin(new Vector3(0, height * p, depth)),
            VectorFromOrigin(new Vector3(width/2, height, depth)),
        };

        var pedimeterTriangles = new List<int> {
            0 + numbersUsed, 1 + numbersUsed, 2 + numbersUsed,
            2 + numbersUsed, 1 + numbersUsed, 3 + numbersUsed,
            1 + numbersUsed, 4 + numbersUsed, 3 + numbersUsed,


            4 + numbersUsed + 1, 5 + numbersUsed + 1, 6 + numbersUsed + 1,
            6 + numbersUsed + 1, 5 + numbersUsed + 1, 7 + numbersUsed + 1,
            5 + numbersUsed + 1, 8 + numbersUsed + 1, 7 + numbersUsed + 1,
        };

        vertices.AddRange(walls);
        vertices.AddRange(roof);
        vertices.AddRange(pediment);

        //--- UVs
        var uvs = new List<Vector2>() {
            //--- Walls
            new Vector2(0f, 0f),
            new Vector2(0f, height * p),
            new Vector2(depth, 0f),
            new Vector2(depth, height * p),
            new Vector2(0f, 0f),
            new Vector2(0f, height * p),
            new Vector2(depth, 0f),
            new Vector2(depth, height * p),

            //--- Roof
            new Vector2(0f, 0f),
            new Vector2(0f, depth),
            new Vector2(Vector3.Distance(new Vector3(0, height * p, 0), new Vector3(width/2, height, 0)), 0f),
            new Vector2(Vector3.Distance(new Vector3(0, height * p, 0), new Vector3(width/2, height, 0)), depth),
            new Vector2(0f, 0f),
            new Vector2(0f, depth),
            new Vector2(Vector3.Distance(new Vector3(0, height * p, 0), new Vector3(width/2, height, 0)), 0f),
            new Vector2(Vector3.Distance(new Vector3(0, height * p, 0), new Vector3(width/2, height, 0)), depth),

            //--- Pedimeter
            new Vector2(0f, 0f),
            new Vector2(width, 0f),
            new Vector2(0f, height * p),
            new Vector2(width/2, height),
            new Vector2(width, height * p),
            new Vector2(0f, 0f),
            new Vector2(width, 0f),
            new Vector2(0f, height * p),
            new Vector2(width/2, height),
            new Vector2(width, height * p),
        };

        //--- Triangles
        
        //mesh.SetVertices(walls);
        //mesh.SetVertices(roof);
        //mesh.SetVertices(pediment);
        mesh.vertices = vertices.ToArray();

        //--- Jonas stinkt
        //--- Triangles zuweisen
        //mesh.triangles = triangles.ToArray();
        mesh.SetTriangles(wallTriangles, 0);
        mesh.SetTriangles(roofTriangles, 1);
        mesh.SetTriangles(pedimeterTriangles, 2);

        //--- Mesh dem MeshFilter zuweisen
        meshFilter.sharedMesh = mesh;
    }

    public Vector3 VectorFromOrigin(Vector3 to)
    {
        return origin + (to - origin);
    }

}
