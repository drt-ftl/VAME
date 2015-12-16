using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFitter : MonoBehaviour
{
    GcdInterpreter gcdI;
    public static int maxPathsThrough = 0;
    public static float maxIntersectDistance = 0;

    void Start()
    {
        gcdI = LoadFile.gcdInterpreter;
    }

    public static int round(float value)
    {
        // Add or subtract 0.5 depending on whether the value is positive
        // or negative, and truncate the result.
        return Mathf.RoundToInt(value);
        //return (int)(value + 0.5f * Mathf.Sign(value));
    }

    public static float roundf(float value)
    {
        // Add or subtract 0.5 depending on whether the value is positive
        // or negative, and truncate the result.
        return Mathf.Round(value);
        //return (int)(value + 0.5f * Mathf.Sign(value));
    }

    public void MakeNew(VoxelClass vc, LineSegment line)
    {
        //vc.IntersectedByLines.Add(line);
        //if (vc.IntersectedByLines.Count > maxPathsThrough)
        //    maxPathsThrough = vc.IntersectedByLines.Count;
    }

    public void FitPaths()
    {
        var divisions = (int)cSectionGCD.sloxelResolution.x;
        foreach (var l in LoadFile.gcdLines)
        {
            var p1 = l.p1;
            var p2 = l.p2;
            var pos = Vector3.zero;
            if (p1.x == p2.x && p1.z != p2.z && p1.y == p2.y) // travels along z, stationary in x and y
            {
                ScanVoxels(l, false);
            }
            else if (p1.z == p2.z && p1.x != p2.x && p1.y == p2.y) // travels along x, stationary in z and y
            {
                ScanVoxels(l, true);
            }
        }
        Camera.main.GetComponent<InspectorR>().SetVoxelsFitted();
        foreach (var v in cSectionGCD.voxels)
        {
            //v.Value.SetMaxAndMin();
            if (v.Value.IntersectedByLines.Count > 1 
                && v.Value.MaxDistance > maxIntersectDistance)
                maxIntersectDistance = v.Value.MaxDistance;
        }
    }

    public void ScanVoxels(LineSegment line, bool movesInX)
    {
        var divisions = cSectionGCD.sloxelResolution.x;
        var half = 0.5f / divisions;
        foreach (var v in cSectionGCD.voxels)
        {
            if (movesInX)
            {
                var x = v.Key.x;
                var v2d = new Vector2(v.Key.y, v.Key.z);
                var l2d = new Vector2(line.p1.y, line.p1.z);
                if (Mathf.Abs(v.Key.y - line.p1.y) <= half && Mathf.Abs(v.Key.z - line.p1.z) <= half)
                {
                    if (line.p2.x > line.p1.x && line.p1.x < x + half && line.p2.x > x - half) // p2 greater than p1, p1 < x + half,  p2 > x - half
                        MakeNew(v.Value, line);
                    else if (line.p2.x < line.p1.x && line.p1.x > x - half && line.p2.x < x + half)
                        MakeNew(v.Value, line);
                }
            }
            else // Moves In Z
            {
                var z = v.Key.z;
                var v2 = new Vector2(v.Key.x, v.Key.y);
                var l2 = new Vector2(line.p1.x, line.p1.y);
                if (Mathf.Abs(v.Key.x - line.p1.x) <= half && Mathf.Abs(v.Key.y - line.p1.y) <= half)
                {
                    if (line.p2.z > line.p1.z && line.p1.z < z + half && line.p2.z > z - half) // p2 greater than p1, p1 < x + half,  p2 > x - half
                        MakeNew(v.Value, line);
                    else if (line.p2.z < line.p1.z && line.p1.z > z - half && line.p2.z < z + half)
                        MakeNew(v.Value, line);
                }
            }
        }
    }
}

