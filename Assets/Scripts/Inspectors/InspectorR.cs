using UnityEngine;
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

    public static float voxelVis;
    public static float resolution = 20;
    private float negHighlight = 0;
    public static float highlight = 0;
    public static float minPathIntersects = 0;
    public static float maxPathIntersects = 0;
    public enum HighlighType { PathDensity, PathSeparation, PathNumbers, None}
    public static HighlighType highlightType;
    public int highlightTypeIndex = 0;
    private string[] pathToggleNames = new string[] { "None", "Count", "Sep", "%" };

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
            //if (GUILayout.Button("Slice"))
            //{
            //    sli = GameObject.Find("SLICER").GetComponent<Slicer>();
            //    sli.Slice();
            //}


            if (LoadFile.stlCodeLoaded)
            {
                if (GUILayout.Button("Voxelize"))
                {
                    foreach (var v in MeshVoxelizer.voxels)
                    {
                        v.Value.IntersectedByLines.Clear();
                    }
                    voxelsFitted = false;
                    highlight = 0;
                    foreach (var v in MeshVoxelizer.voxels)
                        Destroy(v.Value.Voxel);
                    MeshVoxelizer.voxels.Clear();
                    MeshVoxelizer.highlights.Clear();
                    //GameObject.Find("VOXELIZER").GetComponent<SurfaceVoxelizer>().VoxelizeSurfaces((int)resolution);
                    GameObject.Find("VOXELIZER").GetComponent<MeshVoxelizer>().VoxelizeSurfaces((int)resolution);
                    voxelVis = 100;
                    voxelsLoaded = true;
                }

                resolution = GUILayout.HorizontalSlider(resolution, 1, 25, GUILayout.Width(220), GUILayout.Height(12));
                var res = (int)resolution;
                GUILayout.Label("Resolution: " + res.ToString(), KeyStyle, GUILayout.Width(250));
                GameObject.Find("VOXELIZER").GetComponent<MeshVoxelizer>().divisions = res;
                if (LoadFile.gcdCodeLoaded && voxelsLoaded)
                {
                    if (GUILayout.Button("Fit Lines"))
                    {
                        foreach (var v in MeshVoxelizer.voxels)
                        {
                            v.Value.IntersectedByLines.Clear();
                        }
                        GameObject.Find("VOXELIZER").GetComponent<PathFitter>().FitPaths();
                    }
                }
                if (voxelsLoaded)
                    WhenVoxelsAreUp();
                GUILayout.Space(18);
            }
            else GUILayout.Label("Load STL file to voxelize.");
        }
        GUILayout.EndArea();
    }

    void WhenVoxelsAreUp()
    {
        voxelVis = GUILayout.HorizontalSlider(voxelVis, 0, 100, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Visibility: " + voxelVis.ToString("f2"), KeyStyle, GUILayout.Width(250));
        highlight = GUILayout.HorizontalSlider(highlight, 0, MeshVoxelizer.voxels.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Highlight: " + highlight.ToString("f0"), KeyStyle, GUILayout.Width(250));
        var v = MeshVoxelizer.voxels[MeshVoxelizer.highlights[(int)highlight]];
        var str = "Voxel " + highlight.ToString("f0") + "\r\n";
        str += "Position: " + v.Voxel.transform.position.ToString("f3") + "\r\n";
        str += "Temperature: " + "\r\n";
        str += "Position Error: " + "\r\n";
        GUILayout.Box(str, GUILayout.Width(220), GUILayout.Height(60));
        if (!voxelsFitted) return;
        IntScrollPosition = GUILayout.BeginScrollView(IntScrollPosition, GUILayout.Width(225), GUILayout.Height(180));
        {
            var intersects = "Intersected By " + v.IntersectedByLines.Count.ToString() + " lines.\r\n";
            GUILayout.Label(intersects, "i2");
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
        switch(highlightTypeIndex)
        {
            case 0:
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
                break;
            case 3:
                highlightType = HighlighType.PathNumbers;
                break;
            default:
                break;
        }
    }

    public void SetVoxelsFitted ()
    {
        voxelsFitted = true;
    }
    void Update()
    {
        if (MeshVoxelizer.voxels.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && highlight < MeshVoxelizer.voxels.Count - 1)
                highlight++;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && highlight > 1)
                highlight--;
        }
    }

}
