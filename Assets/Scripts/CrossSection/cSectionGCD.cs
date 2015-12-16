using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cSectionGCD
{
    public static Vector4 csPlaneEqn = new Vector4(0f, 1f, 0f, 0f);
    public static List<Triangle> triangles = new List<Triangle>();
    public static Dictionary<float, CrossSectionLayer> layers = new Dictionary<float, CrossSectionLayer>();

    public static Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public static List<float> layerHeights = new List<float>();
    public static List<float> voxelHeights = new List<float>();
    public static List<Vector3> highlights = new List<Vector3>();
    public static bool ready = false;
    public enum CsMode { StepThrough, ByGcdCode, WallThickness, None }
    public static CsMode csMode;
    public static Vector3 sloxelResolution = Vector3.one * 8;
    static float increment = 0;
    private static float topLayerHeight = -1000000;
    private static float bottomLayerHeight = 1000000;
    private static float tlhGrid = -1000000;
    private static float blhGrid = 1000000;
    static float voxelHeight = 0;
    static int voxelLayers = 0;
    public GameObject Voxel;
    public float minLineSepSloxels = 1000000;
    public float maxLineSepSloxels = -1;
    public float minLineCountSloxels = 1000000;
    public float maxLineCountSloxels = -1;

    public cSectionGCD()
    {
        csMode = CsMode.None;
        ready = false;
        sloxelResolution = Vector3.one * (float)InspectorT.slicerForm.ResUpDown.Value;
        increment = 1.0f / sloxelResolution.x;
        csPlaneEqn = new Vector4(0f, 1f, 0f, 0f);
        triangles.Clear();
        layers.Clear();
        layerHeights.Clear();
        voxelHeights.Clear();
        highlights.Clear();
        foreach (var voxel in voxels)
        {
            MonoBehaviour.Destroy(voxel.Value.Voxel);
        }
        voxels.Clear();
        topLayerHeight = -1000000;
        bottomLayerHeight = 1000000;
        tlhGrid = -1000000;
        blhGrid = 1000000;
        voxelHeight = 0;
        voxelLayers = 0;
        minLineSepSloxels = 1000000;
        maxLineSepSloxels = -1;
        minLineCountSloxels = 1000000;
        maxLineCountSloxels = -1;
        InspectorR.voxelsFitted = false;

        foreach (var l in LoadFile.gcdLines)
        {
            if (!layers.ContainsKey(l.p1.y))
            {
                layers.Add(l.p1.y, new CrossSectionLayer(l.p1.y));
                layerHeights.Add(l.p1.y);
                if (l.p1.y < bottomLayerHeight)
                {
                    bottomLayerHeight = l.p1.y;
                    blhGrid = Mathf.Floor(bottomLayerHeight * sloxelResolution.x) * increment;
                }
                if (l.p1.y > topLayerHeight)
                {
                    topLayerHeight = l.p1.y;
                    tlhGrid = Mathf.Ceil(topLayerHeight * sloxelResolution.x) * increment;
                }
            }
            layers[l.p1.y].gcdLines.Add(l);
        }
        var modelHeight = topLayerHeight - bottomLayerHeight;
        var remainder = (int)(layers.Count % sloxelResolution.x);
        var sloxelsPerVoxel = (int)(layers.Count - remainder);
        voxelHeight = increment;// layers.Count / sloxelsPerVoxel;// (topLayerHeight - bottomLayerHeight + increment) / sloxelsPerVoxel;
        for (float i = bottomLayerHeight - voxelHeight / 2; i <= topLayerHeight + voxelHeight / 2; i += voxelHeight)
        {
            if (!voxelHeights.Contains(i))
                voxelHeights.Add(i);
        }

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
                newTriangle.Normal = MakeMesh.normals[3 * i + j];
            }
            newTriangle.PlaneEquation = GetPlane(newTriangle.Points);
            triangles.Add(newTriangle);
        }
        foreach (var layer in layers)
        {
            csPlaneEqn.w = layer.Key;
            CrossSection();
        }
        var wt = new CheckWalls();
        foreach (var layer in layers)
        {
            var min = layer.Value.Min;
            var max = layer.Value.Max;
            foreach (var line in layer.Value.border)
            {
                if (line.Endpoint0.x < min.x)
                    min.x = line.Endpoint0.x;
                if (line.Endpoint1.x < min.x)
                    min.x = line.Endpoint1.x;
                if (line.Endpoint0.x > max.x)
                    max.x = line.Endpoint0.x;
                if (line.Endpoint1.x > max.x)
                    max.x = line.Endpoint1.x;

                if (line.Endpoint0.z < min.z)
                    min.z = line.Endpoint0.z;
                if (line.Endpoint1.z < min.z)
                    min.z = line.Endpoint1.z;
                if (line.Endpoint0.z > max.z)
                    max.z = line.Endpoint0.z;
                if (line.Endpoint1.z > max.z)
                    max.z = line.Endpoint1.z;
            }
            
            layer.Value.Min = min;
            layer.Value.Max = max;
        }
        GetSloxels();
        //getXLines();
        //getZLines();
        ready = true;
        if (InspectorT.slicerForm != null)
        InspectorT.slicerForm.LayerTrackbar.Maximum = cSectionGCD.layers.Count - 1;
        InspectorR.voxelsLoaded = true;
        
        InspectorT.slicerForm.panel1.Invalidate();
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
        var norm = Vector3.Normalize(t.Normal);
        var P = t.Points[0];
        var Q = t.Points[1];
        var R = t.Points[2];
        // Get 3 Vectors
        var PQ = Q - P;
        var PR = R - P;
        var RQ = Q - R;
        if (P.y == h) // P is on plane
        {
            if (Q.y == h) //PQ ParallelToPlane
            {
                layers[h].border.Add(new csLine(P, Q, norm));
            }
            else if (R.y == h) //RP ParallelToPlane
            {
                layers[h].border.Add(new csLine(R, P, norm));
            }
            else if ((Q.y > h && R.y < h) || (Q.y < h && R.y > h))
            {
                layers[h].border.Add(new csLine(P, GetIntersectionPoint(R, Q), norm));
            }
        }
        else if (Q.y == h) // Q is on plane
        {
            if (R.y == h) // RQ Parallel
                layers[h].border.Add(new csLine(R, Q, norm));
            else if ((R.y > h && P.y < h) || (R.y < h && P.y > h))
            {
                layers[h].border.Add(new csLine(Q, GetIntersectionPoint(R, P), norm));
            }
        }
        else if (R.y == h &&
            ((Q.y > h && P.y < h) || (Q.y < h && P.y > h))) // R is on plane
        {
            layers[h].border.Add(new csLine(R, GetIntersectionPoint(P, Q), norm));
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
                layers[h].border.Add(new csLine(pts[0], pts[1], norm));
            }
        }

    }

    public Vector3 GetIntersectionPoint(Vector3 p0, Vector3 p1)
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

    public void getXLines ()
    {
        foreach (var layer in layers)
        {
            var inc = 1 / sloxelResolution.x;
            var left = Mathf.Floor(layer.Value.Min.x * sloxelResolution.x) / sloxelResolution.x;
            var right = Mathf.Ceil(layer.Value.Max.x * sloxelResolution.x) / sloxelResolution.x;
            for (float x = left; x <= right; x+= inc)
            {
                foreach (var line in layer.Value.border)
                {
                    csLine newLine;
                    var ep0 = line.Endpoint0;
                    var ep1 = line.Endpoint1;
                    bool passes = false;
                    if ((ep0.x <= x && ep1.x >= x)
                        || (ep1.x <= x && ep0.x >= x))
                        passes = true;
                    if (!passes) continue;
                    if (ep0.x == x && ep1.x == x)
                    {
                        newLine = new csLine(ep0, ep1, line.Normal);
                        newLine.WallThickness = line.WallThickness;
                        layer.Value.xLines.Add(newLine);
                    }
                    else
                    {
                        foreach (var other in layer.Value.border)
                        {
                            if (other == line) continue;
                            bool otherPasses = false;
                            if ((other.Endpoint0.x <= x && other.Endpoint1.x >= x)
                            || (other.Endpoint1.x <= x && other.Endpoint0.x >= x))
                                otherPasses = true;
                            if (!otherPasses) continue;
                            
                            var p1 = GetXIntersectionPoint(line.Endpoint0, line.Endpoint1, x);
                            var p2 = GetXIntersectionPoint(other.Endpoint0, other.Endpoint1, x);
                            newLine = new csLine(p1, p2, new Vector3(1, 0, 0));
                            newLine.WallThickness = Mathf.Min(line.WallThickness, other.WallThickness);
                            layer.Value.xLines.Add(newLine);
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetXIntersectionPoint(Vector3 p0, Vector3 p1, float x)
    {
        var pt = Vector3.zero;
        var slope = p1 - p0;
        var t = (x - p0.x) / slope.x;
        pt.x = x;
        pt.y = p0.y + slope.y * t;
        pt.z = p0.z + slope.z * t;
        return pt;
    }

    public void getZLines()
    {
        foreach (var layer in layers)
        {
            var inc = 1 / sloxelResolution.z;
            var left = Mathf.Floor(layer.Value.Min.z * sloxelResolution.z) / sloxelResolution.z;
            var right = Mathf.Ceil(layer.Value.Max.z * sloxelResolution.z) / sloxelResolution.z;
            for (float z = left; z <= right; z += inc)
            {
                foreach (var line in layer.Value.border)
                {
                    csLine newLine;
                    var ep0 = line.Endpoint0;
                    var ep1 = line.Endpoint1;
                    bool passes = false;
                    if ((ep0.z <= z && ep1.z >= z)
                        || (ep1.z <= z && ep0.z >= z))
                        passes = true;
                    if (!passes) continue;
                    if (ep0.z == z && ep1.z == z)
                    {
                        newLine = new csLine(ep0, ep1, line.Normal);
                        newLine.WallThickness = line.WallThickness;
                    }
                    else
                    {
                        newLine = null;
                        foreach (var other in layer.Value.border)
                        {
                            if (other == line) continue;
                            bool otherPasses = false;
                            if ((other.Endpoint0.z <= z && other.Endpoint1.z >= z)
                            || (other.Endpoint1.z <= z && other.Endpoint0.z >= z))
                                otherPasses = true;
                            if (!otherPasses) continue;

                            var p1 = GetZIntersectionPoint(line.Endpoint0, line.Endpoint1, z);
                            var p2 = GetZIntersectionPoint(other.Endpoint0, other.Endpoint1, z);
                            if (newLine == null || (Vector3.Magnitude(p1 - p2) > Vector3.Magnitude(newLine.Endpoint0 - newLine.Endpoint1)))
                            {
                                newLine = new csLine(p1, p2, new Vector3(0, 0, 1));
                                newLine.WallThickness = Mathf.Min(line.WallThickness, other.WallThickness);
                            }
                        }
                    }
                    if (newLine != null)
                        layer.Value.zLines.Add(newLine);
                }
            }
        }
    }

    public Vector3 GetZIntersectionPoint(Vector3 p0, Vector3 p1, float z)
    {
        var pt = Vector3.zero;
        var slope = p1 - p0;
        var t = (z - p0.z) / slope.z;
        pt.x = p0.x + slope.x * t;
        pt.y = p0.y + slope.y * t;
        pt.z = z;
        return pt;
    }

    public static void SetStatistics()
    {
        foreach (var _v in voxels)
        {
            var intersectsVoxel = new List<LineSegment>();
            var minLineNumber = 10000000;
            var maxLineNumber = 0;
            foreach (var s in _v.Value.Sloxels)
            {
                foreach (var l in s.IntersectedByLines)
                    intersectsVoxel.Add(l);
            }
            _v.Value.IntersectedByLines = intersectsVoxel;
            _v.Value.SetMaxAndMin();
        }
        foreach (var l in layers)
        {
            foreach (var s in l.Value.Sloxels)
            {
                var linesThrough = s.IntersectedByLines.Count;
            }
        }
        InspectorR.voxelsFitted = true;
    }
    public void GetSloxels ()
    {
        foreach (var layer in layers)
        {
            var y = layer.Key;
            var incX = 1 / sloxelResolution.x;
            var left = Mathf.Floor(layer.Value.Min.x * sloxelResolution.x) / sloxelResolution.x;
            var right = Mathf.Ceil(layer.Value.Max.x * sloxelResolution.x) / sloxelResolution.x;
            var incZ = 1 / sloxelResolution.z;
            var near = Mathf.Floor(layer.Value.Min.z * sloxelResolution.z) / sloxelResolution.z;
            var far = Mathf.Ceil(layer.Value.Max.z * sloxelResolution.z) / sloxelResolution.z;
            for (float z = near - 1.5f * incZ; z <= far + 1.5f * incZ; z += incZ)
            {
                var crosses = new List<Vector3>();
                var crossesNear = new List<Vector3>();
                var crossesFar = new List<Vector3>();
                foreach (var borderLine in layer.Value.border)
                {
                    var minZ = Mathf.Min(borderLine.Endpoint0.z, borderLine.Endpoint1.z) - 2 * incX;
                    var maxZ = Mathf.Max(borderLine.Endpoint0.z, borderLine.Endpoint1.z) + 2 * incX;
                    if (minZ > z || maxZ < z) continue;
                    var slope = borderLine.Endpoint1 - borderLine.Endpoint0;
                    var t = (z - borderLine.Endpoint0.z) / slope.z;
                    var x = borderLine.Endpoint0.x + t * slope.x;
                    var newCross = new Vector3(x, y, z);
                    if (t >= 0 && t <= 1 && !crosses.Contains(newCross))
                        crosses.Add(newCross);

                    t = (z - borderLine.Endpoint0.z - incZ / 2) / slope.z;
                    newCross.x = borderLine.Endpoint0.x + t * slope.x;
                    if (t >= 0 && t <= 1 && !crossesNear.Contains(newCross))
                        crossesNear.Add(newCross);

                    t = (z - borderLine.Endpoint0.z + incZ / 2) / slope.z;
                    newCross.x = borderLine.Endpoint0.x + t * slope.x;
                    
                    if (t >= 0 && t <= 1 && !crossesFar.Contains(newCross))
                        crossesFar.Add(newCross);
                    var minX = Mathf.Min(borderLine.Endpoint0.x, borderLine.Endpoint1.x);
                    var maxX = Mathf.Max(borderLine.Endpoint0.x, borderLine.Endpoint1.x);
                }
                for (float x = left - 2.5f * incX; x <= right + 2.5f * incX; x += incX)
                {
                    var leftsN = 0;
                    var rightsN = 0;
                    var leftsC = 0;
                    var rightsC = 0;
                    var leftsF = 0;
                    var rightsF = 0;
                    var leftsN_L = 0;
                    var rightsN_L = 0;
                    var leftsC_L = 0;
                    var rightsC_L = 0;
                    var leftsF_L = 0;
                    var rightsF_L = 0;
                    var leftsN_R = 0;
                    var rightsN_R = 0;
                    var leftsC_R = 0;
                    var rightsC_R = 0;
                    var leftsF_R = 0;
                    var rightsF_R = 0;
                    foreach (var cross in crosses)
                    {   // DRT Change here to expand range
                        if (cross.x <= x)
                            leftsC++;
                        if (cross.x >= x)
                            rightsC++;
                        if (cross.x <= x - incX / 2)
                            leftsC_L++;
                        if (cross.x >= x - incX / 2)
                            rightsC_L++;
                        if (cross.x <= x + incX / 2)
                            leftsC_R++;
                        if (cross.x >= x + incX / 2)
                            rightsC_R++;
                    }
                    foreach (var crossNear in crossesNear)
                    {
                        if (crossNear.x <= x)
                            leftsN++;
                        if (crossNear.x >= x)
                            rightsN++;
                        if (crossNear.x <= x - incX / 2)
                            leftsN_L++;
                        if (crossNear.x >= x - incX / 2)
                            rightsN_L++;
                        if (crossNear.x <= x + incX / 2)
                            leftsN_R++;
                        if (crossNear.x >= x + incX / 2)
                            rightsN_R++;
                    }
                    foreach (var crossFar in crossesFar)
                    {
                        if (crossFar.x <= x)
                            leftsF++;
                        if (crossFar.x >= x)
                            rightsF++;
                        if (crossFar.x <= x - incX / 2)
                            leftsF_L++;
                        if (crossFar.x >= x - incX / 2)
                            rightsF_L++;
                        if (crossFar.x <= x + incX / 2)
                            leftsF_R++;
                        if (crossFar.x >= x + incX / 2)
                            rightsF_R++;
                    }
                    if ((leftsC % 2 == 1 && rightsC % 2 == 1)
                        || (leftsN % 2 == 1 && rightsN % 2 == 1)
                        || (leftsF % 2 == 1 && rightsF % 2 == 1)
                        || (leftsC_L % 2 == 1 && rightsC_L % 2 == 1)
                        || (leftsN_L % 2 == 1 && rightsN_L % 2 == 1)
                        || (leftsF_L % 2 == 1 && rightsF_L % 2 == 1)
                        || (leftsC_R % 2 == 1 && rightsC_R % 2 == 1)
                        || (leftsN_R % 2 == 1 && rightsN_R % 2 == 1)
                        || (leftsF_R % 2 == 1 && rightsF_R % 2 == 1)
                        || leftsC_L != leftsC_R
                        || leftsN_L != leftsN_R
                        || leftsF_L != leftsF_R)
                    {
                        AddSloxel(new Vector3(x, y, z), layer.Value);
                    }
                }
            }
        }
    }

    public void AddSloxel(Vector3 centroid, CrossSectionLayer layer)
    {        
        var slox = new Sloxel();
        slox.Position = centroid;
        slox.Dim = sloxelResolution.x;
        layer.Sloxels.Add(slox);
        var inc = 1 / sloxelResolution.x;
        foreach (var voxelLayer in voxelHeights)
        {
            if (Mathf.Abs(centroid.y - voxelLayer) <= voxelHeight / 2)
            {
                var voxPos = new Vector3(centroid.x, voxelLayer, centroid.z);
                if (!voxels.ContainsKey(voxPos))
                {
                    var voxObj = MonoBehaviour.Instantiate(GameObject.Find("VOXELIZER").GetComponent<MeshVoxelizer>().voxel, voxPos, Quaternion.identity) as GameObject;
                    voxObj.transform.SetParent(GameObject.Find("VOXELIZER").transform);
                    voxObj.GetComponent<SingleCube>().Voxel();
                    voxObj.GetComponent<SingleCube>().Id = voxels.Count;
                    voxObj.GetComponent<SingleCube>().Origin = voxPos;
                    var scale = increment * 0.9f;
                    voxObj.transform.localScale = new Vector3(scale, 0.9f * voxelHeight, scale);
                    var small = SingleCube.scale;
                    voxObj.GetComponent<BoxCollider>().size = new Vector3(small, small, small);
                    var newVox = new VoxelClass();
                    newVox.Id = (float)voxels.Count;
                    newVox.Origin = voxPos;
                    newVox.singleCube = voxObj.GetComponent<SingleCube>();
                    voxObj.GetComponent<SingleCube>().voxelClass = newVox;
                    newVox.Voxel = voxObj;
                    voxels.Add(voxPos, newVox);
                    highlights.Add(voxPos);
                    InspectorR.highlightVector = voxPos;
                }
                foreach (var line in layer.gcdLines)
                {
                    if (line.MovesInX)
                    {
                        if (line.p1.z >= slox.Position.z - inc / 2 && line.p1.z <= slox.Position.z + inc / 2)
                        {
                            var m = line.p2 - line.p1;
                            var sloxMinX = slox.Position.x - inc / 2;
                            var sloxMaxX = slox.Position.x + inc / 2;
                            var factorMinX = (sloxMinX - line.p1.x) / m.x;
                            var factorMaxX = (sloxMaxX - line.p1.x) / m.x;
                            if ((factorMinX >= 0 && factorMinX <= 1)
                                || (factorMaxX >= 0 && factorMaxX <= 1))
                            {
                                if (line.step < slox.MinLineNumber)
                                    slox.MinLineNumber = line.step;
                                if (line.step > slox.MaxLineNumber)
                                    slox.MaxLineNumber = line.step;
                                if (!slox.IntersectedByLines.Contains(line))
                                    slox.IntersectedByLines.Add(line);
                            }
                        }
                    }
                    else if (line.MovesInZ)
                    {
                        if (line.p1.x >= slox.Position.x - inc / 2 && line.p1.x <= slox.Position.x + inc / 2)
                        {
                            var m = line.p2 - line.p1;
                            var sloxMinZ = slox.Position.z - inc / 2;
                            var sloxMaxZ = slox.Position.z + inc / 2;
                            var factorMinZ = (sloxMinZ - line.p1.z) / m.z;
                            var factorMaxZ = (sloxMaxZ - line.p1.z) / m.z;
                            if ((factorMinZ >= 0 && factorMinZ <= 1)
                                || (factorMaxZ >= 0 && factorMaxZ <= 1))
                            {
                                if (line.step < slox.MinLineNumber)
                                    slox.MinLineNumber = line.step;
                                if (line.step > slox.MaxLineNumber)
                                    slox.MaxLineNumber = line.step;
                                if (!slox.IntersectedByLines.Contains(line))
                                    slox.IntersectedByLines.Add(line);
                            }
                        }
                    }
                }
                var v = voxels[voxPos];
                if (slox.MinLineNumber < v.MinLine)
                    v.MinLine = slox.MinLineNumber;
                if (slox.MaxLineNumber > v.MaxLine)
                    v.MaxLine = slox.MaxLineNumber;
                v.Sloxels.Add(slox);
                slox.Voxel = v;
                if (!layer.Voxels.Contains(v))
                {
                    layer.Voxels.Add(v);
                }
                slox.VoxelOrigin = voxPos;
            }
        }
    }

    public float RoundToLayer (float layerY, float numToRound)
    {
        float rounded = 0;
        return rounded;
    }

    public static void ClearAll()
    {
        csPlaneEqn = new Vector4(0f, 1f, 0f, 0f);
        triangles = new List<Triangle>();
        layers = new Dictionary<float, CrossSectionLayer>();
        voxels = new Dictionary<Vector3, VoxelClass>();
        layerHeights = new List<float>();
        voxelHeights = new List<float>();
        highlights = new List<Vector3>();
        ready = false;
        topLayerHeight = -1000000;
        bottomLayerHeight = 1000000;
        tlhGrid = -1000000;
        blhGrid = 1000000;
        voxelHeight = 0;
        voxelLayers = 0;
}
}
