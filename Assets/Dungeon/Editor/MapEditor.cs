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
        if (GUILayout.Button("Generate Quazy Shit"))
        {
            mapGenerator.GenerateCrazyShit(1);
        }
        if (GUILayout.Button("Generate 2Disch Map"))
        {
            mapGenerator.GenerateMapV2();
        }
        if (GUILayout.Button("Smooth"))
        {
            mapGenerator.SmoothMap(new System.Random());
        }
        if (GUILayout.Button("Fill"))
        {
            mapGenerator.FillMap();
        }
        if (GUILayout.Button("Mesh"))
        {
            mapGenerator.CreateMesh();
        }

        if (EditorGUI.EndChangeCheck())
        { 
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();
    }

    public static MapGeneratorV2 CreateTriangleCoords()
    {
        var go = new GameObject("Cave");
        var coords = go.AddComponent<MapGeneratorV2>();

        coords.GenerateMap();
        return coords;
    }
}
