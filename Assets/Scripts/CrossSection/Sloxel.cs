using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sloxel
{
    public Sloxel()
    {
        IntersectedByLines = new List<LineSegment>();
        CrazyBalls = new List<cctPointScript>();
        MaxLineNumber = -1;
        MinLineNumber = 100000000;
        MaximumSeparation = 0;
        MinimumSeparation = 100000000;
        WallThickness = 1000000;
    }
    public float WallThickness { get; set; }
    public int Layer { get; set; }
    public List<LineSegment> IntersectedByLines { get; set; }
    public int Id { get; set; }
    public int MinLineNumber { get; set; }
    public int MaxLineNumber { get; set; }
    public Vector2 GridPosition { get; set; }
    public Vector3 Position { get; set; }
    public VoxelClass Voxel { get; set; }
    public float Dim { get; set; }
    public Vector3 VoxelOrigin { get; set; }
    public float MinimumSeparation { get; set; }
    public float MaximumSeparation { get; set; }
    public List<cctPointScript> CrazyBalls { get; set; }
}
