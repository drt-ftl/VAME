using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LoadFile : MonoBehaviour 
{
    #region declarations
    public Material graphMaterial;
    public Color gridColor;
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
    public static Dictionary <int, int> model_code_xrefDMC = new Dictionary<int, int>();
    public static Dictionary<int, int> model_code_xrefJOB = new Dictionary<int, int>();
    public static Dictionary<int, int> model_code_xrefGCD = new Dictionary<int, int>();
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
	public static Transform stlHolder;
	private Transform dmcHolder;
	private Transform jobHolder;
    private Transform gcdHolder;
    private enum Type {STL,DMC,JOB,GCD,AMF,CS}
	private Type type;
	public static float stlScale = 1f;
	private float dmcScale = 25/2540000f;
	private float jobScale = 1f;

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
                            if (InspectorR.voxelsLoaded)
                            {
                                foreach (var v in MeshVoxelizer.voxels)
                                {
                                    v.Value.IntersectedByLines.Clear();
                                }
                                GameObject.Find("VOXELIZER").GetComponent<PathFitter>().FitPaths();
                            }
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
					    default:
						    break;
					}
                    var camPos = Camera.main.transform.position;
                    camPos.z = Min.z * 3.0f;
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
        if (gcdCodeLoaded)
            DrawPaths(Type.GCD);
        if (jobCodeLoaded)
            DrawPaths(Type.JOB);
        if (dmcCodeLoaded)
            DrawPaths(Type.DMC);
    }

    private void DrawCS()
    {
        if (!cSectionGCD.ready) return;
        foreach (var cs in cSectionGCD.csLines)
        {
            int low = 0;
            int high = 0;
            var col = new Color(0f, 0f, 1f, 1f);
            switch (cSectionGCD.csMode)
            {
                case cSectionGCD.CsMode.StepThrough:
                    if (cSectionGCD.layerHeights.IndexOf(cs.Key) != InspectorT.slicerForm.trackBar1.Value) col.a = (float)InspectorT.slicerForm.transparency.Value / 100f;
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
            
            foreach (var line in cs.Value)
            {
                switch (cSectionGCD.csMode)
                {
                    case cSectionGCD.CsMode.StepThrough:
                        break;
                    case cSectionGCD.CsMode.ByGcdCode:
                        if (cSectionGCD.layerHeights.IndexOf(cs.Key) <= low
                        || cSectionGCD.layerHeights.IndexOf(cs.Key) > high)
                            col.a = (float)InspectorT.slicerForm.transparency.Value / 100f;
                        else col.a = 1.0f;
                        break;
                    case cSectionGCD.CsMode.WallThickness:
                        var minWT = InspectorT.slicerForm.trackBar1.Value / 100f;
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
        GUI.Label(new Rect(250, 100, 100, 300), "");
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

	private void Draw(Type _type)
	{
		foreach (var Vertex1 in vertices)
		{
			var newSegment = new LineSegment();
			var index = vertices.IndexOf(Vertex1);
			if (index <= 0)
				continue;
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
                newSegment.step = gcdLines.Count;
                gcdLines.Add(newSegment);
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
                        Camera.main.GetComponent<InspectorR>().OnVoxelize();
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
}
