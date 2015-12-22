using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckWalls
{
    public static bool WallsChecked = false;
    private Dictionary<float, CrossSectionLayer> CSs;
    public CheckWalls(Dictionary<float, CrossSectionLayer> css)
    {
        CSs = css;
        WallsChecked = true;
        foreach (var cs in CSs) // Each Slice
        {
            var pathNumber = 0;
            var doneList = new List<csLine>();
            foreach (var line in cs.Value.border) // Each Line In Slice
            {
                if (cs.Value.BorderPaths.Count == 0)
                {
                    cs.Value.BorderPaths.Add(new List<int>());
                }
                cs.Value.BorderPaths[pathNumber].Add(cs.Value.border.IndexOf(line));
                line.PathNumberInLayer = pathNumber;
                if (line.Endpoint1 == cs.Value.border[cs.Value.BorderPaths[pathNumber][0]].Endpoint0)
                {
                    cs.Value.BorderPaths.Add(new List<int>());
                    pathNumber++;
                }

                if (doneList.Contains(line)) continue;
                doneList.Add(line);
                foreach (var other in cs.Value.border) // Check Against All On Slice
                {
                    CheckForSkewLines(line, other);
                }
            }
            //foreach (var sloxel in cs.Value.Sloxels)
            //{
            //    var sPos = new Vector2(sloxel.Position.x, sloxel.Position.z);
            //    float shortestDistance = 10000000;
            //    foreach (var bLine in cs.Value.border)
            //    {
            //        var p1 = new Vector2(bLine.Endpoint0.x, bLine.Endpoint0.z);
            //        var p2 = new Vector2(bLine.Endpoint1.x, bLine.Endpoint1.z);
            //        var n = new Vector2(bLine.Normal.x, bLine.Normal.z);
            //        var closestPoint = GetClosestPointOnLineSegment(p1, p2, sPos);
            //        var d = (sPos - closestPoint).magnitude;
            //        if (d < shortestDistance)
            //        {
            //            shortestDistance = d;
            //            sloxel.WallThickness = bLine.WallThickness;
            //            if (sloxel.WallThickness < sloxel.Voxel.WallThickness)
            //                sloxel.Voxel.WallThickness = sloxel.WallThickness;
            //        }
            //    }
            //}
        }
    }
    public void SetSloxelWT()
    {
        foreach (var cs in CSs) // Each Slice
        {
            foreach (var sloxel in cs.Value.Sloxels)
            {
                var sPos = new Vector2(sloxel.Position.x, sloxel.Position.z);
                float shortestDistance = 10000000;
                foreach (var bLine in cs.Value.border)
                {
                    var p1 = new Vector2(bLine.Endpoint0.x, bLine.Endpoint0.z);
                    var p2 = new Vector2(bLine.Endpoint1.x, bLine.Endpoint1.z);
                    var closestPoint = GetClosestPointOnLineSegment(p1, p2, sPos);
                    var d = (sPos - closestPoint).magnitude;
                    if (d < shortestDistance)
                    {
                        shortestDistance = d;
                        sloxel.WallThickness = bLine.WallThickness;
                        //if (sloxel.WallThickness < sloxel.Voxel.WallThickness)
                        //    sloxel.Voxel.WallThickness = sloxel.WallThickness;
                    }
                }
            }
        }
    }

    public static Vector2 GetClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AP = P - A;       //Vector from A to P   
        Vector2 AB = B - A;       //Vector from A to B  

        float magnitudeAB = AB.magnitude;     //Magnitude of AB vector (it's length squared)     
        float ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
        float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

        if (distance < 0)     //Check if P projection is over vectorAB     
        {
            return A;

        }
        else if (distance > 1)
        {
            return B;
        }
        else
        {
            return A + AB * distance;
        }
    }

    void CheckForSkewLines(csLine line, csLine other)
    {
        if (line.Endpoint0 == other.Endpoint0
            || line.Endpoint0 == other.Endpoint1
            || line.Endpoint1 == other.Endpoint0
            || line.Endpoint1 == other.Endpoint1)
            return;
        var nLine = line.Normal;
        nLine = Vector3.Normalize(nLine);
        var nOther = line.Normal;
        nOther = Vector3.Normalize(nOther);
        var dot = Vector3.Dot(nLine, nOther);
        //if (dot >= 0) return; // If normals are at less than 90 degrees to each other, they are not facing each other
        var lSl = line.Endpoint1 - line.Endpoint0;
        var oSl = other.Endpoint1 - other.Endpoint0;
        line.CloseLines.Add(other);
        other.CloseLines.Add(line);
        var d = 100000f;
        #region parallel
        if (Mathf.Abs(lSl.x) == 0 && Mathf.Abs(oSl.x) == 0) // parallel to x
        {
                d = Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x);                
        }
        else if (Mathf.Abs(lSl.z) == 0 && Mathf.Abs(oSl.z) == 0) // parallel to z
        {
                d = Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z);
        }
        #endregion
        else
        {
            var lNorm = line.Normal;
            var oNorm = other.Normal;
            var lMag = Vector3.SqrMagnitude(lNorm);
            var oMag = Vector3.SqrMagnitude(oNorm);
            var denom = lMag * oMag;
            var numer = Vector3.Dot(lNorm, oNorm);
            var theta = Mathf.Acos(numer / denom);
            theta = (theta / (2 * Mathf.PI)) * 360f;
            if (theta < 90 || theta > 270) return;
            var diff = line.Endpoint0 - other.Endpoint0;
            var sCheck1Constant = diff.x / oSl.x;
            var sCheck2Constant = diff.y / oSl.y;
            var sCheck1Slope = lSl.x / oSl.x;
            var sCheck2Slope = lSl.y / oSl.y;
            var constant = sCheck1Constant - sCheck2Constant;
            var slope = sCheck1Slope - sCheck2Slope;
            var t = (-constant) / slope;
            var s = diff.x + t * oSl.z;
            if (t < 0) t = 0;
            if (t > 1) t = 1;
            if (s < 0) s = 0;
            if (s > 1) s = 1;
            d = Vector3.Distance(line.Endpoint0 + t * lSl, other.Endpoint0 + s * oSl);
        }

        if (d > 0.0001f && d< line.WallThickness)
        {
            line.WallThickness = d;
            line.Closest = other;
        }
        if (d > 0.0001f && d < other.WallThickness)
        {
            other.WallThickness = d;
            other.Closest = line;
        }
    }


   float CheckForParallelLines(csLine line, csLine other, float min)
    {
        var lineLength = Vector3.Distance(line.Endpoint0, line.Endpoint1);
        var otherLength = Vector3.Distance(other.Endpoint0, other.Endpoint1);
        if (line.Endpoint0.z == line.Endpoint1.z) // Moves In X
        {
            if (other.Endpoint0.z == other.Endpoint1.z) // Both Move In X
            {
                if (lineLength >= otherLength)
                {
                    if (other.Endpoint0.x <= line.Endpoint0.x && other.Endpoint0.x >= line.Endpoint1.x
                        || other.Endpoint0.x >= line.Endpoint0.x && other.Endpoint0.x <= line.Endpoint1.x
                        || other.Endpoint1.x <= line.Endpoint0.x && other.Endpoint1.x >= line.Endpoint1.x
                        || other.Endpoint1.x >= line.Endpoint0.x && other.Endpoint1.x <= line.Endpoint1.x) // Check that endpoints are within  x range
                    {
                        if (Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z) < min)
                        {
                            min = Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z);
                            line.CloseLines.Add(other);
                            line.Closest = other;
                            line.WallThickness = min;
                            other.CloseLines.Add(line);
                            other.Closest = other;
                            other.WallThickness = min;
                        }
                    }
                }
                else
                if (line.Endpoint0.x <= other.Endpoint0.x && line.Endpoint0.x >= other.Endpoint1.x
                        || line.Endpoint0.x >= other.Endpoint0.x && line.Endpoint0.x <= other.Endpoint1.x
                        || line.Endpoint1.x <= other.Endpoint0.x && line.Endpoint1.x >= other.Endpoint1.x
                        || line.Endpoint1.x >= other.Endpoint0.x && line.Endpoint1.x <= other.Endpoint1.x) // Check that endpoints are within  x range
                {
                    if (Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z) < min)
                    {
                        min = Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z);
                        line.CloseLines.Add(other);
                        line.Closest = other;
                        line.WallThickness = min;
                        other.CloseLines.Add(line);
                        other.Closest = other;
                        other.WallThickness = min;
                    }
                }
            }
        }

        if (line.Endpoint0.x == line.Endpoint1.x) // Moves In Z
        {
            if (other.Endpoint0.x == other.Endpoint1.x) // Both Move In Z
            {
                if (lineLength >= otherLength)
                {
                    if (other.Endpoint0.z <= line.Endpoint0.z && other.Endpoint0.z >= line.Endpoint1.z
                        || other.Endpoint0.z >= line.Endpoint0.z && other.Endpoint0.z <= line.Endpoint1.z
                        || other.Endpoint1.z <= line.Endpoint0.z && other.Endpoint1.z >= line.Endpoint1.z
                        || other.Endpoint1.z >= line.Endpoint0.z && other.Endpoint1.z <= line.Endpoint1.z) // Check that endpoints are within  Z range
                    {
                        if (Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x) < min)
                        {
                            min = Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x);
                            line.CloseLines.Add(other);
                            line.Closest = other;
                            line.WallThickness = min;
                            other.CloseLines.Add(line);
                            other.Closest = other;
                            other.WallThickness = min;
                        }
                    }
                }
                else
                if (line.Endpoint0.z <= other.Endpoint0.z && line.Endpoint0.z >= other.Endpoint1.z
                        || line.Endpoint0.z >= other.Endpoint0.z && line.Endpoint0.z <= other.Endpoint1.z
                        || line.Endpoint1.z <= other.Endpoint0.z && line.Endpoint1.z >= other.Endpoint1.z
                        || line.Endpoint1.z >= other.Endpoint0.z && line.Endpoint1.z <= other.Endpoint1.z) // Check that endpoints are within  Z range
                {
                    if (Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x) < min)
                    {
                        min = Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x);
                        line.CloseLines.Add(other);
                        line.Closest = other;
                        line.WallThickness = min;
                        other.CloseLines.Add(line);
                        other.Closest = other;
                        other.WallThickness = min;
                    }
                }
            }
        }
        return min;
    }
}
