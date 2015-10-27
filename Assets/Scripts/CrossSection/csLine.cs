﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class csLine
{ 
    public csLine (Vector3 ep0, Vector3 ep1)
    {
        Endpoint0 = ep0;
        Endpoint1 = ep1;
        CloseLines = new List<csLine>();
        WallThickness = 100000f;
    }
    public Vector3 Endpoint0 { get; set; }
    public Vector3 Endpoint1 { get; set; }
    public List<csLine> CloseLines { get; set; }
    public csLine Closest { get; set; }
    public float WallThickness { get; set; }
}