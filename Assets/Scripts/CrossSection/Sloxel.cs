using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sloxel
{
    public Sloxel()
    {
        IntersectedByLines = new List<LineSegment>();
    }

    public List<LineSegment> IntersectedByLines { get; set; }
    public int Id { get; set; }
    public Vector2 GridPosition {get; set;}
    public Vector3 Position { get; set; }
    public VoxelClass Voxel { get; set; } 
    public float Dim { get; set; }  
    public Vector3 VoxelOrigin { get; set; } 
}
