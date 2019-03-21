using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MapGeneratorV3))]
public class MapEditorV2 : Editor
{
    MapGeneratorV3 mapGenerator;
    GeneratorModel model = new GeneratorModel
    {
        UseRandomSeed = true,
        Increment = 0.07f,
        NumberOfAdjacents = 13,
        LandMasses = 0.45f,
        NumberOfSmoothings = 1,
        Variance = 1,
        RandomAmplitude = 2f,
    };
    MapGeneratorV3.MapGenerators selectedMapGenerator;

    private void Awake()
    {
        mapGenerator = target as MapGeneratorV3;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        selectedMapGenerator = (MapGeneratorV3.MapGenerators)EditorGUILayout.EnumPopup("Choose your Map Generator: ", selectedMapGenerator);

        if (GUILayout.Button("Generate"))
        {
            IGenerator gen = null;
            if (model.UseRandomSeed)
                model.Seed = System.DateTime.Now.ToString();
            switch (selectedMapGenerator)
            {
                case MapGeneratorV3.MapGenerators.NoiseGenerator:
                    gen = new NoiseGenerator(model.Increment, model.Seed);
                    break;
                case MapGeneratorV3.MapGenerators.RandomGenerator:
                    gen = new RandomGenerator(model.LandMasses, model.NumberOfAdjacents, model.Variance, model.NumberOfSmoothings, model.Seed);
                    break;
                case MapGeneratorV3.MapGenerators.Random2DGenerator:
                    gen = new Random2DGenerator(model.Increment, model.NumberOfAdjacents, model.NumberOfSmoothings, model.LandMasses, model.Seed);
                    break;
                case MapGeneratorV3.MapGenerators.RandomOutOfPlaceGenerator:
                    gen = new RandomOutOfPlaceGenerator(model.LandMasses, model.NumberOfAdjacents, model.Variance, model.NumberOfSmoothings, model.Seed);
                    break;
                case MapGeneratorV3.MapGenerators.Random2DOutOfPlaceGenerator:
                    gen = new Random2DOutOfPlaceGenerator(model.LandMasses, model.NumberOfAdjacents, model.Variance, model.NumberOfSmoothings, model.Seed);
                    break;
                case MapGeneratorV3.MapGenerators.NoiseRandomGenerator:
                    gen = new NoiseRandomGenerator(model.FloorIncrement, model.RandomAmplitude, model.Increment, model.Seed);
                    break;
            }
            mapGenerator.GenerateMap(gen);
        }
        switch (selectedMapGenerator)
        {
            case MapGeneratorV3.MapGenerators.NoiseGenerator:
                EditorGUILayout.LabelField("Noise Generator");
                model.Increment = EditorGUILayout.Slider("Increment", model.Increment, 0f, 0.1f);
                break;
            case MapGeneratorV3.MapGenerators.RandomGenerator:
            case MapGeneratorV3.MapGenerators.RandomOutOfPlaceGenerator:
                EditorGUILayout.LabelField("Random Generator");
                model.LandMasses = EditorGUILayout.Slider("Land Masses", model.LandMasses, 0f, 1f);
                model.NumberOfAdjacents = EditorGUILayout.IntSlider("NumberOfAdjacents", model.NumberOfAdjacents, 10, 20);
                model.Variance = EditorGUILayout.IntSlider("Variance", model.Variance, 0, 5);
                model.NumberOfSmoothings = EditorGUILayout.IntField("Number of Smoothings", model.NumberOfSmoothings);
                break;
            case MapGeneratorV3.MapGenerators.Random2DGenerator:
            case MapGeneratorV3.MapGenerators.Random2DOutOfPlaceGenerator:
                EditorGUILayout.LabelField("Random2D Generator");
                model.LandMasses = EditorGUILayout.Slider("Land Masses", model.LandMasses, 0f, 1f);
                model.NumberOfAdjacents = EditorGUILayout.IntSlider("NumberOfAdjacents", model.NumberOfAdjacents, 0, 9);
                model.NumberOfSmoothings = EditorGUILayout.IntField("Number of Smoothings", model.NumberOfSmoothings);
                model.Increment = EditorGUILayout.Slider("Increment", model.Increment, 0f, 0.1f);
                break;
            case MapGeneratorV3.MapGenerators.NoiseRandomGenerator:
                EditorGUILayout.LabelField("Noise Generator");
                model.Increment = EditorGUILayout.Slider("Increment", model.Increment, 0f, 0.5f);
                model.FloorIncrement = EditorGUILayout.Slider("Floor Increment", model.FloorIncrement, 0f, 0.5f);
                model.RandomAmplitude = EditorGUILayout.Slider("Random Amplitude", model.RandomAmplitude, -2f, 2f);
                break;
        }
        model.UseRandomSeed = EditorGUILayout.Toggle("Use random Seed", model.UseRandomSeed);
        model.Seed = EditorGUILayout.TextField("Seed", model.Seed);


        if (GUILayout.Button("Mesh"))
        {
            mapGenerator.CreateMesh();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    [MenuItem("GameObject/Caves/Cave")]
    public static MapGeneratorV3 CreateTriangleCoords()
    {
        var go = new GameObject("Cave");
        var coords = go.AddComponent<MapGeneratorV3>();

        //coords.GenerateMap();
        return coords;
    }
}
