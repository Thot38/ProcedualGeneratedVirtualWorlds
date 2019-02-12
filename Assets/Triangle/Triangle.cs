using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    public Vector3 P0 = new Vector3(0, 0, 0);
    public Vector3 P1 = new Vector3(0, 0, 1);
    public Vector3 P2 = new Vector3(1, 0, 0);

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Mesh mesh;


    //public Vector3[] points = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };

    public void CreateMesh()
    {
        //--- MeshFilter holen
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        //--- vom MeshFilter zu Mesh
        mesh = meshFilter.sharedMesh;
        if(mesh == null)
            mesh = new Mesh { name = "Triangle" };

        //--- MeshRenderer holen
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        if(meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        //--- Mesh zusammenstellen
        //--- Vertuces/Points
        var verticies = new Vector3[] { P0, P1, P2 };

        //--- Triangles
        var triangles = new int[] { 0, 1, 2 };
        
        //--- Mesh befüllen
        mesh.Clear();
        //--- Vertices zuweisen
        mesh.vertices = verticies;
        //--- Jonas stinkt
        //--- Triangles zuweisen
        mesh.triangles = triangles;
        //--- Mesh dem MeshFilter zuweisen

        meshFilter.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {
        //for (int i = 0; i < points.Length; i++)
        //{
        //    points[i] = transform.TransformPoint(points[i]);
        //    points[i] = transform.TransformPoint(points[i]);
        //    points[i] = transform.TransformPoint(points[i]);
        //}
        var P0_transformed = transform.TransformPoint(P0);
        var P1_transformed = transform.TransformPoint(P1);
        var P2_transformed = transform.TransformPoint(P2);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(P0_transformed, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(P1_transformed, 0.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(P2_transformed, 0.1f);

        Gizmos.color = Color.grey;
        Gizmos.DrawLine(P0_transformed, P1_transformed);
        Gizmos.DrawLine(P1_transformed, P2_transformed);
        Gizmos.DrawLine(P2_transformed, P0_transformed);

        CreateMesh();
    }

}