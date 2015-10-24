using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cSection
{
    private float layerHeight = 0.0015f;
    public static Vector4 csPlaneEqn = new Vector4(0f, 1f, 0f, 0f);
    public static List<Triangle> triangles = new List<Triangle>();
    public static Dictionary<float, List<csLine>> csLines = new Dictionary<float, List<csLine>>();
    public static bool ready = true;

    public cSection(float _layerHeight)
    {
        ready = false;
        layerHeight = _layerHeight;
        var Min = LoadFile.stlInterpreter.Min.y - LoadFile.stlInterpreter.centroid.y - layerHeight;
        var Max = LoadFile.stlInterpreter.Max.y - LoadFile.stlInterpreter.centroid.y + layerHeight;

        var verts = new List<Vector3>();
        foreach (var _v in MakeMesh.verts)
        {
            verts.Add(_v);
        }
        var count = (int)(verts.Count / 3);
        for (int i = 0; i < count; i++)
        {
            var newTriangle = new Triangle();
            for (int j = 0; j <= 2; j++)
            {
                if (verts[3 * i + j].y > newTriangle.Max) newTriangle.Max = verts[3 * i + j].y;
                if (verts[3 * i + j].y < newTriangle.Min) newTriangle.Min = verts[3 * i + j].y;
                newTriangle.Points[j] = verts[3 * i + j];
            }
            newTriangle.PlaneEquation = GetPlane(newTriangle.Points);
            triangles.Add(newTriangle);
        }
        for (float y = Min; y <= Max; y += layerHeight)
        {
            csPlaneEqn.w = y;
            csLines.Add(y, new List<csLine>());
            CrossSection();
        }
        ready = true;
        var wt = new CheckWalls();
    }
    public void CrossSection()
    {
        foreach (var t in triangles)
        {
            if (CheckForCross(t.Min, t.Max))
            {
                GetLineEqn(t);
            }
        }
    }
    public bool CheckForCross(float min, float max)
    {
        var y = csPlaneEqn.w;
        if ((min <= y && max >= y))
        {
            return true;
        }
        return false;
    }

    public Vector4 GetPlane(Vector3[] v)
    {
        // Get 3 Points
        var P = v[0];
        var Q = v[1];
        var R = v[2];
        // Get 3 Vectors
        var PQ = Q - P;
        var PR = R - P;
        var RQ = Q - R;
        // Take cross product of two vectors to get Normal Vector
        var n = Vector3.Cross(PQ, PR);
        //Take Dot Product of Normal and any point on plane to get constant value
        var d = Vector3.Dot(n, P);
        var planeEqn = new Vector4(n.x, n.y, n.z, d);
        return planeEqn;
    }

    public void GetLineEqn(Triangle t)
    {
        var h = csPlaneEqn.w;
        var P = t.Points[0];
        var Q = t.Points[1];
        var R = t.Points[2];
        // Get 3 Vectors
        var PQ = Q - P;
        var PR = R - P;
        var RQ = Q - R;
        if (P.y == h)
        {
            if (Q.y == h) //PQ ParallelToPlane
            {
                csLines[h].Add(new csLine(P, Q));
            }
            else if (R.y == h) //RP ParallelToPlane
            {
                csLines[h].Add(new csLine(R, P));
            }
            else if ((Q.y > h && R.y < h) || (Q.y < h && R.y > h))
            {
                csLines[h].Add(new csLine(P, GetIntersectionPoint(R, Q)));
            }
        }
        else if (Q.y == h)
        {
            if (R.y == h) // RQ Parallel
                csLines[h].Add(new csLine(R, Q));
            else if ((R.y > h && P.y < h) || (R.y < h && P.y > h))
            {
                csLines[h].Add(new csLine(Q, GetIntersectionPoint(R, P)));
            }
        }
        else if (R.y == h &&
            ((Q.y > h && P.y < h) || (Q.y < h && P.y > h)))
        {
            csLines[h].Add(new csLine(R, GetIntersectionPoint(P, Q)));
        }
        else // no vertices on plane
        {
            var pts = new List<Vector3>();
            if ((P.y > h && Q.y < h) || (P.y < h && Q.y > h))
            {
                pts.Add(GetIntersectionPoint(P, Q));
            }
            if ((P.y > h && R.y < h) || (P.y < h && R.y > h))
            {
                pts.Add(GetIntersectionPoint(P, R));
            }
            if ((Q.y > h && R.y < h) || (Q.y < h && R.y > h))
            {
                pts.Add(GetIntersectionPoint(Q, R));
            }
            if (pts.Count > 1)
            {
                csLines[h].Add(new csLine(pts[0], pts[1]));
            }
        }
        
    }

    public Vector3 GetIntersectionPoint (Vector3 p0, Vector3 p1)
    {
        var h = csPlaneEqn.w;
        var pt = Vector3.zero;
        var slope = p1 - p0;
        var t = (h - p0.y) / slope.y;
        pt.x = p0.x + slope.x * t;
        pt.y = h;
        pt.z = p0.z + slope.z * t;
        return pt;
    }
}
