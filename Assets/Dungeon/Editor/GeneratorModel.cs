using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorModel
{
    public float Increment { get; set; }
    public float FloorIncrement { get; set; }
    public string Seed { get; set; }
    public float LandMasses { get; set; }
    public float RandomAmplitude { get; set; }
    public int NumberOfAdjacents { get; set; }
    public int Variance { get; set; }
    public bool UseRandomSeed { get; set; }
    public int NumberOfSmoothings { get; set; }
}
