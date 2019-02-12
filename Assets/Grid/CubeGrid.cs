using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CubeGrid : MonoBehaviour
{
    int width = 10;
    int height = 10;

    // Start is called before the first frame update
    void Reset()
    { 
        this.gameObject.transform.DetachChildren();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //--- GameObject
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

                //--- Positioning
                var y = Mathf.PerlinNoise(i / width, j / height);
                Debug.Log(string.Format("{0}, {1}, {2}", i, y, j));
                go.transform.position = new Vector3(i, y, j);

                //--- As Child of Grid GameObject
                go.transform.parent = this.gameObject.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}