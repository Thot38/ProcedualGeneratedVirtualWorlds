using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    int[,] map;

    public bool useRandomSeed = true;
    public string seed;

    public void GenerateMap()
    {
        Debug.Log("Start");
        map = new int[width, height];

        if (useRandomSeed)
            seed = DateTime.Now.ToString();

        var pseudoRandomGenerator = new System.Random(seed.GetHashCode());

        //--- Pick a start location
        Vector2Int start = Vector2Int.zero;
        Vector2Int end = Vector2Int.zero;

        do
        {

            start.x = UnityEngine.Random.Range(0, width - 1);
            start.y = UnityEngine.Random.Range(0, height - 1);
            //start.y = pseudoRandomGenerator.Next(height);
            //start.y = pseudoRandomGenerator.Next(height); 
        } while (start.x > 0 && start.x < width - 1 && start.y > 0 && start.y < height - 1);
        map[start.x, start.y] = 1;

        //--- Pick an end location
        do
        {
            end.x = UnityEngine.Random.Range(0, width - 1);
            end.y = UnityEngine.Random.Range(0, height - 1);
        } while ((end.x > 0 && end.x < width - 1 && end.y > 0 && end.y < height - 1) || Vector2.Distance(start, end) < 2);
        map[end.x, end.y] = 2;

        var path = PickPath(start, end);

        foreach (var vect in path)
        {
            if(map[vect.x, vect.y] == 0)
                map[vect.x, vect.y] = 3;
        }


        Debug.Log("End");

    }

    public void OnDrawGizmos()
    {
        if (map == null)
            GenerateMap();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.color = PickColor(map[i, j]);
                Gizmos.DrawCube(new Vector3(i, 0, j), new Vector3(1, 1, 1));
            }
        }
    }

    private Color PickColor(int v)
    {
        switch (v)
        {
            case 0: return Color.black;
            case 1: return Color.white;
            case 2: return Color.red;
            case 3: return Color.blue;
            default: return Color.cyan;
        }
    }

    private List<Vector2Int> PickPath(Vector2Int start, Vector2Int end)
    {
        var resultList = new List<Vector2Int>();
        while (!Vector2.Equals(start, end))
        {
            var result = Vector2Int.zero;

            var endIsToTheLeft = start.x < end.x;
            var endIsToTheBottom = start.y > end.y;

            var next = UnityEngine.Random.Range(0, 3);

            switch (next)
            {
                //--- Forward
                case 0:
                    if (endIsToTheLeft)
                        result.x = start.x - 1;
                    else
                        result.x = start.x + 1;
                    break;
                //--- Up
                case 1:
                    if (height > start.x + 1)
                        result.y = start.y + 1;
                    break;
                //--- Down
                case 2:
                    if (0 < start.x - 1)
                        result.y = start.y - 1;
                    break;
                default: break;

            }

            //if (result.x >= 0 && result.x < width && result.y >= 0 && result.y < height)
            //{
                resultList.Add(result);
                start = result;
            //}
        }
        return resultList;
    }
}
