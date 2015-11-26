using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckWalls
{
    Dictionary<float, CrossSectionLayer> CSs = new Dictionary<float, CrossSectionLayer>();
    public CheckWalls()
    {
        CSs = cSectionGCD.layers;
        foreach (var cs in CSs) // Each Slice
        {
            foreach (var line in cs.Value.border) // Each Line In Slice
            {
                foreach (var other in cs.Value.border) // Check Against All On Slice
                {
                    if (other == line) continue; // But Skip This One
                    CheckForSkewLines(line, other);
                }
            }
        }
    }
    void CheckForSkewLines(csLine line, csLine other)
    {
        var dot = Vector3.Dot(line.Normal, other.Normal);
        if (dot >= 0) return; // If normals are at less than 90 degrees to each other, they are not facing each other
        var lSl = line.Endpoint1 - line.Endpoint0;
        var oSl = other.Endpoint1 - other.Endpoint0;
        line.CloseLines.Add(other);
        other.CloseLines.Add(line);
        var d = 100000f;
        #region parallel
        //var lMag = Vector3.Magnitude(lSl);
        //var oMag = Vector3.Magnitude(oSl);
        //var lmagOmag = lMag * oMag;
        //if (Mathf.Abs(Vector3.Dot(lSl, oSl) - lmagOmag) < 0.001f) //parallel
        {
            if (Mathf.Abs(lSl.x) <= 0.001f && Mathf.Abs(oSl.x) <= 0.001f) // parallel to x
            {
                d = Mathf.Abs(line.Endpoint0.x - other.Endpoint0.x);                
            }
            else if (Mathf.Abs(lSl.z) <= 0.001f && Mathf.Abs(oSl.x) <= 0.001f) // parallel to z
            {
                d = Mathf.Abs(line.Endpoint0.z - other.Endpoint0.z);
            }
            else
            {
                d = 0f;
                var x = lSl.x;
                var z = lSl.z;
            }

            if (d > 0.001f && d < line.WallThickness)
            {
                line.WallThickness = d;
                line.Closest = other;
            }
            if (d > 0.0001f && d < other.WallThickness)
            {
                other.WallThickness = d;
                other.Closest = other;
            }
        }
        #endregion
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
