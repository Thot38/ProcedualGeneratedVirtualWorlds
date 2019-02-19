using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public Vector3 GroundP0 = new Vector3(0f, 0f, 0f);
    public Vector3 GroundP1 = new Vector3(0f, 0f, 0f);
    public Vector3 GroundP2 = new Vector3(0f, 0f, 0f);
    public Vector3 GroundP3 = new Vector3(0f, 0f, 0f);

    [Range(0f, 1f)]
    public float u = 1f / 3f;
    [Range(0f, 1f)]
    public float w = 1f / 3f;

    public float height = 10f;
    public float depth = 10f;
    public float width = 10f;

    [Range(1, 100)]
    public int numberOfSegmentsU = 90;
    [Range(1, 100)]
    public int numberOfSegmentsW = 45;

    public Mesh mesh;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;

    public Material material;

    public void OnDrawGizmos()
    {
        // Zeichne die "Umrandung" mit grauen Linien
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(GroundP0, GroundP1);
        Gizmos.DrawLine(GroundP1, GroundP3);
        Gizmos.DrawLine(GroundP3, GroundP2);
        Gizmos.DrawLine(GroundP2, GroundP0);

        // Zeichne die Kontrollpunkte in grün
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GroundP0, 0.1f);
        Gizmos.DrawSphere(GroundP1, 0.1f);
        Gizmos.DrawSphere(GroundP2, 0.1f);
        Gizmos.DrawSphere(GroundP3, 0.1f);

    }

    private Vector3 PointOnBilinearSurface(Vector3 p00, Vector3 p01, Vector3 p10, Vector3 p11, float u, float w)
    {
        var p_1 = PointOnLine(p00, p10, u);
        var p_2 = PointOnLine(p01, p11, u);

        var point = PointOnLine(p_1, p_2, w);

        return point;
    }

    private Vector3 PointOnLine(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    private void Update()
    {
        if (mesh == null)
        {
            CreateMesh();
            SetDoorway(new Vector3(10f, 3f, 15f), Vector3.zero);
        }
    }

    public void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.sharedMesh = mesh = new Mesh();

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }

        SetCorners();

        var numberOfPointsU = numberOfSegmentsU + 1;
        var numberOfPointsW = numberOfSegmentsW + 1;

        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        var stepU = 1f / (float)numberOfSegmentsU;
        var stepW = 1f / (float)numberOfSegmentsW;

        for (int u = 0; u < numberOfPointsU; u++)
        {
            for (int w = 0; w < numberOfPointsW; w++)
            {
                var _u = u * stepU;
                var _w = w * stepW;

                var point = PointOnBilinearSurface(GroundP0, GroundP1, GroundP2, GroundP3, _w, _u);

                point.y = 0f; //--- Todo Height of Point; Somewhat along the lines of (x, z) !between (h,h) and (h+w, h+d) then heigth = Distance(point, (Closest(h,h), (h+w, h+d)))
                if (point.x < height)
                {
                    point.y = height - point.x;
                    point.x = height;
                }
                else if (point.x > height + width)
                {
                    point.y = point.x - (height + width);
                    point.x = (height + width);
                }

                if (point.z < height)
                {
                    point.y = height - point.z;
                    point.z = height;
                }
                else if (point.z > height + depth)
                {
                    point.y = point.z - (height + depth);
                    point.z = (height + depth);
                }
                vertices.Add(point);

                uvs.Add(new Vector2(_u, _w));
            }
        }
        mesh.SetVertices(vertices);
        mesh.uv = uvs.ToArray();
        triangles = new List<int>();

        for (int u = 0; u < numberOfSegmentsU; u++)
        {
            for (int w = 0; w < numberOfSegmentsW; w++)
            {
                var p0 = u * numberOfPointsW + w;
                var p1 = u * numberOfPointsW + (w + 1);
                var p2 = (u + 1) * numberOfPointsW + w;
                var p3 = (u + 1) * numberOfPointsW + (w + 1);

                triangles.Add(p0);
                triangles.Add(p1);
                triangles.Add(p2);

                mesh.triangles = triangles.ToArray();

                triangles.Add(p2);
                triangles.Add(p1);
                triangles.Add(p3);

                mesh.triangles = triangles.ToArray();
            }
        }
        mesh.RecalculateNormals();
    }

    private void SetCorners()
    {
        GroundP0 = new Vector3(0f, 0f, 0f); //--- Origin
        GroundP1 = new Vector3(0f, 0f, width + 2 * height); //--- Width
        GroundP2 = new Vector3(depth + 2 * height, 0f, 0f); //--- Depth
        GroundP3 = new Vector3(depth + 2 * height, 0f, height + 2 * height); //--- Width and Length
    }

    private void SetDoorway(Vector3 pointOnDoor, Vector3 yetAnotherPointOnDoor)
    {
        var vertecies = mesh.vertices;
        var triagles = mesh.triangles;
        
        vertices = vertices.Where(p => Vector3.Equals(p, pointOnDoor)).ToList();
    }
}