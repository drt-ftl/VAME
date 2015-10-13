using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelClass
{
    public VoxelClass()
    {
        IntersectedByLines = new List<LineSegment>();
    }
    public void SetMaxAndMin()
    {
        MaxLine = -1;
        MinLine = 1000000000;
        foreach (var inter in IntersectedByLines)
        {
            var index = LoadFile.gcdLines.IndexOf(inter);
            if (index > MaxLine)
                MaxLine = index;
            if (index < MinLine)
                MinLine = index;
        }
    }
    public GameObject Voxel { get; set; }
    public Vector3 ScanDirection { get; set; }
    public List<LineSegment> IntersectedByLines { get; set; }
    public int MaxLine { get; internal set; }
    public int MinLine { get; internal set; }
}
