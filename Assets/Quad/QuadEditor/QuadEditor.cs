using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quad))]
public class QuadEditor : Editor
{
    private static bool _drawDefaultInspector = false;
    Quad quad;

    void Awake()
    {
        quad = target as Quad;
    }


    public override void OnInspectorGUI()
    {
        quad.P0 = EditorGUILayout.Vector3Field("P0", quad.P0);
        quad.P1 = EditorGUILayout.Vector3Field("P1", quad.P1);
        quad.P2 = EditorGUILayout.Vector3Field("P2", quad.P2);
        quad.P3 = EditorGUILayout.Vector3Field("P3", quad.P3);

        if (_drawDefaultInspector = EditorGUILayout.Foldout(_drawDefaultInspector, "DrawDefaultInspector"))
            DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        var P0_transformed = quad.transform.TransformPoint(quad.P0);
        var P1_transformed = quad.transform.TransformPoint(quad.P1);
        var P2_transformed = quad.transform.TransformPoint(quad.P2);
        var P3_transformed = quad.transform.TransformPoint(quad.P3);

        quad.P0 = Handles.FreeMoveHandle(P0_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        quad.P0 = quad.transform.InverseTransformPoint(quad.P0);
        quad.P1 = Handles.FreeMoveHandle(P1_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        quad.P1 = quad.transform.InverseTransformPoint(quad.P1);
        quad.P2 = Handles.FreeMoveHandle(P2_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        quad.P2 = quad.transform.InverseTransformPoint(quad.P2);
        quad.P3 = Handles.FreeMoveHandle(P3_transformed, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        quad.P3 = quad.transform.InverseTransformPoint(quad.P3);
    }

    [MenuItem("GameObject/3D Object/MyQuad")]
    public static Quad CreateQuad()
    {
        var go = new GameObject("Quad");
        var quad = go.AddComponent<Quad>();
        quad.CreateMesh();

        return quad;
    }
}
