using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimplexNoise : INoise
{
    private readonly float transformationFactor; //--- F
    private readonly float retransformationFactor; //--- G
    private readonly float dimensions;

    //--- https://en.wikipedia.org/wiki/Simplex_noise
    public SimplexNoise(int dimensions)
    {
        this.dimensions = dimensions;
        transformationFactor = (Mathf.Sqrt(dimensions + 1) - 1) / dimensions;
        transformationFactor = (1 - (1 / Mathf.Sqrt(dimensions + 1))) / dimensions;
    }

    public float Noise(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public float Noise(int[] coordinates)
    {
        if (coordinates.Length != dimensions)
            throw new System.ArgumentException($"Expected {dimensions} of Coordinates, but recieved {coordinates.Length}.");

        //--- Coordinate Skewing -> Positioning the point inside a hypercubic honeycomb with unit distant points
        var skewdPoints = new List<float>();
        var pointsOnHypercubeCell = new List<float>();
        var pointSum = coordinates.Sum();
        foreach (var i in coordinates)
        {
            skewdPoints.Add(i + (pointSum) * transformationFactor);
        }

        foreach(var point in skewdPoints)
        {
            pointsOnHypercubeCell.Add(point - Mathf.Floor(point));
        }

        //--- Simplicial subdivision



        return 0;
    }
}
