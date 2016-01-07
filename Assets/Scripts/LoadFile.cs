using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCAT_Explorer;


public class LoadFile : MonoBehaviour 
{
    #region declarations
    public Material graphMaterial;
    public Color gridColor;
    public GameObject cctPoint;
    public static Vector3 Min = new Vector3(1000,1000,1000);
    public static Vector3 Max = new Vector3(-1000, -1000, -1000);
    JobInterpreter jobInterpreter = new JobInterpreter();
	public static GcdInterpreter gcdInterpreter = new GcdInterpreter();
	DmcInterpreter dmcInterpreter = new DmcInterpreter();
	public static StlInterpreter stlInterpreter = new StlInterpreter();
	public Material mat;
	public Material stlMat;
	public LineRenderer lineRenderer;
	public static bool loading = false;
	private bool drawn = true;
	private int vertexCount = 0;
	public static List <Vector3> currentVertices = new List<Vector3>();
	public GameObject triangleObject;
	[HideInInspector]
	public List<Vector3> vertices = new List<Vector3> ();
	public static List <LineSegment> dmcLines = new List<LineSegment>();
	public static List <LineSegment> jobLines = new List<LineSegment>();
    public static List<LineSegment> gcdLines = new List<LineSegment>();
    public static List<LineSegment> cctLines = new List<LineSegment>();
    public static Dictionary <int, int> model_code_xrefDMC = new Dictionary<int, int>();
    public static Dictionary<int, int> model_code_xrefJOB = new Dictionary<int, int>();
    public static Dictionary<int, int> model_code_xrefGCD = new Dictionary<int, int>();
    public static Dictionary<int, bool> LaserOnModelRef = new Dictionary<int, bool>();
    public static List<int> model_code_xrefSTL = new List<int>();
    public static List<string> jobCode = new List<string>();
    public static List<string> gcdCode = new List<string>();
	public static List<string> dmcCode = new List<string>();
    public static int firstGcdLineInCode = 0;
    public static int firstJobLineInCode = 0;
    public static int firstDmcLineInCode = 0;
	public static int firstStlLineInCode = 0;
	public static List<string> stlCode = new List<string>();
    public static bool gcdCodeLoaded = false;
    public static bool jobCodeLoaded = false;
	public static bool dmcCodeLoaded = false;
	public static bool stlCodeLoaded = false;
    public static bool cCatCodeLoaded = false;
    public static Transform stlHolder;
	private Transform dmcHolder;
	private Transform jobHolder;
    private Transform gcdHolder;
    private enum Type {STL,DMC,JOB,GCD,AMF,CS,CCT}
	private Type type;
	public static float stlScale = 1f;
	private float dmcScale = 25/2540000f;
	private float jobScale = 1f;
    public static float runTime = 0;
    public static float playbackTime = 0;
    public static float playbackStartTime;
    public static float maxPlaybackTime = 0;
    public float ips = 1;
    public static bool playback = false;
    public static List<CCATpoint> cctVerts = new List<CCATpoint>();
    Vector3 cctMin = Vector3.one * 10000;
    Vector3 cctMax = Vector3.one * -10000;
    Vector3 cctCentroid = Vector3.zero;
    public static int cctHighlight = -1;
    public static List<CCATpoint> gcdPointVerts = new List<CCATpoint>();
    public static List<Vector3> gcdVerts = new List<Vector3>();
    private float gcdPathDistance = 0;
    private float cctRes = 1.0f;
    private int cctPointCount = 0;
    private float totalCctPathDistance = 0f;

    private GUIStyle windowStyle = new GUIStyle ();
	[DllImport("user32.dll")]
	private static extern void OpenFileDialog();
	[DllImport("user32.dll")]
	private static extern void ShowDialog ();
    [DllImport("user32.dll")]
    private static extern void SaveFileDialog();
    public GUISkin inspectorSkin;
	public GUIStyle myStyle;
    public Transform target;
    MakeMesh MM;
    public GUISkin restoreSkin;
    public Color LineHighlight;
    public static CCAT_E ccatExplorer;
    public static float cctVis = 100;
    public static float cctErrorThreshold = 0.01f;
    #endregion

    public void Start()
	{
        dmcHolder = GameObject.Find ("dmcHolder").transform;
		stlHolder = GameObject.Find ("stlHolder").transform;
		jobHolder = GameObject.Find ("jobHolder").transform;
        gcdHolder = GameObject.Find("gcdHolder").transform;
        var tbSpace = 20;
        var tbWidth = InspectorT.TopToolbarStrings.Length * 120;
        GetComponent<InspectorT>().MainRect = new Rect(2, 0, tbWidth, 80);
        GetComponent<InspectorR>().MainRect = new Rect(Screen.width - 255, tbSpace, 250, 570);
        GetComponent<InspectorL>().MainRect = new Rect(5, tbSpace, 250, 570);
        windowStyle.fontSize = 50;
        MM = GameObject.Find("MESH").GetComponent<MakeMesh>();
        MM.material = stlMat;
        MM.Begin();
    }

    void OnApplicationQuit()
    {
        if (InspectorT.slicerForm != null)
            InspectorT.slicerForm.Close();
        if (LoadFile.ccatExplorer != null)
            LoadFile.ccatExplorer.Close();
    }
	public void loadFile()
	{
		loading = true;
        vertices.Clear();
		System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog ();
		openFileDialog.InitialDirectory = Application.dataPath + "/Samples";
        var sel = "";
        sel += "VAME Files (*.vme)|*.vme|";
        if (!stlCodeLoaded)
            sel += "STL Files (*.STL)|*.STL|";
        if (!gcdCodeLoaded)
            sel += "GCD Files (*.gcd)|*.gcd|";
        if (!dmcCodeLoaded)
            sel += "DMC Files (*.DMC)|*.DMC|";
        if (!jobCodeLoaded)
            sel += "JOB Files (*.JOB)|*.JOB|";
        if (!cCatCodeLoaded)
            sel += "CCAT Files (*.cct)|*.cct|";
        sel = sel.TrimEnd('|');
        openFileDialog.Filter = sel;
        openFileDialog.FilterIndex = 1;
		openFileDialog.RestoreDirectory = false;

		if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			try
			{
				var fileName = openFileDialog.FileName;
                if (fileName.EndsWith("amf"))
                    type = Type.AMF;
				if (fileName.EndsWith("JOB"))
					type = Type.JOB;
				else if (fileName.EndsWith("STL"))
					type = Type.STL; 
				else if (fileName.EndsWith("DMC"))
					type = Type.DMC;
                else if (fileName.EndsWith("gcd"))
                    type = Type.GCD;
                if (fileName.EndsWith("cct"))
                    type = Type.CCT;
                if (fileName != null)
				{
					var reader = new StreamReader(fileName);
					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();
                        scanAMF(line);
						switch (type)
						{
                        case Type.GCD:
                            scanGCD(line);
                            break;
                        case Type.JOB:
							scanJOB(line);
							break;
						case Type.DMC:
							scanDMC(line);
							break;
						case Type.STL:
							scanSTL(line);
							break;
                        case Type.CCT:
                            scanCCT(line);
                            break;
                            default:
							break;
						}
					}
                    var il = GetComponent<InspectorL>();
					switch (type)
					{
                        case Type.GCD:
                            Draw(Type.GCD);
                            InspectorL.gcdTimeSlider = gcdLines.Count - 1;
                            InspectorL.gcdTimeSliderMin = 0;
                            InspectorL.gcdVisSlider = 1;
                            InspectorL.lastLoaded = InspectorL.LastLoaded.GCD;
                            gcdCodeLoaded = true;
                            break;
                        case Type.JOB:
						    Draw (Type.JOB);
                            InspectorL.jobTimeSlider = jobLines.Count - 1;
                            InspectorL.jobTimeSliderMin = 0;
                            InspectorL.jobVisSlider = 1;
                            InspectorL.lastLoaded = InspectorL.LastLoaded.JOB;
                            jobCodeLoaded = true;
						    break;
					    case Type.DMC:
						    Draw (Type.DMC);
                            InspectorL.dmcTimeSlider = dmcLines.Count - 1;
                            InspectorL.dmcTimeSliderMin = 0;
                            InspectorL.dmcVisSlider = 1;
                            InspectorL.lastLoaded = InspectorL.LastLoaded.DMC;
                            dmcCodeLoaded = true;
						    break;
					    case Type.STL:
                            InspectorL.lastLoaded = InspectorL.LastLoaded.STL;
                            GameObject.Find("MESH").GetComponent<MakeMesh>().MergeMesh();
                            InspectorL.stlTimeSlider = MM.GetMesh().vertices.Length / 3 - 1;
                            InspectorL.stlTimeSliderMin = 0;
                            InspectorL.stlVisSlider = 1;
                            stlCodeLoaded = true;
                            InspectorManager.VoxelManager = true;
                            break;
                        case Type.CCT:
                            //DrawCCT();
                            GenerateCctObjects();
                            //InspectorL.dmcTimeSlider = dmcLines.Count - 1;
                            //InspectorL.dmcTimeSliderMin = 0;
                            //InspectorL.dmcVisSlider = 1;
                            //InspectorL.lastLoaded = InspectorL.LastLoaded.DMC;
                            Min.z = cctMin.z;
                            cCatCodeLoaded = true;
                            break;
                        default:
						    break;
					}
                    var camPos = Camera.main.transform.position;
                    camPos.z = Min.z * 8.0f;
                    Camera.main.transform.position = camPos;
					Camera.main.GetComponent<CameraControl>().target.position = Vector3.zero;
					transform.LookAt(Camera.main.GetComponent<CameraControl>().target);
					loading = false;
					drawn = false;
				}
			}
			catch {}
		}
	}

	void OnGUI()
	{
        GUI.skin = inspectorSkin;
        GUI.skin.scrollView.normal.background = restoreSkin.scrollView.normal.background;
        
        var groupRectL = GetComponent<InspectorL>().MainRect;
        GUI.Window(0, groupRectL, GetComponent<InspectorL>().InspectorWindowL, "Inspector");
        if (InspectorManager.VoxelManager)
        {
            var groupRectR = GetComponent<InspectorR>().MainRect;
            GUI.Window(1, groupRectR, GetComponent<InspectorR>().InspectorWindowR, "Voxel Manager");
        }
        var groupRectT = GetComponent<InspectorT>().MainRect;
        GUI.Window(2, groupRectT, GetComponent<InspectorT>().InspectorWindowT, "", "Toolbar");
        GUI.BringWindowToFront(2);
        DrawCS();
        DrawCSX();
        DrawCSZ();
        if (gcdCodeLoaded)
        {
            if (!playback)
                DrawPaths(Type.GCD);
            else
                PlaybackGCD();
        }
        if (jobCodeLoaded)
            DrawPaths(Type.JOB);
        if (dmcCodeLoaded)
            DrawPaths(Type.DMC);
        //if (cCatCodeLoaded)
        //    DrawPaths(Type.CCT);
    }

    private void DrawCS()
    {
        if (!cSectionGCD.ready) return;
        foreach (var cs in cSectionGCD.layers)
        {
            int low = 0;
            int high = 0;
            var col = new Color(0f, 0f, 1f, 1f);
            switch (cSectionGCD.csMode)
            {
                case cSectionGCD.CsMode.StepThrough:
                    //if (cSectionGCD.layerHeights.IndexOf(cs.Key) != InspectorT.slicerForm.LayerTrackbar.Value) col.a = (float)InspectorT.slicerForm.transparency.Value / 100f;
                    break;
                case cSectionGCD.CsMode.ByGcdCode:
                    low = cSectionGCD.layerHeights.IndexOf(gcdLines[(int)InspectorL.gcdTimeSliderMin].p1.y);
                    high = cSectionGCD.layerHeights.IndexOf(gcdLines[(int)InspectorL.gcdTimeSlider].p1.y);                    
                    break;
                case cSectionGCD.CsMode.WallThickness:
                    break;
                default:
                    break;
            }

            if (InspectorT.slicerForm.ShowCsection.Checked)
            {
                foreach (var line in cs.Value.border)
                {
                    switch (cSectionGCD.csMode)
                    {
                        case cSectionGCD.CsMode.StepThrough:
                            break;
                        case cSectionGCD.CsMode.ByGcdCode:
                            //if (cSectionGCD.layerHeights.IndexOf(cs.Key) <= low
                            //|| cSectionGCD.layerHeights.IndexOf(cs.Key) > high)
                            //    col.a = (float)InspectorT.slicerForm.transparency.Value / 100f;
                            //else col.a = 1.0f;
                            break;
                        case cSectionGCD.CsMode.WallThickness:
                            var minWT = InspectorT.slicerForm.wtSlider.Value / 100f;
                            if (line.WallThickness < minWT)
                            {
                                col.r = 1.0f - line.WallThickness / minWT;
                                col.b = line.WallThickness / minWT;
                            }
                            break;
                    }
                    var a = line.Endpoint0;
                    var b = line.Endpoint1;
                    graphMaterial.SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Color(col);
                    GL.Vertex(a);
                    GL.Vertex(b);
                    GL.End();
                }
            }
        }
        GUI.Label(new Rect(250, 100, 100, 300), "");
    }

    private void DrawCSX()
    {
        if (!cSectionGCD.ready) return;
        //foreach (var cs in cSectionGCD.layers)
        var layerTrackbar = InspectorT.slicerForm.LayerTrackbar.Value;
        var y = cSectionGCD.layerHeights[layerTrackbar];
        //var y = gcdLines[(int)InspectorL.gcdTimeSlider].p1.y;
        var sloxels = cSectionGCD.layers[y].Sloxels;
        {
            foreach (var slox in sloxels)
            {
                var dim = 1.0f / slox.Dim;
                var a = slox.Position;
                a.x -= dim / 2;
                a.z -= dim / 2;
                var b = a;
                b.x += dim;
                var c = b;
                c.z += dim;
                var d = c;
                d.x -= dim;
                graphMaterial.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Color(gridColor);
                GL.Vertex(a);
                GL.Vertex(b);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(gridColor);
                GL.Vertex(b);
                GL.Vertex(c);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(gridColor);
                GL.Vertex(c);
                GL.Vertex(d);
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(gridColor);
                GL.Vertex(d);
                GL.Vertex(a);
                GL.End();
            }
        }
        GUI.Label(new Rect(250, 100, 100, 300), "");
    }
    private void DrawCSZ()
    {
    }

    private void PlaybackGCD()
    {
        foreach (var line in gcdLines)
        {
            var col = line.LineColor;
            var a = line.p1;
            var b = line.p2;
            playbackTime = UnityEngine.Time.realtimeSinceStartup - playbackStartTime;
            if (line.StartTime > playbackTime)
                continue;
            else if (line.EndTime < playbackTime)
            {
                var dt = playbackTime - line.StartTime;
                var d = dt / (line.EndTime - line.StartTime);
                var m = b - a;
                b = a + m * d;
            }
            if (UnityEngine.Time.realtimeSinceStartup - playbackStartTime >= maxPlaybackTime)
            {
                playbackStartTime = UnityEngine.Time.realtimeSinceStartup;
            }
            graphMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(col);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.End();
        }
    }
    private void DrawPaths(Type _type)
    {
        var tmpList = new List<LineSegment>();
        switch(_type)
        {
            case Type.GCD:
                tmpList = gcdLines;
                break;
            case Type.JOB:
                tmpList = jobLines;
                break;
            case Type.DMC:
                tmpList = dmcLines;
                break;
            default:
                break;
        }
        foreach (var line in tmpList)
        {
            var col = line.LineColor;
            var a = line.p1;
            var b = line.p2;
            graphMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(col);
            GL.Vertex(a);
            GL.Vertex(b);
            GL.End();
        }
    }

    private void GenerateCctObjects()
    {
        ConvertGcdDataToPoints();
           
        cctCentroid = (cctMax - cctMin) / 2f;
        foreach (var gcdV in gcdPointVerts)
        {
            gcdV.Position -= gcdInterpreter.centroid;
        }
        foreach (var v in cctVerts)
        {
            v.Position -= gcdInterpreter.centroid;
            var index = cctVerts.IndexOf(v);
            var Vertex1 = v.Position;
            var newObj = Instantiate(cctPoint, v.Position, Quaternion.identity) as GameObject;
            if (gcdPointVerts.Count > index - 1)
            {
                var delta = (gcdPointVerts[index].Position - v.Position);
                delta *= 1000f;
                var c = 0.75f * (delta + Vector3.one * 0.25f);
                
                var newCol = new Color(c.x, c.y, c.z, 1.0f);
                newObj.GetComponent<cctPointScript>().SetColor(newCol);
            }
            newObj.GetComponent<cctPointScript>().Id = index;
            newObj.GetComponent<cctPointScript>().Temperature = v.Temp;
            newObj.GetComponent<cctPointScript>().Go();
            newObj.transform.SetParent(GameObject.Find("POINTS").transform);
        }
        if (ccatExplorer == null)
        {
            ccatExplorer = new CCAT_E();
            CCAT_E.visChanged += cctVisChanged;
            CCAT_E.thresholdChanged += cctThresholdChanged;
            //SlicerForm.SlicerForm.buttonPressed += ButtonPressed;
        }
        ccatExplorer.Show();
    }

    private void DrawCCT()
    {
        cctCentroid = (cctMax - cctMin) / 2f;
        foreach (var v in cctVerts)
        {
            var index = cctVerts.IndexOf(v);
            if (index == 0) continue;
            var Vertex1 = v.Position;
            var Vertex0 = cctVerts[index - 1].Position;
            var newSegment = new LineSegment();
            newSegment.LineColor = new Color(0f, 0f, 1f, 1f);
            newSegment.p1 = Vertex0 - cctCentroid;
            newSegment.p2 = Vertex1 - cctCentroid;
            newSegment.step = v.Id - 1;
            cctLines.Add(newSegment);
        }
    }
	private void Draw(Type _type)
	{
		foreach (var Vertex1 in vertices)
		{
			var newSegment = new LineSegment();
			var index = vertices.IndexOf(Vertex1);
			if (index <= 0)
				continue;
            if (!LaserOnModelRef[index]) continue;
            var Vertex0 = vertices[index - 1];
			if (_type == Type.JOB)
			{
                newSegment.LineColor = new Color(0f, 0f, 1f, 1f);
				newSegment.p1 = Vertex0 - jobInterpreter.centroid;
				newSegment.p2 = Vertex1 - jobInterpreter.centroid;
                newSegment.step = jobLines.Count;
                jobLines.Add (newSegment);
			}
            else if (_type == Type.GCD)
            {
                newSegment.LineColor = new Color(0f, 0f, 1f, 1f);
                newSegment.p1 = Vertex0 - gcdInterpreter.centroid;
                newSegment.p2 = Vertex1 - gcdInterpreter.centroid;
                var d = Vector3.Magnitude(Vertex1 - Vertex0);
                var t = d / ips;
                newSegment.StartTime = runTime;
                runTime += t;
                newSegment.EndTime = runTime;
                newSegment.step = gcdLines.Count;
                gcdLines.Add(newSegment);
                if (runTime > maxPlaybackTime)
                    maxPlaybackTime = runTime;
            }
            else if (_type == Type.DMC)
			{
                newSegment.LineColor = new Color(0f, 0f, 1f, 1f);
                newSegment.p1 = Vertex0 - dmcInterpreter.centroid;
                newSegment.p2 = Vertex1 - dmcInterpreter.centroid;
                newSegment.step = dmcLines.Count;
                dmcLines.Add(newSegment);
            }
		}
	}

	void scanJOB(string _line)
	{
        if (_line == "\r\n") return;
		_line = _line.Trim();
		jobCode.Add (_line.ToString () + "\r\n");
        if (!_line.Contains(' ')) return;
        var chunks = _line.Split(' ');
        switch (chunks[0][0])
		{
		case 'G':
            if (!chunks[1].StartsWith("F"))
            {
                jobInterpreter.StartsWithG(_line);
            }
			break;
		case '*':
			jobInterpreter.StartsWithStar(_line);
			break;
		default:
			break;
		}
	}

    void scanGCD(string _line)
    {
        if (_line == "\r\n") return;
        _line = _line.Trim();
        gcdCode.Add(_line.ToString() + "\r\n");
        if (!_line.Contains(' ')) return;
        var chunks = _line.Split(' ');
        switch (chunks[0][0])
        {
            case 'G':
                if (!chunks[1].StartsWith("F"))
                {
                    gcdInterpreter.StartsWithG(_line);
                }
                break;
            case 'O':
                if (chunks[0][1] == 'U')
                {
                    if (chunks.Length > 1 && chunks[1].Contains(","))
                    {
                        var command = chunks[1].Split(',');
                        if (command[1] == "0")
                            gcdInterpreter.LaserOn = false;
                        else gcdInterpreter.LaserOn = true;
                    }
                }
                //    gcdInterpreter.StartsWithO(_line);
                break;
            default:
                break;
        }
    }

    void scanDMC(string _line)
    {
        if (_line == "\r\n") return;
        _line = _line.Trim();
		dmcCode.Add (_line.ToString () + "\r\n");
        //dmcCodeLong += _line.ToString() + "      " + "\r\n";
        var chunks = _line.Split(' ');
		switch (chunks[0])
		{
		    case "PA":
			    dmcInterpreter.StartsWithPA(_line, dmcScale);
			    break;
            case "G1":
                dmcInterpreter.StartsWithPA(_line, dmcScale);
                break;
            case "REM":
			dmcInterpreter.StartsWithREM(_line);
			break;
		default:
			break;
		}
	}

	void scanSTL(string _line)
    {
        //if (_line == "\r\n") return;
        _line = _line.Trim();
		stlCode.Add (_line.ToString () + "\r\n");
        var chunks = _line.Split(' ');
		if (_line.Contains ("outer")) 
		{
			currentVertices.Clear();
            stlInterpreter.outerloop();
        }
		else if (_line.Contains("endloop"))
		{
			stlInterpreter.endloop(_line, this);
		}
		else if (_line.Contains("vertex"))
		{
			stlInterpreter.vertex(_line);
		}

        else if (_line.Contains("normal"))
        {
            stlInterpreter.normal(_line);
        }
    }

    void scanAMF(string _line)
    {
        {
            _line = _line.Trim();
            if (_line.StartsWith("|||ftlVAME"))
            {
                var chunks = _line.Split(' ');
                #region STL
                if (chunks[1] == "STL")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        vertices.Clear();
                        stlInterpreter = new StlInterpreter();
                        type = Type.STL;
                    }
                    else
                    {
                        InspectorL.lastLoaded = InspectorL.LastLoaded.STL;
                        GameObject.Find("MESH").GetComponent<MakeMesh>().MergeMesh();
                        InspectorL.stlTimeSlider = MM.GetMesh().vertices.Length / 3 - 1;
                        InspectorL.stlTimeSliderMin = 0;
                        InspectorL.stlVisSlider = 1;
                        stlCodeLoaded = true;
                        InspectorManager.VoxelManager = true;
                        type = Type.AMF;
                    }
                }
                #endregion
                #region GCD
                if (chunks[1] == "GCD")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        gcdInterpreter = new GcdInterpreter();
                        vertices.Clear();
                        type = Type.GCD;
                    }
                    else
                    {
                        Draw(Type.GCD);
                        InspectorL.gcdTimeSlider = gcdLines.Count - 1;
                        InspectorL.gcdTimeSliderMin = 0;
                        InspectorL.gcdVisSlider = 1;
                        InspectorL.lastLoaded = InspectorL.LastLoaded.GCD;
                        gcdCodeLoaded = true;
                        type = Type.AMF;
                    }
                }
                #endregion
                #region DMC
                if (chunks[1] == "DMC")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        dmcInterpreter = new DmcInterpreter();
                        vertices.Clear();
                        type = Type.DMC;
                    }
                    else
                    {
                        Draw(Type.DMC);
                        InspectorL.dmcTimeSlider = dmcLines.Count - 1;
                        InspectorL.dmcTimeSliderMin = 0;
                        InspectorL.dmcVisSlider = 1;
                        InspectorL.lastLoaded = InspectorL.LastLoaded.DMC;
                        dmcCodeLoaded = true;
                        type = Type.AMF;
                    }
                }
                #endregion
                #region JOB
                if (chunks[1] == "JOB")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        jobInterpreter = new JobInterpreter();
                        vertices.Clear();
                        type = Type.JOB;
                    }
                    else
                    {
                        Draw(Type.JOB);
                        InspectorL.jobTimeSlider = jobLines.Count - 1;
                        InspectorL.jobTimeSliderMin = 0;
                        InspectorL.jobVisSlider = 1;
                        InspectorL.lastLoaded = InspectorL.LastLoaded.JOB;
                        jobCodeLoaded = true;
                        type = Type.AMF;
                    }
                }
                #endregion
                #region voxels
                if (chunks[1] == "Voxels")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        vertices.Clear();
                        float.TryParse(chunks[3], out InspectorR.voxelVis);
                        float.TryParse(chunks[4], out InspectorR.resolution);
                    }
                    else
                    {
                        //Camera.main.GetComponent<InspectorR>().OnVoxelize();
                        type = Type.AMF;
                    }
                }
                #endregion
                #region Setup
                if (chunks[1] == "AMF")
                {
                    if (chunks[2] == "BEGIN")
                    {
                        Camera.main.GetComponent<InspectorT>().Restart();
                        //var savedType = "";
                        //var sts
                        //float.TryParse(chunks[3], out InspectorR.voxelVis);
                        //float.TryParse(chunks[4], out InspectorR.resolution);
                    }
                    if (chunks[2] == "END")
                    {
                        //var savedType = "";
                        //var sts
                        //float.TryParse(chunks[3], out InspectorR.voxelVis);
                        //float.TryParse(chunks[4], out InspectorR.resolution);
                    }
                }
                #endregion
            }
        }
    }

    void scanCCT(string _line)
    {
        if (_line == "\r\n" || _line.Contains('X')) return;
        if (_line.StartsWith("STEPSIZE:"))
        {
            var ssText = _line.Split(':');
            float res;
            if (float.TryParse(ssText[1], out res))
                cctRes = res;
            return;
        }
        else if (_line.StartsWith("DISTANCE:"))
        {
            var ssText = _line.Split(':');
            float dist;
            if (float.TryParse(ssText[1], out dist))
                totalCctPathDistance = dist;
            return;
        }
        else if (_line.StartsWith("POINTS:"))
        {
            var ssText = _line.Split(':');
            int count;
            if (Int32.TryParse(ssText[1], out count))
                cctPointCount = count;
            return;
        }
        _line = _line.Trim();
        var chunks = _line.Split(',');
        var initial = 999999999f;
        var vertex = Vector3.one * initial;
        float x;
        float y;
        float z;
        float temperature;
        if (float.TryParse(chunks[0], out x))
        {
            vertex.x = x;
        }
        if (float.TryParse(chunks[2], out y))
        {
            vertex.y = y;
        }
        if (float.TryParse(chunks[1], out z))
        {
            vertex.z = z;
        }
        if (float.TryParse(chunks[3], out temperature))
        {
            if (vertex.x != initial && vertex.y != initial && vertex.z != initial)
            {
                var newPoint = new CCATpoint();
                newPoint.Position = vertex;
                if (vertex.x < cctMin.x)
                    cctMin.x = vertex.x;
                if (vertex.x > cctMax.x)
                    cctMax.x = vertex.x;
                if (vertex.y < cctMin.y)
                    cctMin.y = vertex.y;
                if (vertex.y > cctMax.y)
                    cctMax.y = vertex.y;
                if (vertex.z < cctMin.z)
                    cctMin.z = vertex.z;
                if (vertex.z > cctMax.z)
                    cctMax.z = vertex.z;
                
                newPoint.Temp = temperature;
                newPoint.Id = cctVerts.Count;
                cctVerts.Add(newPoint);
            }
        }
    }

    void ConvertGcdDataToPoints()
    {
        var partialDistance = 0f;
        foreach (var v1 in gcdVerts)
        {
            var index = gcdVerts.IndexOf(v1);
            if (index == 0) continue;
            var v0 = gcdVerts[index - 1];
            var d = Vector3.Distance(v0, v1);
            var rem = cctRes - partialDistance;
            if (d < rem)
            {
                gcdPathDistance += d;
                partialDistance += d;
                continue;
            }
            var t = (rem / d);
            var m = v1 - v0;
            var p = new Vector3();
            p.x = v0.x + m.x * t;
            p.y = v0.y + m.y * t;
            p.z = v0.z + m.z * t;
            var newPoint = new CCATpoint();
            newPoint.Id = gcdPointVerts.Count;
            newPoint.Position = p;
            gcdPointVerts.Add(newPoint);
            while ((t + cctRes / d) < 1)
            {
                t += (cctRes / d);
                p.x = v0.x + m.x * t;
                p.y = v0.y + m.y * t;
                p.z = v0.z + m.z * t;
                var _newPoint = new CCATpoint();
                _newPoint.Id = gcdPointVerts.Count;
                _newPoint.Position = p;
                gcdPointVerts.Add(_newPoint);
                gcdPathDistance += cctRes;
            }
            partialDistance = (1 - t) * d;
            gcdPathDistance += partialDistance;
        }
    }

    private void cctVisChanged (float _vis)
    {
        cctVis = _vis;
    }

    private void cctThresholdChanged(float _t)
    {
        cctErrorThreshold = _t;
    }
}
