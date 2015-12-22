using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csLine
{ 
    public csLine (Vector3 ep0, Vector3 ep1, Vector3 normal)
    {
        Endpoint0 = ep0;
        Endpoint1 = ep1;
        Normal = normal;
        CloseLines = new List<csLine>();
        WallThickness = 100000f;
        PathNumberInLayer = 0;
    }
    public int PathNumberInLayer { get; set; }
    public Vector3 Endpoint0 { get; set; }
    public Vector3 Endpoint1 { get; set; }
    public List<csLine> CloseLines { get; set; }
    public csLine Closest { get; set; }
    public float WallThickness { get; set; }
    public Vector3 Normal { get; set; }
    public Triangle Triangle0 { get; set; }
    public Triangle Triangle1 { get; set; }
}
