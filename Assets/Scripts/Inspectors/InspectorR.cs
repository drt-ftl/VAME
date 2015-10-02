using UnityEngine;
using System.Collections.Generic;

public class InspectorR : InspectorManager
{
    public Texture2D hr;
    private Rect mainRect;
    private Rect thisRect;
    private Vector2 scrollPosition;
    public GameObject testVoxel;
    public Transform centroid;
    public Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public VoxelizeSTL2 vs2;
    private bool voxelsLoaded = false;

    public static float voxelVis;
    public static float resolution = 20;
    private float negHighlight = 0;
    public static float highlight = 0;


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
            if (LoadFile.stlCodeLoaded)
            {
                if (GUILayout.Button("Voxelize"))
                {
                    highlight = 0;
                    foreach (var v in vs2.voxels)
                        Destroy(v.Value.Voxel);
                    vs2.voxels.Clear();
                    vs2.highlights.Clear();
                    vs2.voxelize();
                    voxelVis = 100;
                    voxelsLoaded = true;
                }

                resolution = GUILayout.HorizontalSlider(resolution, 2, 25, GUILayout.Width(220), GUILayout.Height(12));
                var res = (int)resolution;
                GUILayout.Label("Resolution: " + res.ToString(), KeyStyle, GUILayout.Width(250));
                vs2.divisions = res;

                if (vs2.voxels.Count > 0)
                {
                    WhenVoxelsAreUp();
                }


                GUILayout.Space(18);
            }
            else GUILayout.Label("Load STL model to voxelize.");
        }
        GUILayout.EndArea();
    }

    void WhenVoxelsAreUp()
    {
        voxelVis = GUILayout.HorizontalSlider(voxelVis, 0, 100, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Visibility: " + voxelVis.ToString("f2"), KeyStyle, GUILayout.Width(250));
        highlight = GUILayout.HorizontalSlider(highlight, 0, vs2.voxels.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
        GUILayout.Label("Highlight: " + highlight.ToString("f0"), KeyStyle, GUILayout.Width(250));
        var v = vs2.voxels[vs2.highlights[(int)highlight]];
        var str = "Voxel " + highlight.ToString("f0") + "\r\n";
        str += "Position: " + v.Voxel.transform.position.ToString("f3") + "\r\n";
        str += "Temperature: " + "\r\n";
        str += "Position Error: " + "\r\n";
        GUILayout.Box(str, GUILayout.Width(220), GUILayout.Height(100));
        //if (LoadFile.dmcCodeLoaded && GUILayout.Button("Fit To Paths"))
        //{
        //    var dim = VoxelizeSTL2.finalDim;
        //    var offset = VoxelizeSTL2.offset;
        //    foreach (var line in LoadFile.dmcLines)
        //    {
        //        var _p1 = line.p1 / dim;
        //        var _p2 = line.p2 / dim;
        //        if (vs2.voxels.ContainsKey(new Vector3((int)_p1.x * dim, (int)_p1.y * dim, (int)_p1.z * dim)))
        //        {
        //            print("p1" + _p1.ToString());
        //        }
        //    }
        //}
    }

    void Update()
    {
        if (vs2.voxels.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && highlight < vs2.voxels.Count - 1)
                highlight++;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && highlight > 1)
                highlight--;
        }
    }
}
