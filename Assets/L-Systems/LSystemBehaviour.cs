using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemBehaviour : MonoBehaviour
{
    public LSystem lSystem = new LSystem();
    public TurtleRenderer turtleRenderer = new TurtleRenderer();

    public string generatedString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDrawGizmos()
    {
        //    var generatedString = lSystem.Generate();
        //    Debug.Log("Generated String: " + generatedString);
        turtleRenderer.Process(generatedString);
    }

    private void Reset()
    {
        lSystem = new LSystem();
        generatedString = string.Empty;
        turtleRenderer = new TurtleRenderer();
    }
}
