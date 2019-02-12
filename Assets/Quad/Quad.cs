using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Quad : MonoBehaviour
{
    public Vector3 P0 = Vector3.zero;
    public Vector3 P1 = Vector3.up;
    public Vector3 P2 = Vector3.right;
    public Vector3 P3 = new Vector3(1, 1, 0);

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Mesh mesh;

    public Material material;
    public Texture texture;

    void OnDrawGizmos()
    {
        var P0_transformed = transform.TransformPoint(P0);
        var P1_transformed = transform.TransformPoint(P1);
        var P2_transformed = transform.TransformPoint(P2);
        var P3_transformed = transform.TransformPoint(P3);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(P0_transformed, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(P1_transformed, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(P2_transformed, 0.1f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(P3_transformed, 0.1f);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(P0_transformed, P1_transformed);
        Gizmos.DrawLine(P1_transformed, P2_transformed);
        Gizmos.DrawLine(P2_transformed, P3_transformed);
        Gizmos.DrawLine(P3_transformed, P0_transformed);
        Gizmos.DrawLine(P1_transformed, P3_transformed);

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
            mesh = new Mesh { name = "Quad" };

        //--- MeshRenderer holen
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        material = new Material(Shader.Find("Standard"));
        texture = Resources.Load<Texture>("rings");

        material.mainTexture = texture;

        meshRenderer.sharedMaterials = new Material[] { material };

        //--- Mesh zusammenstellen
        //--- Vertices/Points
        var verticies = new List<Vector3> { P0, P1, P2, P3 };

        var uvs = new Vector2[verticies.Count];
        uvs[0] = new Vector2(0f, 0.5f);
        uvs[1] = new Vector2(0f, 1f);
        uvs[2] = new Vector2(1f, 0f);
        uvs[3] = new Vector2(1f, 1f);

        //--- Triangles
        var triangles = new List<int>();
     
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(2);
        triangles.Add(1);
        triangles.Add(3);

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
    }
}
