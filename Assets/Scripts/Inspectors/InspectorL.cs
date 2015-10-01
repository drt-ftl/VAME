using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InspectorL : InspectorManager
{
    #region declarations
    public Texture2D hr;
    private Rect mainRect;
    private Rect thisRect;
    private Vector2 scrollPosition;
    public static float stlVisSlider = 1;
    public static float dmcVisSlider = 1;
    public static float jobVisSlider = 1;
    public static float stlTimeSlider = 1;
    public static float stlTimeSliderPrev = 1;
    public static float stlTimeSliderMin = 1;
    public static float dmcTimeSlider = 1;
    public static float dmcTimeSliderPrev = 1;
    public static float dmcTimeSliderMin = 1;
    public static float dmcCodeSlider = 1;
    public static float jobTimeSlider = 1;
    private int typeInt = 0;
    private string[] toolbarStrings = { "STL", "DMC", "JOB" };
    public static Color stlColor = new Color(1.0f, 0.5f, 0.5f, 1f);
    public static Color dmcLineColor = new Color(0f, 0.7f, 0f, 1f);
    public static Color jobLineColor = new Color(1f, 0f, 0f, 1f);
    private Vector3 _p1;
    private Vector3 _p2;
    private Vector3 _p3;
    private float _p1x;
    private float _p1y;
    private float _p1z;
    private float _p2x;
    private float _p2y;
    private float _p2z;
    private float _p3x;
    private float _p3y;
    private float _p3z;
    private List<string> prevTypeList = new List<string>();
    public enum LastLoaded { STL,DMC,JOB,None};
    public static LastLoaded lastLoaded;
    private float voxelVis;
    private int dmcCodeOffset = 0;
    private int stlCodeOffset = 0;
    int max = 50;
    #endregion
    public Rect MainRect
    {
        get { return mainRect; }
        set
        {
            mainRect = value;
            thisRect = new Rect(margin, margin, mainRect.width - (margin * 2), mainRect.height - (margin * 2));
        }
    }
    public void InspectorWindowL(int id)
    {
        var availableToggles = GetAvailableToggles();
        GUILayout.BeginArea(thisRect);
        {
            GUILayout.Space(25);
            GUILayout.Box(hr, "hr", GUILayout.Width(250), GUILayout.Height(10));
            GUILayout.Space(10);
            // Load
            if (!LoadFile.loading && (!LoadFile.stlCodeLoaded || !LoadFile.dmcCodeLoaded))
            {
                if (GUILayout.Button("Load"))
                {
                    LoadFile.loading = true;
                    GetComponent<LoadFile>().loadFile();
                }
            }
            // Quit
            if (GUILayout.Button("Quit"))
            {
                Application.Quit();
            }

            if (availableToggles.Length > 0)
            {
                // Toggle File Type
                typeInt = GUILayout.Toolbar(typeInt, availableToggles, GUILayout.Width(220));

                // Visibility
                VisibiltySlider();
                GUILayout.Space(18);

                //Code Panel
                GUILayout.Box("<color=lime>" + GetCode()[0] + "</color>" + GetCode()[1], GUILayout.Width(225), GUILayout.Height(220));
                GUILayout.Space(10);

                // Time Sliders
                TimeSliders();

                //Coordinates
                CoordinateBoxes();
            }

        }
        GUILayout.EndArea();
    }
    public void VisibiltySlider()
    {
        switch (GetAvailableToggles()[typeInt])
        {
            case "STL":
                stlVisSlider = GUILayout.HorizontalSlider(stlVisSlider, 0, 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Visibility: " + (stlVisSlider * 100).ToString("f0"), KeyStyle, GUILayout.Width(250));
                stlColor.a = stlVisSlider;

                break;
            case "DMC":
                dmcVisSlider = GUILayout.HorizontalSlider(dmcVisSlider, 0, 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Visibility: " + (dmcVisSlider * 100).ToString("f0"), KeyStyle, GUILayout.Width(250));
                foreach (var line in LoadFile.dmcLines)
                {
                    if (line.Line != null)
                    {
                        dmcLineColor.a = dmcVisSlider * dmcVisSlider;
                        line.LineColor = dmcLineColor;
                    }
                }
                break;
            default:
                break;
        }
    }
    public void TimeSliders()
    {
        switch (GetAvailableToggles()[typeInt])
        {
            case "STL":
                var indexSTL = 0;
                stlTimeSlider = GUILayout.HorizontalSlider(stlTimeSlider, 1, GameObject.Find("MESH").GetComponent<MakeMesh>().GetMesh().vertices.Length / 3 - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Time Max: " + stlTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(220));
                stlTimeSliderMin = GUILayout.HorizontalSlider(stlTimeSliderMin, 0, stlTimeSlider - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Time Min: " + stlTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(250));
                GUILayout.Label(scrollPosition.y.ToString(), KeyStyle, GUILayout.Width(250));
                if (stlTimeSlider != stlTimeSliderPrev)
                    scrollPosition.y = 0;
                stlTimeSliderPrev = stlTimeSlider;
                break;
            case "DMC":
                dmcTimeSlider = GUILayout.HorizontalSlider(dmcTimeSlider, 1, LoadFile.dmcLines.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Time Max: " + dmcTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(250));
                dmcTimeSliderMin = GUILayout.HorizontalSlider(dmcTimeSliderMin, 0, dmcTimeSlider - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Time Min: " + dmcTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(250));

                GUILayout.Label(scrollPosition.y.ToString(), KeyStyle, GUILayout.Width(250));
                if (dmcTimeSlider != dmcTimeSliderPrev)
                    scrollPosition.y = 0;
                dmcTimeSliderPrev = dmcTimeSlider;

                var indexDMC = 0;
                foreach (var line in LoadFile.dmcLines)
                {
                    if (line.Line != null)
                    {
                        if ((int)dmcTimeSlider < indexDMC || (int)dmcTimeSliderMin > indexDMC)
                        {
                            line.LineColor = new Color(dmcLineColor.r, dmcLineColor.g, dmcLineColor.b, 0f);
                        }
                        else if ((int)dmcTimeSlider == indexDMC)
                        {
                            line.LineWidth = 0.01f;
                            line.LineColor = new Color(1f, 1f, 1f, dmcLineColor.a);
                            _p1 = line.p1;
                            _p2 = line.p2;
                        }
                        else
                        {
                            line.LineWidth = 0.002f;
                            line.LineColor = dmcLineColor;
                        }
                    }
                    indexDMC++;
                }
                break;
            default:
                break;
        }
    }
    private string[] GetAvailableToggles ()
    {
        var list = new List<string>();
        if (LoadFile.stlCodeLoaded)
            list.Add("STL");
        if (LoadFile.dmcCodeLoaded)
            list.Add("DMC");
        if (LoadFile.jobCodeLoaded)
            list.Add("JOB");
        if (list.Contains(lastLoaded.ToString()))
        {
            typeInt = list.IndexOf(lastLoaded.ToString());
            lastLoaded = LastLoaded.None;
        }
        return list.ToArray();
    }
    private string[] GetCode ()
    {
        var code = new string[2];
        var first = "";
        var bulk = "";
        switch (GetAvailableToggles()[typeInt])
        {
            case "STL":
                max = 50;
                if (max > LoadFile.stlCode.Count - 1)
                    max = LoadFile.stlCode.Count - 1;
                if (stlTimeSlider >= LoadFile.firstStlLineInCode)
                {
                    var ts = (int)stlTimeSlider;
                    if (LoadFile.model_code_xrefSTL.Count - 1 < ts)
                        ts = LoadFile.model_code_xrefSTL.Count - 1;
                    var firstLineIndex = LoadFile.model_code_xrefSTL[ts];
                    first = LoadFile.stlCode[firstLineIndex];
                    for (int i = firstLineIndex + 1; i < firstLineIndex + max; i++)
                    {
                        if (LoadFile.stlCode.Count > i)
                            bulk += LoadFile.stlCode[i];
                    }
                }
                else
                {
                    first = LoadFile.stlCode[LoadFile.firstStlLineInCode];
                    for (int i = LoadFile.firstStlLineInCode + 1; i < (int)stlTimeSlider + max; i++)
                    {
                        if (LoadFile.stlCode.Count > i)
                            bulk += LoadFile.stlCode[i];
                    }
                }
                break;
            case "DMC":
                max = 50;
                if (max > LoadFile.dmcCode.Count - 1)
                    max = LoadFile.dmcCode.Count - 1;
                if (dmcTimeSlider >= LoadFile.firstDmcLineInCode)
                {
                    var ts = (int)dmcTimeSlider;
                    if (LoadFile.model_code_xrefDMC.Count - 1 < ts)
                        ts = LoadFile.model_code_xrefDMC.Count - 1;
                    var firstLineIndex = LoadFile.model_code_xrefDMC[ts];
                    first = LoadFile.dmcCode[firstLineIndex];
                    
                    for (int i = firstLineIndex + 1; i < firstLineIndex + max; i++)
                    {
                        if (LoadFile.dmcCode.Count > i)
                            bulk += LoadFile.dmcCode[i];
                    }
                }
                else
                {
                    for (int i = (int)dmcTimeSlider; i < (int)dmcTimeSlider + max; i++)
                    {
                        if (LoadFile.dmcCode.Count > i)
                            bulk += LoadFile.dmcCode[i];
                    }
                }
                break;
            default:
                break;
        }
        code[0] = first;
        code[1] = bulk;
        return code;
    }
    private void CoordinateBoxes()
    {
        GUILayout.BeginHorizontal();
        switch (GetAvailableToggles()[typeInt])
        {
            #region STL
            case "STL":
                var p = GameObject.Find("MESH").GetComponent<MakeMesh>().GetTriangleVertices((int)stlTimeSlider - 1);
                var p1 = p[0];
                var p2 = p[1];
                var p3 = p[2];
                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Entity </color></size>");
                GUILayout.Label("<size=10><color=black> X </color></size>");
                GUILayout.Label("<size=10><color=black> Y </color></size>");
                GUILayout.Label("<size=10><color=black> Z </color></size>");
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 1</color></size>");
                GUILayout.TextField(p1.x.ToString("f3"));
                GUILayout.TextField(p1.y.ToString("f3"));
                GUILayout.TextField(p1.z.ToString("f3"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 2</color></size>");
                GUILayout.TextField(p2.x.ToString("f3"));
                GUILayout.TextField(p2.y.ToString("f3"));
                GUILayout.TextField(p2.z.ToString("f3"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 3</color></size>");
                GUILayout.TextField(p3.x.ToString("f3"));
                GUILayout.TextField(p3.y.ToString("f3"));
                GUILayout.TextField(p3.z.ToString("f3"));
                GUILayout.EndVertical();
                break;
            #endregion
            case "DMC":
                var line = LoadFile.dmcLines[(int)dmcTimeSlider];
                var endpoint1 = line.p1;
                var endpoint2 = line.p2;
                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Entity </color></size>");
                GUILayout.Label("<size=10><color=black> X </color></size>");
                GUILayout.Label("<size=10><color=black> Y </color></size>");
                GUILayout.Label("<size=10><color=black> Z </color></size>");
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 1</color></size>");
                GUILayout.TextField(endpoint1.x.ToString("f3"));
                GUILayout.TextField(endpoint1.y.ToString("f3"));
                GUILayout.TextField(endpoint1.z.ToString("f3"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 2</color></size>");
                GUILayout.TextField(endpoint2.x.ToString("f3"));
                GUILayout.TextField(endpoint2.y.ToString("f3"));
                GUILayout.TextField(endpoint2.z.ToString("f3"));
                GUILayout.EndVertical();
                
                break;
            default:
                break;
        }
        GUILayout.EndHorizontal();
    }
    void Update()
    {
        if (!LoadFile.dmcCodeLoaded && !LoadFile.stlCodeLoaded) return;
        switch (GetAvailableToggles()[typeInt])
        {
            case "STL":
                if (Input.GetKeyDown(KeyCode.UpArrow) && stlTimeSlider < GameObject.Find("MESH").GetComponent<MakeMesh>().GetMesh().vertices.Length / 3 - 1)
                    stlTimeSlider++;
                if (Input.GetKeyDown(KeyCode.DownArrow) && stlTimeSlider > 1)
                    stlTimeSlider--;
                break;
            case "DMC":
                if (Input.GetKeyDown(KeyCode.UpArrow) && dmcTimeSlider < LoadFile.dmcLines.Count - 1)
                    dmcTimeSlider++;
                if (Input.GetKeyDown(KeyCode.DownArrow) && dmcTimeSlider > 1)
                    dmcTimeSlider--;
                break;
            default:
                break;
        }
    }
}

