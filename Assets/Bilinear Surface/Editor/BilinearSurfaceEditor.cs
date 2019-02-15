using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Der Bilinear Surface Editor
/// 
/// Der Editor soll Handles für die 4 Knotrollpunkte des Sufaces bereitstellen,
/// die frei bewegt werden können.
/// 
/// Zusätzlich wollen wir die Anzahl der Segmente in U- und W-Richtung über
/// Slider einstellen können.
/// 
/// Um einen "Demo-Punkt" auf der Fläche darzustellen, wollen wir zwei Slider
/// für die Parameter u und w haben.
/// </summary>
[CustomEditor(typeof(BilinearSurface))]
public class BilinearSurfaceEditor : Editor
{

    // Die BilinearSurface-Komponente des GameObjects
    BilinearSurface triangleCoords;

    // Use this for initialization
    void Awake()
    {
        // Wir holen uns die BilinearSufrace-Komponente
        triangleCoords = target as BilinearSurface;
    }

    // Update is called once per frame
    void OnSceneGUI()
    {
        // Zeichne und behandle ein Handle für den Punkt P00
        bool changed = false;

        Vector3 old = triangleCoords.P00;
        triangleCoords.P00 = Handles.FreeMoveHandle(triangleCoords.P00,Quaternion.identity,0.2f,Vector3.zero, Handles.SphereHandleCap);
        changed = changed || old != triangleCoords.P00;

        // Zeichne und behandle ein Handle für den Punkt P01
        old = triangleCoords.P01;
        triangleCoords.P01 = Handles.FreeMoveHandle(
            triangleCoords.P01,
            Quaternion.identity,
            0.2f,
            Vector3.zero,
            Handles.SphereHandleCap);
        changed = changed || old != triangleCoords.P01;
        // Zeichne und behandle ein Handle für den Punkt P10
        old = triangleCoords.P10;
        triangleCoords.P10 = Handles.FreeMoveHandle(
                triangleCoords.P10,
                Quaternion.identity,
                0.2f,
                Vector3.zero,
                Handles.SphereHandleCap);
        changed = changed || old != triangleCoords.P10;
        // Zeichne und behandle ein Handle für den Punkt P11
        old = triangleCoords.P11;
        triangleCoords.P11 = Handles.FreeMoveHandle(
               triangleCoords.P11,
               Quaternion.identity,
               0.2f,
               Vector3.zero,
               Handles.SphereHandleCap);
        changed = changed || old != triangleCoords.P11;

        if(changed)
            triangleCoords.CreateMesh();
    }

    /// <summary>
    /// Wir überschreiben die Methode OnInspectorGUI um uns einen eigenen
    /// Inspektor zu basteln
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Wir beginnen eine ChangeCheck-Umgebung um auf Änderungen
        // der Werte reagieren zu können.
        EditorGUI.BeginChangeCheck();

        var oldAmp = triangleCoords.amplitude;
        triangleCoords.amplitude = EditorGUILayout.IntSlider("Amplitude", (int) triangleCoords.amplitude, -10, 10);
        triangleCoords.roughidity = EditorGUILayout.IntSlider("Roughidity", (int)triangleCoords.roughidity, -10, 10);
        // Slider für den Parameter u
        triangleCoords.u = EditorGUILayout.Slider("u",
            triangleCoords.u, 0f,
            1f);
        // Slider für den Parameter w
        triangleCoords.w = EditorGUILayout.Slider("w",
                    triangleCoords.w, 0f,
                    1f);

        // Slider für die Anzahl der Segmente in U-Richtung
        triangleCoords.numberOfSegmentsU = EditorGUILayout.IntSlider("Segments U",
                   triangleCoords.numberOfSegmentsU, 1,
                   100);

        // Slider für die Anzahl der Segmente in W-Richtung
        triangleCoords.numberOfSegmentsW = EditorGUILayout.IntSlider("Segments W",
                    triangleCoords.numberOfSegmentsW, 1,
                    100);

        triangleCoords.heightMapTexture = EditorGUILayout.ObjectField("HeightTexture", triangleCoords.heightMapTexture, typeof(Texture2D)) as Texture2D;
        triangleCoords.noiseMapTexture = EditorGUILayout.ObjectField("NoiseTexture", triangleCoords.noiseMapTexture, typeof(Texture2D)) as Texture2D;
        triangleCoords.mainTexture = (Texture2D)EditorGUILayout.ObjectField("MainTexture", triangleCoords.mainTexture, typeof(Texture2D));
        triangleCoords.rockTexture = EditorGUILayout.ObjectField("Rock", triangleCoords.rockTexture, typeof(Texture2D)) as Texture2D;
        triangleCoords.waterTexture = EditorGUILayout.ObjectField("Water", triangleCoords.waterTexture, typeof(Texture2D)) as Texture2D;
        triangleCoords.grassTexture = EditorGUILayout.ObjectField("Grass", triangleCoords.grassTexture, typeof(Texture2D)) as Texture2D;

        // Wir beenden die ChangeCheck-Umgebung und prüfen ob eine Eingabe
        // gemacht wurde.
        if (EditorGUI.EndChangeCheck())
        {
            // Wurde eine Eingabe gemacht, zeichnen wir das Szenenfenster 
            triangleCoords.CreateMesh();
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();
    }

    [MenuItem("GameObject/TriangleCoords")]
    public static BilinearSurface CreateTriangleCoords()
    {
        GameObject go = new GameObject("TriangleCoords");
        BilinearSurface coords =
            go.AddComponent<BilinearSurface>();

        coords.CreateMesh();
        return coords;
    }
}
