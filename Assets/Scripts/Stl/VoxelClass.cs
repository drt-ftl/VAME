using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelClass
{
    public VoxelClass()
    {
        IntersectedByLines = new List<LineSegment>();
        Sloxels = new List<Sloxel>();
        MaxDistance = -1;
        MinDistance = 10000000;
        WallThickness = 1000000;
    }
    public List<Sloxel> Sloxels { get; set; }
    public float WallThickness { get; set; }
    public void SetMaxAndMin()
    {
        MaxDistance = 0;
        MinDistance = 1000000000;
        IntersectedByLines.Clear();
        WallThickness = 1000000;
        foreach (var slox in Sloxels)
        {
            if (slox.WallThickness < WallThickness)
                WallThickness = slox.WallThickness;
            foreach (var line in slox.IntersectedByLines)
            {
                if (!IntersectedByLines.Contains(line))
                {
                    IntersectedByLines.Add(line);
                }
            }
        }
        foreach (var inter in IntersectedByLines)
        {
            GetDistanceBetweenParallelLines();
        }
    }
    public float Id { get; set; }
    public GameObject Voxel { get; set; }
    public Vector3 ScanDirection { get; set; }
    public Vector3 Origin { get; set; }
    public List<LineSegment> IntersectedByLines { get; set; }
    public int MaxLine { get; internal set; }
    public int MinLine { get; internal set; }
    public float MaxDistance { get; internal set; }
    public float MinDistance { get; internal set; }
    public float MedianLineNumber { get; internal set; }
    public float StandardDeviation { get; internal set; }
    public float MeanDistance { get; internal set; }
    public bool Ready { get; set; }
    public SingleCube singleCube { get; set; }
    public void GetDistanceBetweenParallelLines()
    {
        var lineNumbers = new List<int>();
        MeanDistance = 0;
        var alreadyChecked = new List<LineSegment>();
        foreach (var inter in IntersectedByLines)
        {
            alreadyChecked.Add(inter);
            lineNumbers.Add(inter.step);
            foreach (var inter2 in IntersectedByLines)
            {
                if (!alreadyChecked.Contains(inter2))
                {
                    var A = Vector2.zero;
                    var B = Vector2.zero;
                    
                    if (inter.MovesInX && inter2.MovesInX)
                    {
                        A = new Vector2(inter.p1.z, inter.p1.y);
                        B = new Vector2(inter2.p1.z, inter2.p1.y);
                    }
                    else if (inter.MovesInZ && inter2.MovesInZ)
                    {
                        A = new Vector2(inter.p1.x, inter.p1.y);
                        B = new Vector2(inter2.p1.x, inter2.p1.y);
                    }
                    var d = Mathf.Abs(Vector2.Distance(A, B));
                    if (d > 0)
                    {
                        if (d < MinDistance)
                            MinDistance = d;
                        if (d > MaxDistance)
                            MaxDistance = d;
                        if (d > cSectionGCD.maxLineSepSloxels)
                            cSectionGCD.maxLineSepSloxels = d;
                        if (d < cSectionGCD.minLineSepSloxels)
                            cSectionGCD.minLineSepSloxels = d;
                    }
                    MeanDistance += d;
                }
            }
        }
        var count = lineNumbers.Count;
        if (count <= 1)
        {
            if (count == 0)
                MedianLineNumber = -1;
            else
                MedianLineNumber = lineNumbers[0];
            MeanDistance = 0;
            StandardDeviation = 0;
            return;
        }
        if (count % 2 == 0)
        {
            var a = lineNumbers[(int)Mathf.Floor((count - 1) / 2.0f)];
            var b = lineNumbers[(int)Mathf.Ceil((count - 1) / 2.0f)];
            MedianLineNumber = (a + b) / 2.0f;
        }
        else if (count % 2 != 0)
        {
            MedianLineNumber = lineNumbers[(int)Mathf.Floor((count - 1) / 2.0f)];
        }
        var sum = 0.0f;
        var variance = 0.0f;
        foreach (var ln in lineNumbers)
        {
            sum += ln;
        }
        var average = sum / lineNumbers.Count;
        foreach (var ln in lineNumbers)
        {
            variance += Mathf.Pow(average - ln, 2);
        }
        StandardDeviation = Mathf.Pow(variance, 0.5f);
        MeanDistance = MeanDistance / count; 
        
        foreach (var s in Sloxels)
        {
            var minSep = 1000000f;
            var maxSep = -1f;
            var alreadyDone = new List<LineSegment>();
            foreach (var l in s.IntersectedByLines)
            {
                alreadyDone.Add(l);
                var d = 0f;
              
                if (l.MovesInX)
                {
                    foreach (var other in s.IntersectedByLines)
                    {
                        if (other.MovesInX && !alreadyDone.Contains(other))
                        {
                            d = Mathf.Abs(l.p1.z - l.p2.z);
                            if (d < s.MinimumSeparation)
                                s.MinimumSeparation = d;
                        }
                    }
                }
                else if (l.MovesInZ)
                {
                    foreach (var other in s.IntersectedByLines)
                    {
                        if (other.MovesInZ && !alreadyDone.Contains(other))
                        {
                            d = Mathf.Abs(l.p1.x - l.p2.x);
                            if (d < s.MinimumSeparation)
                                s.MinimumSeparation = d;
                        }
                    }
                }
            }
        }       
    }
    public void DistanceBetweenSkewLines()
    {
        foreach (var inter in IntersectedByLines)
        {
            foreach (var inter2 in IntersectedByLines)
            {
                if (inter2 != inter)
                {
                    if (inter.p1 != inter2.p1 && inter.p1 != inter2.p2 && inter.p2 != inter2.p1 && inter.p2 != inter2.p2)
                    {
                        var slope1 = inter.p2 - inter.p1;
                        var slope2 = inter2.p2 - inter2.p2;
                        var eqn1 = new List<Vector2>(); //0 is x, 1 is y, 2 is z (t, c)
                        eqn1.Add(new Vector2(slope1.x, inter.p1.x));
                        eqn1.Add(new Vector2(slope1.y, inter.p1.y));
                        eqn1.Add(new Vector2(slope1.z, inter.p1.z));

                        var eqn2 = new List<Vector2>(); //0 is x, 1 is y, 2 is z (s, c) (Q)
                        eqn2.Add(new Vector2(slope2.x, inter2.p1.x));
                        eqn2.Add(new Vector2(slope2.y, inter2.p1.y));
                        eqn2.Add(new Vector2(slope2.z, inter2.p1.z));

                        var PQ = new List<Vector3>(); // (s,t,c)
                        PQ.Add(new Vector3(eqn2[0].x, -eqn1[0].x, eqn2[0].y - eqn1[0].y));//x
                        PQ.Add(new Vector3(eqn2[1].x, -eqn1[1].x, eqn2[1].y - eqn1[1].y));//y
                        PQ.Add(new Vector3(eqn2[2].x, -eqn1[2].x, eqn2[2].y - eqn1[2].y));//z

                        var dot_s1 = PQ[0].x * inter.p1.x + PQ[1].x * inter.p1.y + PQ[2].x * inter.p1.z;
                        var dot_t1 = PQ[0].y * inter.p1.x + PQ[1].y * inter.p1.y + PQ[2].y * inter.p1.z;
                        var dot_c1 = PQ[0].z * inter.p1.x + PQ[1].z * inter.p1.y + PQ[2].z * inter.p1.z;

                        var dot_s2 = PQ[0].x * inter.p2.x + PQ[1].x * inter.p2.y + PQ[2].x * inter.p2.z;
                        var dot_t2 = PQ[0].y * inter.p2.x + PQ[1].y * inter.p2.y + PQ[2].y * inter.p2.z;
                        var dot_c2 = PQ[0].z * inter.p2.x + PQ[1].z * inter.p2.y + PQ[2].z * inter.p2.z;

                        var s_ratio = dot_s1 / dot_s2;
                        var _t = dot_t1 - s_ratio * dot_t2;
                        var _c = dot_c1 - s_ratio * dot_c2;
                        var t = -_c / _t;
                        // plug back into dot1

                        var s = -(dot_t1 * t + dot_c1) / dot_s1;

                        var PQline = new Vector3();
                        PQline.x = PQ[0].x * s + PQ[1].x * s + PQ[2].x * s;
                        PQline.y = PQ[0].y * s + PQ[1].y * s + PQ[2].y * s;
                        PQline.z = PQ[0].z * s + PQ[1].z * s + PQ[2].z * s;

                        //var v = new Vector3(x, y, z);
                        var d = Vector3.Magnitude(PQline);
                        if (d < MinDistance)
                            MinDistance = d;
                        if (d > MaxDistance)
                            MaxDistance = d;
                    }
                }
            }
        }
    }
    
}
