using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public Vector3 position;
    public Quaternion rotation;

    public Color color = Color.white;

    public Location()
    {
        this.position = Vector3.zero;
        this.rotation = Quaternion.identity;
    }

    public Location(Location other)
    {
        this.position = other.position;
        this.rotation = other.rotation;
    }

    public void SetLocation(Location other)
    {
        this.position = other.position;
        this.rotation = other.rotation;
    }

    public void TurnLeft(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }

    public void TurnRight(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, angle, 0f));
    }

    public void PitchDown(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(-angle, 0f, 0f));
    }

    public void PitchUp(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(angle, 0f, 0f));
    }

    public void RollLeft(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, 0f, -angle));
    }

    public void RollRight(float angle)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void TurnAround()
    {
        rotation = Quaternion.Inverse(rotation);
    }

    public void Move(float distance)
    {
        var targetForward = (this.rotation * Vector3.forward).normalized;

        this.position += targetForward * distance;
    }

}
