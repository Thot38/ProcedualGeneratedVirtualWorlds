using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoise
{
    float Noise(int x, int y);
    float Noise(int[] coordinates);


}
