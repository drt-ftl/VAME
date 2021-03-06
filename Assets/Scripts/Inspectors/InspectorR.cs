﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class InspectorR : InspectorManager
{
    public Texture2D hr;
    private Rect mainRect;
    private Rect thisRect;
    private Vector2 scrollPosition;
    private Vector2 IntScrollPosition;
    public GameObject testVoxel;
    public Transform centroid;
    public Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public PathVoxelizer pv;
    public static bool voxelsLoaded = false;
    public static bool voxelsFitted = false;

    public static float voxelVis = 100;
    public static float resolution = 20;
    private float negHighlight = 0;
    public static float highlight = 0;
    public static Vector3 highlightVector = new Vector3();
    public static float minPathIntersects = 0;
    public static float maxPathIntersects = 0;
    public static float minIntDist = 0;
    public static float maxIntDist = 0;
    public enum HighlighType { PathDensity, PathSeparation, PathNumbers, None}
    public static HighlighType highlightType;
    public int highlightTypeIndex = 0;
    private string[] pathToggleNames = new string[] { "None", "Count", "Sep", "%" };
    public Color voxelColor;
    private float lastMaxSep = 0;
    private float lastMinSep = 0;
    private float lastVoxelVis = 0;

    public static bool ShowPathIntersects = false;

    Slicer sli;

    void Start()
    {
        highlightType = HighlighType.None;
    }

    public Rect MainRect
    {
        get { return mainRect; }
        set
        {
            mainRect = value;
            thisRect = new Rect(margin, margin, mainRect.width - (margin * 2), mainRect.height - (margin * 2));
        }
    }
    public void InspectorWindowR(int id)
    {
       
        GUILayout.BeginArea(thisRect);
        {
            GUILayout.Space(25);

            GUILayout.Box(hr, "hr", GUILayout.Width(230), GUILayout.Height(10));
            GUILayout.Space(10);
            if (LoadFile.stlCodeLoaded || voxelsLoaded)
            {
                if (voxelsLoaded)
                    WhenVoxelsAreUp();
            }
            //else GUILayout.Label("Load STL file to voxelize.");
        }
        GUILayout.EndArea();
    }

    void WhenVoxelsAreUp()
    {
        if (voxelVis != lastVoxelVis) InspectorT.slicerForm.panel1.Invalidate();
        lastVoxelVis = voxelVis;
        voxelVis = GUILayout.HorizontalSlider(voxelVis, 0, 100, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Visibility: " + voxelVis.ToString("f2"), KeyStyle, GUILayout.Width(250));
        highlight = GUILayout.HorizontalSlider(highlight, 0, cSectionGCD.voxels.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Highlight: " + highlight.ToString("f0"), KeyStyle, GUILayout.Width(250));
        if (!cSectionGCD.voxels.ContainsKey(highlightVector)) return;
        var sloxList = cSectionGCD.layers[cSectionGCD.layerHeights[InspectorT.slicerForm.LayerTrackbar.Value]].Sloxels;
        var hSlox = sloxList[InspectorT.slicerForm.SloxelNumber.Value];
        var v = hSlox.Voxel;
        var str = "Voxel " + highlight.ToString("f0") + "\r\n";
        if (v.Voxel == null) return;
        str += "Position: " + v.Voxel.transform.position.ToString("f3") + "\r\n";
        str += "Temperature: " + "\r\n";
        str += "Position Error: " + "\r\n";
        GUILayout.Box(str, GUILayout.Width(220), GUILayout.Height(60));
        if (!voxelsFitted)
        {
            if (GUILayout.Button("Get Statistics"))
            {
                cSectionGCD.SetStatistics();
            }
            else
                return;
        }
        IntScrollPosition = GUILayout.BeginScrollView(IntScrollPosition, GUILayout.Width(225), GUILayout.Height(180));
        {
            var sloxVoxInfo = "<b>Voxel #: " + v.Id.ToString() + "\r\n";
            sloxVoxInfo += "Wall Thickness: " + v.WallThickness.ToString() + "\r\n";
            sloxVoxInfo += "Includes " + v.Sloxels.Count.ToString() + " Sloxels: \r\n</b>\r\n";
            foreach (var slox in v.Sloxels)
            {
                if (slox == hSlox)
                    sloxVoxInfo += "<color=red>";
                sloxVoxInfo += ("Layer: " + slox.Layer + ", " + "Sloxel #: " + slox.Id.ToString() + "\r\n");
                if (slox == hSlox)
                    sloxVoxInfo += "</color>";
            }
            GUILayout.Label(sloxVoxInfo, "i");
            var balls = "Contains " + v.CrazyBalls.Count.ToString() + " Data Points \r\n";
            if (v.CrazyBalls.Count >= 1)
            {
                var minError = 1000f;
                var maxError = 0f;
                var totalError = 0f;
                foreach (var ball in v.CrazyBalls)
                {
                    if (ball.ErrorDistance > minError)
                        minError = ball.ErrorDistance;
                    if (ball.ErrorDistance > maxError)
                        maxError = ball.ErrorDistance;
                    totalError += ball.ErrorDistance;
                    balls += "Point[" + ball.Id.ToString() + "] - Error: " + ball.ErrorDistance.ToString("f4") + "\r\n";
                }
                var averageError = totalError / v.CrazyBalls.Count;
                balls += "Average Error: " + averageError.ToString("f4");
            }
            GUILayout.Label(balls, "i2");
            var intersects = "Intersected By " + v.IntersectedByLines.Count.ToString() + " lines.\r\n";
            GUILayout.Label(intersects, "i2");
            
            GUILayout.Label("Median Line Number: " + v.MedianLineNumber.ToString("f0"), "i");
            GUILayout.Label("Line Number Std Dev: " + v.StandardDeviation.ToString("f3"), "i");
            if (v.MinDistance < 100)
                GUILayout.Label("Min Separation: " + v.MinDistance.ToString("f3"), "i");
            else
                GUILayout.Label("Min Separation: --", "i");
            if (v.MaxDistance > 0)
            {
                GUILayout.Label("Max Separation: " + v.MaxDistance.ToString("f3"), "i");
                GUILayout.Label("Mean Separation: " + v.MeanDistance.ToString("f3"), "i");
            }
            else
            {
                GUILayout.Label("Max Separation: --", "i");
                GUILayout.Label("Mean Separation: --", "i");
            }
            GUILayout.Space(8);
            foreach (var ints in v.IntersectedByLines)
            {
                var intersection = "[" + LoadFile.gcdLines.IndexOf(ints).ToString() + "]: " + "\r\n"
                    + "     P1: " + ints.p1.ToString("f3") + "," + "\r\n"
                    + "     P2: " + ints.p2.ToString("f3")
                    + "\r\n";
                GUILayout.Label(intersection, "i");
            }
            for (int q = 0; q <= 20; q++)
                GUILayout.Label("");
        }
        GUILayout.EndScrollView();
        highlightTypeIndex = GUILayout.Toolbar(highlightTypeIndex, pathToggleNames);
        switch (highlightTypeIndex)
        {
            case 0:
                highlightType = HighlighType.None;
                break;
            case 1:
                highlightType = HighlighType.PathDensity;
                minPathIntersects = GUILayout.HorizontalSlider(minPathIntersects, 0, maxPathIntersects);
                GUILayout.Label("Min #: " + minPathIntersects.ToString("f0"), KeyStyle, GUILayout.Width(220));
                maxPathIntersects = GUILayout.HorizontalSlider(maxPathIntersects, 1, PathFitter.maxPathsThrough);
                GUILayout.Label("Max #: " + maxPathIntersects.ToString("f0"), KeyStyle, GUILayout.Width(220));
                break;
            case 2:
                highlightType = HighlighType.PathSeparation;

                if (lastMaxSep != maxIntDist)
                    InspectorT.slicerForm.panel1.Invalidate();
                lastMaxSep = maxIntDist;
                if (lastMinSep != minIntDist)
                    InspectorT.slicerForm.panel1.Invalidate();
                lastMinSep = minIntDist;
                minIntDist = GUILayout.HorizontalSlider(minIntDist, 0, maxIntDist);
                GUILayout.Label("Min #: " + minIntDist.ToString("f4"), KeyStyle, GUILayout.Width(220));
                maxIntDist = GUILayout.HorizontalSlider(maxIntDist, 0, cSectionGCD.maxLineSepSloxels);
                GUILayout.Label("Max #: " + maxIntDist.ToString("f4"), KeyStyle, GUILayout.Width(220));
                break;
            case 3:
                highlightType = HighlighType.PathNumbers;
                break;
            default:
                break;
        }
        if (GUILayout.Button("Clear Voxels"))
        {
            ClearVoxels();
        }
    }

    public void SetVoxelsFitted ()
    {
        voxelsFitted = true;
    }
    void Update()
    {
        if (cSectionGCD.voxels.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && highlight < cSectionGCD.voxels.Count - 1)
                highlight++;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && highlight > 1)
                highlight--;
        }
    }

}
