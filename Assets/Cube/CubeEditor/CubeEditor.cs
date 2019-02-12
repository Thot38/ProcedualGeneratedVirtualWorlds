using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor
{
    private static bool _drawDefaultInspector = false;
    Cube cube;

    void Awake()
    {
        cube = target as Cube;
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < 8; i++)
        {
            cube.corners[i] = EditorGUILayout.Vector3Field("P" + i, cube.corners[i]);
        }

        if (_drawDefaultInspector = EditorGUILayout.Foldout(_drawDefaultInspector, "DrawDefaultInspector"))
            DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        var cornersTransformed = cube.TransformPosition(cube.corners);

        for (int i = 0; i < 8; i++)
        {
            cube.corners[i] = Handles.FreeMoveHandle(cornersTransformed[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
            cube.corners[i] = cube.transform.InverseTransformPoint(cube.corners[i]);
        }

    }

    [MenuItem("GameObject/3D Object/MyCube")]
    public static Cube CreateCube()
    {
        var go = new GameObject("Cube");
        var cube = go.AddComponent<Cube>();
        cube.CreateMesh();

        return cube;
    }

}
