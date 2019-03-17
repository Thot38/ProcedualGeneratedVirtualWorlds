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
        var skewdPoints = new List<float>(); //--- x'
        var pointsOnHypercubeCell = new List<float>(); //--- xi
        var scewFactor = coordinates.Sum() * transformationFactor;

        var unscewFactor = coordinates.Sum() * retransformationFactor;

        foreach (var i in coordinates)
        {
            skewdPoints.Add(i + scewFactor);
        }

        foreach(var point in skewdPoints)
        {
            pointsOnHypercubeCell.Add(point - FastFloor(point));
        }

        //--- Simplicial subdivision
        //--- Find the corresponding Schäfli orthoscheme (Triangular dissection of a (Hyper-)Cube)
        //--- Where Two Faces are right Triangles
        //--- Every Hypercube in d-dimensional Space can be dissected into d! congruent orthoschemes
        


        return 0;
    }

    //public float Noise3D(float x, float y, float z)
    //{
    //    //--- Coordinate Skewing -> Positioning the point inside a hypercubic honeycomb with unit distant points
    //    var scewFactor = (x + y + z) * transformationFactor;
    //    var scewdPoints = new List<float> { x + scewFactor, y + scewFactor, z + scewFactor };

    //    var pointsOnHypercubeCell = new List<float>;
 
    //    var unscewFactor = (x + y + z) * retransformationFactor;
    //    var unscewdPoints = new List<float>();
    //    foreach (var point in scewdPoints)
    //    {
    //        pointsOnHypercubeCell.Add(FastFloor(point + scewFactor));
    //    }

    //    foreach(var i in scewdPoints)
    //    {
    //        unscewdPoints.Add(i - unscewFactor);
    //    }

    //    var distanceFormOrigin = new List<float> { x - unscewdPoints[0], y - unscewdPoints[1], z - unscewdPoints[2] };
    //    //--- Detemine which simplex we are in

    //}

    private int FastFloor(float x)
    {
        return x > 0 ? (int)x : (int)x - 1;
    }
}
