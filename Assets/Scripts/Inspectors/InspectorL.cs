using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

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
    public static float gcdVisSlider = 1;
    public static float stlTimeSlider = 1;
    public static float stlTimeSliderPrev = 1;
    public static float stlTimeSliderMin = 1;
    public static float dmcTimeSlider = 1;
    public static float dmcTimeSliderPrev = 1;
    public static float dmcTimeSliderMin = 1;
    public static float dmcCodeSlider = 1;
    public static float jobTimeSlider = 1;
    public static float jobTimeSliderMin = 1;
    public static float jobTimeSliderPrev = 1;
    public static float gcdTimeSlider = 1;
    public static float gcdTimeSliderMin = 1;
    public static float gcdTimeSliderPrev = 1;
    private int typeInt = 0;
    private string[] toolbarStrings = { "STL", "DMC", "JOB" };
    public static Color stlColor = new Color(1.0f, 0.5f, 0.5f, 1f);
    public Color dmcLineColor = new Color(0f, 0.7f, 0f, 1f);
    public Color jobLineColor = new Color(1f, 0f, 0f, 1f);
    public Color gcdLineColor = new Color(1f, 0f, 0f, 1f);
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
    public enum LastLoaded { STL,DMC,JOB,GCD,None};
    public static LastLoaded lastLoaded;
    private float voxelVis;
    int max = 50;
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

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
            if ((!LoadFile.stlCodeLoaded || !LoadFile.dmcCodeLoaded || !LoadFile.jobCodeLoaded || !LoadFile.gcdCodeLoaded))
            {
                if (GUILayout.Button("Load"))
                {
                    //MessageBox(new IntPtr(0), "Hello World!", "Hello Dialog", 3);
                    
                    LoadFile.loading = true;
                    GetComponent<LoadFile>().loadFile();
                }
            }

            if (availableToggles != null)
            {
                // Toggle File Type
                typeInt = GUILayout.Toolbar(typeInt, availableToggles.ToArray(), GUILayout.Width(220));

                // Visibility
                VisibiltySlider();
                GUILayout.Space(18);

                //Code Panel
                if (GetCode() != null)
                {
                    GUILayout.Box("<color=lime>" + GetCode()[0] + "</color>" + GetCode()[1], GUILayout.Width(225), GUILayout.Height(180));
                    GUILayout.Space(10);
                }

                // Time Sliders
                TimeSliders();

                //Coordinates
                CoordinateBoxes();

                // Clear
                GUILayout.Space(8);
                switch (GetAvailableToggles()[typeInt])
                {
                    case "STL":
                        if (GUILayout.Button("Clear STL"))
                        {
                            ClearSTL(0);
                            ClearSTL(1);
                        }
                        break;
                    case "DMC":
                        if (GUILayout.Button("Clear DMC"))
                        {
                            ClearDMC();
                        }
                        break;
                    case "JOB":
                        if (GUILayout.Button("Clear JOB"))
                        {
                            ClearJOB();
                        }
                        break;
                    case "GCD":
                        if (GUILayout.Button("Clear GCD"))
                        {
                            ClearGCD();
                        }
                        break;
                    default:
                        break;
                }
            }
            // Quit
            if (GUILayout.Button("Quit"))
            {
                Application.Quit();
            }
        }
        GUILayout.EndArea();
    }
    public void VisibiltySlider()
    {
        if (GetAvailableToggles() == null) return;
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
            case "JOB":
                jobVisSlider = GUILayout.HorizontalSlider(jobVisSlider, 0, 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Visibility: " + (jobVisSlider * 100).ToString("f0"), KeyStyle, GUILayout.Width(250));
                foreach (var line in LoadFile.jobLines)
                {
                    if (line.Line != null)
                    {
                        jobLineColor.a = jobVisSlider * jobVisSlider;
                        line.LineColor = jobLineColor;
                    }
                }
                break;
            case "GCD":
                gcdVisSlider = GUILayout.HorizontalSlider(gcdVisSlider, 0, 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.Label("Visibility: " + (gcdVisSlider * 100).ToString("f0"), KeyStyle, GUILayout.Width(250));
                foreach (var line in LoadFile.gcdLines)
                {
                    if (line.Line != null)
                    {
                        gcdLineColor.a = gcdVisSlider * gcdVisSlider;
                        line.LineColor = gcdLineColor;
                    }
                }
                break;
            default:
                break;
        }
    }
    public void TimeSliders()
    {
        if (GetAvailableToggles() == null) return;
        switch (GetAvailableToggles()[typeInt])
        {
            case "STL":
                stlTimeSlider = GUILayout.HorizontalSlider(stlTimeSlider, 1, GameObject.Find("MESH").GetComponent<MakeMesh>().GetMesh().vertices.Length / 3 - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Max): " + stlTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var stlCodeNumber = LoadFile.model_code_xrefSTL[(int)stlTimeSlider];
                    GUILayout.Label("Code Line #: " + stlCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();
                stlTimeSliderMin = GUILayout.HorizontalSlider(stlTimeSliderMin, 0, stlTimeSlider, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Min): " + stlTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var stlCodeNumber = LoadFile.model_code_xrefSTL[(int)stlTimeSliderMin];
                    GUILayout.Label("Code Line #: " + stlCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();
                if (stlTimeSliderMin >= stlTimeSlider && stlTimeSlider >= 0)
                    stlTimeSliderMin = stlTimeSlider;
                GUILayout.Label(scrollPosition.y.ToString(), KeyStyle, GUILayout.Width(250));
                if (stlTimeSlider != stlTimeSliderPrev)
                    scrollPosition.y = 0;
                stlTimeSliderPrev = stlTimeSlider;
                break;
            case "DMC":
                dmcTimeSlider = GUILayout.HorizontalSlider(dmcTimeSlider, 1, LoadFile.dmcLines.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Max): " + dmcTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var dmcCodeNumber = LoadFile.model_code_xrefDMC[(int)dmcTimeSlider];
                    GUILayout.Label("Code Line #: " + dmcCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();
                if (dmcTimeSliderMin >= dmcTimeSlider && dmcTimeSlider >= 0)
                    dmcTimeSliderMin = dmcTimeSlider;
                dmcTimeSliderMin = GUILayout.HorizontalSlider(dmcTimeSliderMin, 0, dmcTimeSlider - 2, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Min): " + dmcTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var dmcCodeNumber = LoadFile.model_code_xrefDMC[(int)dmcTimeSliderMin];
                    GUILayout.Label("Code Line #: " + dmcCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();

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
                            line.LineColor = Camera.main.GetComponent<LoadFile>().LineHighlight;
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
            case "JOB":
                jobTimeSlider = GUILayout.HorizontalSlider(jobTimeSlider, 1, LoadFile.jobLines.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Max): " + jobTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var jobCodeNumber = LoadFile.model_code_xrefJOB[(int)jobTimeSlider];
                    GUILayout.Label("Code Line #: " + jobCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();
                if (jobTimeSliderMin >= jobTimeSlider && jobTimeSlider >= 0)
                    jobTimeSliderMin = jobTimeSlider;
                jobTimeSliderMin = GUILayout.HorizontalSlider(jobTimeSliderMin, 0, jobTimeSlider, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Min): " + jobTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var jobCodeNumber = LoadFile.model_code_xrefJOB[(int)jobTimeSliderMin];
                    GUILayout.Label("Code Line #: " + jobCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();

                GUILayout.Label(LoadFile.jobCode.Count.ToString(), KeyStyle, GUILayout.Width(250));
                if (jobTimeSlider != jobTimeSliderPrev)
                    scrollPosition.y = 0;
                jobTimeSliderPrev = jobTimeSlider;

                var indexJOB = 0;
                foreach (var line in LoadFile.jobLines)
                {
                    if (line.Line != null)
                    {
                        if ((int)jobTimeSlider < indexJOB || (int)jobTimeSliderMin > indexJOB)
                        {
                            line.LineColor = new Color(jobLineColor.r, jobLineColor.g, jobLineColor.b, 0f);
                        }
                        else if ((int)jobTimeSlider == indexJOB)
                        {
                            line.LineWidth = 0.01f;
                            line.LineColor = Camera.main.GetComponent<LoadFile>().LineHighlight;
                            _p1 = line.p1;
                            _p2 = line.p2;
                        }
                        else
                        {
                            line.LineWidth = 0.002f;
                            line.LineColor = jobLineColor;
                        }
                    }
                    indexJOB++;
                }
                break;
            case "GCD":
                gcdTimeSlider = GUILayout.HorizontalSlider(gcdTimeSlider, 1, LoadFile.gcdLines.Count - 1, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Max): " + gcdTimeSlider.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var gcdCodeNumber = LoadFile.model_code_xrefGCD[(int)gcdTimeSlider];
                    GUILayout.Label("Code Line #: " + gcdCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();
                if (gcdTimeSliderMin >= gcdTimeSlider && gcdTimeSlider >= 0)
                    gcdTimeSliderMin = gcdTimeSlider;
                gcdTimeSliderMin = GUILayout.HorizontalSlider(gcdTimeSliderMin, 0, gcdTimeSlider, GUILayout.Width(220), GUILayout.Height(12));
                GUILayout.BeginHorizontal(GUILayout.Width(220));
                {
                    GUILayout.Label("Path # (Min): " + gcdTimeSliderMin.ToString("f0"), KeyStyle, GUILayout.Width(110));
                    var gcdCodeNumber = LoadFile.model_code_xrefGCD[(int)gcdTimeSliderMin];
                    GUILayout.Label("Code Line #: " + gcdCodeNumber.ToString(), KeyStyleR, GUILayout.Width(110));
                }
                GUILayout.EndHorizontal();

                GUILayout.Label(LoadFile.gcdCode.Count.ToString(), KeyStyle, GUILayout.Width(250));
                if (gcdTimeSlider != gcdTimeSliderPrev)
                    scrollPosition.y = 0;
                gcdTimeSliderPrev = gcdTimeSlider;

                var indexGCD = 0;
                foreach (var line in LoadFile.gcdLines)
                {
                    if (line.Line != null)
                    {
                        if ((int)gcdTimeSlider < indexGCD || (int)gcdTimeSliderMin > indexGCD)
                        {
                            line.LineColor = new Color(gcdLineColor.r, gcdLineColor.g, gcdLineColor.b, 0f);
                        }
                        else if ((int)gcdTimeSlider == indexGCD)
                        {
                            line.LineWidth = 0.01f;
                            line.LineColor = Camera.main.GetComponent<LoadFile>().LineHighlight;
                            _p1 = line.p1;
                            _p2 = line.p2;
                        }
                        else
                        {
                            line.LineWidth = 0.002f;
                            line.LineColor = gcdLineColor;
                        }
                    }
                    indexGCD++;
                }
                break;
            default:
                break;
        }
    }
    private List<string> GetAvailableToggles ()
    {
        var list = new List<string>();
        if (LoadFile.stlCodeLoaded)
            list.Add("STL");
        if (LoadFile.dmcCodeLoaded)
            list.Add("DMC");
        if (LoadFile.jobCodeLoaded)
            list.Add("JOB");
        if (LoadFile.gcdCodeLoaded)
            list.Add("GCD");
        if (list.Contains(lastLoaded.ToString()))
        {
            typeInt = list.IndexOf(lastLoaded.ToString());
            lastLoaded = LastLoaded.None;
        }
        if (list.Count == 0)
            return null;
        return list;
    }
    private string[] GetCode ()
    {
        var code = new string[2];
        var first = "";
        var bulk = "";
        if (GetAvailableToggles() == null) return null;
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
            case "JOB":
                max = 50;
                if (max > LoadFile.jobCode.Count - 1)
                    max = LoadFile.jobCode.Count - 1;
                if (jobTimeSlider >= LoadFile.firstJobLineInCode)
                {
                    var ts = (int)jobTimeSlider;
                    if (LoadFile.model_code_xrefJOB.Count - 1 < ts )
                        ts = LoadFile.model_code_xrefJOB.Count - 1;
                    var firstLineIndex = LoadFile.model_code_xrefJOB[ts];
                    first = LoadFile.jobCode[firstLineIndex];

                    for (int i = firstLineIndex + 1; i < firstLineIndex + max; i++)
                    {
                        if (LoadFile.jobCode.Count > i)
                            bulk += LoadFile.jobCode[i];
                    }
                }
                else
                {
                    for (int i = (int)jobTimeSlider; i < (int)jobTimeSlider + max; i++)
                    {
                        if (LoadFile.jobCode.Count > i)
                            bulk += LoadFile.jobCode[i];
                    }
                }
                break;
            case "GCD":
                max = 50;
                if (max > LoadFile.gcdCode.Count - 1)
                    max = LoadFile.gcdCode.Count - 1;
                if (gcdTimeSlider >= LoadFile.firstGcdLineInCode)
                {
                    var ts = (int)gcdTimeSlider;
                    if (LoadFile.model_code_xrefGCD.Count - 1 < ts)
                        ts = LoadFile.model_code_xrefGCD.Count - 1;
                    var firstLineIndex = LoadFile.model_code_xrefGCD[ts];
                    first = LoadFile.gcdCode[firstLineIndex];

                    for (int i = firstLineIndex + 1; i < firstLineIndex + max; i++)
                    {
                        if (LoadFile.gcdCode.Count > i)
                            bulk += LoadFile.gcdCode[i];
                    }
                }
                else
                {
                    for (int i = (int)gcdTimeSlider; i < (int)gcdTimeSlider + max; i++)
                    {
                        if (LoadFile.gcdCode.Count > i)
                            bulk += LoadFile.gcdCode[i];
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
        if (GetAvailableToggles() == null) return;
        switch (GetAvailableToggles()[typeInt])
        {
            #region STL
            case "STL":
                var p = GameObject.Find("MESH").GetComponent<MakeMesh>().GetTriangleVertices((int)stlTimeSlider);
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
            case "JOB":
                var _line = LoadFile.jobLines[(int)jobTimeSlider];
                var _endpoint1 = _line.p1;
                var _endpoint2 = _line.p2;
                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Entity </color></size>");
                GUILayout.Label("<size=10><color=black> X </color></size>");
                GUILayout.Label("<size=10><color=black> Y </color></size>");
                GUILayout.Label("<size=10><color=black> Z </color></size>");
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 1</color></size>");
                GUILayout.TextField(_endpoint1.x.ToString("f3"));
                GUILayout.TextField(_endpoint1.y.ToString("f3"));
                GUILayout.TextField(_endpoint1.z.ToString("f3"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 2</color></size>");
                GUILayout.TextField(_endpoint2.x.ToString("f3"));
                GUILayout.TextField(_endpoint2.y.ToString("f3"));
                GUILayout.TextField(_endpoint2.z.ToString("f3"));
                GUILayout.EndVertical();
                break;
            case "GCD":
                var __line = LoadFile.gcdLines[(int)gcdTimeSlider];
                var __endpoint1 = __line.p1;
                var __endpoint2 = __line.p2;
                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Entity </color></size>");
                GUILayout.Label("<size=10><color=black> X </color></size>");
                GUILayout.Label("<size=10><color=black> Y </color></size>");
                GUILayout.Label("<size=10><color=black> Z </color></size>");
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 1</color></size>");
                GUILayout.TextField(__endpoint1.x.ToString("f3"));
                GUILayout.TextField(__endpoint1.y.ToString("f3"));
                GUILayout.TextField(__endpoint1.z.ToString("f3"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("<size=10><color=black> Vertex 2</color></size>");
                GUILayout.TextField(__endpoint2.x.ToString("f3"));
                GUILayout.TextField(__endpoint2.y.ToString("f3"));
                GUILayout.TextField(__endpoint2.z.ToString("f3"));
                GUILayout.EndVertical();
                break;
            default:
                break;
        }
        GUILayout.EndHorizontal();
    }
    void Update()
    {
        if (!LoadFile.dmcCodeLoaded && !LoadFile.stlCodeLoaded && !LoadFile.jobCodeLoaded && !LoadFile.gcdCodeLoaded) return;
        var availableToggles = GetAvailableToggles();
        if (availableToggles == null) return;
        if (availableToggles.Count - 1 < typeInt)
            typeInt = availableToggles.Count - 1;
        switch (availableToggles[typeInt])
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
            case "JOB":
                if (Input.GetKeyDown(KeyCode.UpArrow) && jobTimeSlider < LoadFile.jobLines.Count - 1)
                    jobTimeSlider++;
                if (Input.GetKeyDown(KeyCode.DownArrow) && jobTimeSlider > 1)
                    jobTimeSlider--;
                break;
            case "GCD":
                if (Input.GetKeyDown(KeyCode.UpArrow) && gcdTimeSlider < LoadFile.gcdLines.Count - 1)
                    gcdTimeSlider++;
                if (Input.GetKeyDown(KeyCode.DownArrow) && gcdTimeSlider > 1)
                    gcdTimeSlider--;
                break;
            default:
                break;
        }
    }
}

