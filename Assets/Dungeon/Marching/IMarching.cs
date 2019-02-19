using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMarching
{
    float Surface { get; set; }

    void Generate(float[,,] mapPoints, int width, int height, int depth, IList<Vector3> verticies, IList<int> indices);
}
