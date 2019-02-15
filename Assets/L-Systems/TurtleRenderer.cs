using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleRenderer
{
    public float angle = 22.5f;
    public float distance = 1f;

    private Stack<Location> stack = new Stack<Location>();

    public TurtleRenderer()
    {

    }

    private Color GetColor(int colorNumber)
    {
        switch (colorNumber) {
            case 0: return Color.red;
            case 1: return Color.black;
            case 2: return Color.yellow;
            case 3: return Color.cyan;
            case 4: return Color.green;
            case 5: return Color.blue;
            default: return Color.magenta;
        }
    }

    public void Process(string operations)
    {
        stack = new Stack<Location>();

        var lastLocation = new Location();
        var currentLocation = new Location();

        var i = 0;
        foreach (var c in operations)
        {
            switch (c)
            {
                case LSystem.CLOCKWISE:
                    currentLocation.TurnRight(angle);
                    break;
                case LSystem.COUNTERCLOCKWISE:
                    currentLocation.TurnLeft(angle);
                    break;
                case LSystem.BITCHDOWN:
                    currentLocation.PitchDown(angle);
                    break;
                case LSystem.BITCHUP:
                    currentLocation.PitchUp(angle);
                    break;
                case LSystem.POPDATBITCH:
                    currentLocation = stack.Pop();
                    break;
                case LSystem.PUSH:
                    stack.Push(new Location(currentLocation));
                    break;
                case LSystem.THEYHATINRIGHT:
                    currentLocation.RollRight(angle);
                    break;
                case LSystem.THEYSEEMEROLLINGLEFT:
                    currentLocation.RollLeft(angle);
                    break;
                case LSystem.TURNAROUNDANDHANDSUP:
                    currentLocation.TurnAround();
                    break;
                case LSystem.COLOR:
                    if (i + 1 < operations.Length)
                    {
                        currentLocation.color = GetColor(int.Parse(operations.Substring(i + 1, 1)));
                    }
                    break;
                default:
                    lastLocation.SetLocation(currentLocation);
                    currentLocation.Move(distance);

                    Gizmos.color = currentLocation.color;
                    Gizmos.DrawLine(lastLocation.position, currentLocation.position);
                    break;
            }
            i++;
        }
    }
}
