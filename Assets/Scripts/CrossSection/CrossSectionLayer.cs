using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CrossSectionLayer
{
    public CrossSectionLayer(float _height)
    {
        Height = _height;
        border = new List<csLine>();
        xLines = new List<csLine>();
        zLines = new List<csLine>();
        gcdLines = new List<LineSegment>();
        Sloxels = new List<Sloxel>();
        Voxels = new List<VoxelClass>();
        Min = Vector3.one * 1000000;
        Max = Vector3.one * -1000000;
    }
    public List<csLine> border { get; set; }
    public List<csLine> xLines { get; set; }
    public List<csLine> zLines { get; set; }
    public List<LineSegment> gcdLines { get; set; }
    public Vector3 Max { get; set; }
    public Vector3 Min { get; set; }
    public float Height { get; set; }
    public List<Sloxel> Sloxels{ get; set; }
    public List<VoxelClass> Voxels { get; set; }
}
