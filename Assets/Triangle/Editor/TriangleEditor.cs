using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Triangle))]
public class TriangleEditor : Editor
{
    private static bool _drawDefaultInspector = false;
    Triangle triangle;

    void Awake()
    {
        triangle = target as Triangle;
    }


    public override void OnInspectorGUI()
    {
        triangle.P0 = EditorGUILayout.Vector3Field("P0", triangle.P0);
        triangle.P1 = EditorGUILayout.Vector3Field("P1", triangle.P1);
        triangle.P2 = EditorGUILayout.Vector3Field("P2", triangle.P2);

        if (_drawDefaultInspector = EditorGUILayout.Foldout(_drawDefaultInspector, "DrawDefaultInspector"))
            DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        var P0_transformed = triangle.transform.TransformPoint(triangle.P0);
        var P1_transformed = triangle.transform.TransformPoint(triangle.P1);
        var P2_transformed = triangle.transform.TransformPoint(triangle.P2);

        triangle.P0 = Handles.FreeMoveHandle(P0_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        triangle.P0 = triangle.transform.InverseTransformPoint(triangle.P0);
        triangle.P1 = Handles.FreeMoveHandle(P1_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        triangle.P1 = triangle.transform.InverseTransformPoint(triangle.P1);
        triangle.P2 = Handles.FreeMoveHandle(P2_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        triangle.P2 = triangle.transform.InverseTransformPoint(triangle.P2);
    }

    [MenuItem("GameObject/3D Object/Triangle")]
    public static Triangle CreateTriangle()
    {
        var go = new GameObject("Triangle");
        var triangle = go.AddComponent<Triangle>();
        triangle.CreateMesh();

        return triangle;
    }
}
