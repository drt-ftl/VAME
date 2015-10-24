using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle
{
    public Triangle ()
    {
        Points = new Vector3[3];
        LineSlopes = new List<Vector3>();
        LineInts = new List<Vector3>();
        Min = 1000000;
        Max = -1000000;
    }
    public float Min { get; set; }
    public float Max { get; set; }
    public Vector3[] Points { get; set; }
    public Vector4 PlaneEquation { get; set; }
    public List<Vector3> LineSlopes { get; set; }
    public List<Vector3> LineInts { get; set; }
    public List<Vector3> Endpoint1 { get; set; }
    public List<Vector3> Endpoint2 { get; set; }
}
