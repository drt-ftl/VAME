using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CCATpoint
{
    public Vector3 Position { get; set; }
    public float Temp { get; set; }
    public int Id { get; set; }
    public LineSegment Line { get; set; }
    public float t { get; set; }
}
