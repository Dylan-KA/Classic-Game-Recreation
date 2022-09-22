using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    public Transform Target
    { get; protected set; }

    public Vector3 StartPos
    { get; protected set; }

    public Vector3 EndPos
    { get; protected set; }

    public float StartTime
    { get; protected set; }

    public float Duration
    { get; protected set; }

    public Tween(Transform Target, Vector3 StartPos, Vector3 EndPos, float StartTime, float Duration)
    {
        this.Target = Target;
        this.StartPos = StartPos;
        this.EndPos = EndPos;
        this.StartTime = StartTime;
        this.Duration = Duration;
    }
}

