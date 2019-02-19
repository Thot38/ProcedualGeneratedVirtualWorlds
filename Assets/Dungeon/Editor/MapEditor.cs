using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGeneratorV2))]
public class RoomEditor : Editor
{
    MapGeneratorV2 mapGenerator;

    private void Awake()
    {
        mapGenerator = target as MapGeneratorV2;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("ReGenerate"))
        {
            mapGenerator.GenerateMap();
        }

        if (EditorGUI.EndChangeCheck())
        { 
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();
    }

    [MenuItem("GameObject/Cavern")]
    public static MapGeneratorV2 CreateTriangleCoords()
    {
        var go = new GameObject("Cave");
        var coords = go.AddComponent<MapGeneratorV2>();

        coords.GenerateMap();
        return coords;
    }
}
