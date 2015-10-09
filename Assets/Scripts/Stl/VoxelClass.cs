using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelClass
{
    public VoxelClass()
    {
        IntersectedByLines = new List<LineSegment>();
    }
    public GameObject Voxel { get; set; }
    public Vector3 ScanDirection { get; set; }
    public List<LineSegment> IntersectedByLines { get; set; }
}
