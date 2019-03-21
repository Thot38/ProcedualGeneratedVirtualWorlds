using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    void Fill(ref float[,,] map, int width, int height, int depth);
    void Border(float[,,] map, int width, int height, int depth);
}
