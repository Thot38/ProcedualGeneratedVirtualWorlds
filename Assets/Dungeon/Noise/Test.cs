using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<Texture2D> texture = new List<Texture2D>();
    public PerlinNoiseWrapper perlinNoiseWrapper = new PerlinNoiseWrapper();

    public void Reset()
    {
        if (texture.Count > 5)
            return;


        for (int k = 0; k < 5; k++)
        {
            var x = new Texture2D(64, 64)
            {
                name = "x"
            };
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    float value = perlinNoiseWrapper.Noise(i, j);
                    x.SetPixel(i, j, new Color(value, value, value));

                }

            }
            x.Apply();
            texture.Add(x);
        }
    }
}
