using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(LSystemBehaviour))]
public class LSystemEditor : Editor
{
    LSystemBehaviour lSystemBehaviour;
    LSystem lSystem;
    TurtleRenderer turtle;
    string newRule = "Edit new rule here...";

    private void Awake()
    {
        lSystemBehaviour = target as LSystemBehaviour;
        lSystem = lSystemBehaviour.lSystem;

        turtle = lSystemBehaviour.turtleRenderer;
    }

    private void OnSceneGUI()
    {
        
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        lSystem.axiom = EditorGUILayout.TextField("Axiom", lSystem.axiom);

        lSystem.numberOfIterations = EditorGUILayout.IntSlider("Iterations", lSystem.numberOfIterations, 0, 10);

        List<char> keyz = lSystem.demRulz.Keys.ToList();
        foreach (var key in keyz)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key + " = ");
            var value = EditorGUILayout.TextField(lSystem.demRulz[key]);
            lSystem.EditRule(key, value);
            if (GUILayout.Button("X"))
            {
                lSystem.DeleteRule(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        newRule = EditorGUILayout.TextField(newRule);
        if (GUILayout.Button("AddRule"))
        {
            lSystem.AddRule(newRule);
            newRule = "Yo bling rule here, nigga!";
        }

        EditorGUILayout.Separator();
        turtle.angle = EditorGUILayout.Slider("Angle", turtle.angle, 0f, 360f);
        turtle.distance = EditorGUILayout.Slider("Distance", turtle.distance, -10f, float.MaxValue);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Benjamin"))
        {
            lSystemBehaviour.generatedString = lSystem.Generate();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    [MenuItem("GameObject/L-System")]
    public static LSystemBehaviour CreateLSystemBehaviour()
    {
        var go = new GameObject("LSystem");
        var lSysBehav = go.AddComponent<LSystemBehaviour>();
        return lSysBehav;
    }
}
