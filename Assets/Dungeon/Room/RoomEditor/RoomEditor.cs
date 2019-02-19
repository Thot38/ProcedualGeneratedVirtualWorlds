using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    Room room;

    private void Awake()
    {
        room = target as Room;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        
        if (EditorGUI.EndChangeCheck())
        {
            room.CreateMesh();
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();
    }

    [MenuItem("GameObject/Room")]
    public static Room CreateTriangleCoords()
    {
        var go = new GameObject("Room");
        var coords = go.AddComponent<Room>();

        coords.CreateMesh();
        return coords;
    }
}
