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
	GCodeInterpreter gCodeInterpreter = new GCodeInterpreter();
	DmcInterpreter dmcInterpreter = new DmcInterpreter();
	StlInterpreter stlInterpreter = new StlInterpreter();
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
	public static Dictionary <int, int> model_code_xrefDMC = new Dictionary<int, int>();
	public static List<int> model_code_xrefSTL = new List<int>();
	public static List<string> jobCode = new List<string>();
	public static List<string> dmcCode = new List<string>();
	public static int firstDmcLineInCode = 0;
	public static int firstStlLineInCode = 0;
	public static List<string> stlCode = new List<string>();
	public static bool jobCodeLoaded = false;
	public static bool dmcCodeLoaded = false;
	public static bool stlCodeLoaded = false;
	public static Transform stlHolder;
	private Transform dmcHolder;
	private Transform jobHolder;
	private enum Type {STL,DMC,JOB}
	private Type type;
	public static float stlScale = 1f;
	private float dmcScale = 25/2540000f;
	private float jobScale = 1f;
	public static float xMin = 0;
	public static float xMax = 0;
	public static float yMin = 0;
	public static float yMax = 0;
	public static float zMin = 0;
	public static float zMax = 0;
	private GUIStyle windowStyle = new GUIStyle ();
	[DllImport("user32.dll")]
	private static extern void OpenFileDialog();
	[DllImport("user32.dll")]
	private static extern void ShowDialog ();
	public GUISkin inspectorSkin;
	public GUIStyle myStyle;
    public Transform target;
    public static string dmcCodeLong = "";
    public static string stlCodeLong = "";
    public static string jobCodeLong = "";
    MakeMesh MM;

    void Start()
	{
		dmcHolder = GameObject.Find ("dmcHolder").transform;
		stlHolder = GameObject.Find ("stlHolder").transform;
		jobHolder = GameObject.Find ("jobHolder").transform;
        GetComponent<InspectorR>().MainRect = new Rect(Screen.width - 255, 5, 250, 570);
        GetComponent<InspectorL>().MainRect = new Rect(5, 5, 250, 570);
        windowStyle.fontSize = 50;
        MM = GameObject.Find("MESH").GetComponent<MakeMesh>();
        MM.material = stlMat;
        MM.Begin();
	}

	public void loadFile()
	{
		loading = true;
		System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog ();
			openFileDialog.InitialDirectory = Application.dataPath + "/Samples";
			if (!stlCodeLoaded && !dmcCodeLoaded)
				openFileDialog.Filter = "DMC Files (*.DMC)|*.DMC|STL Files (*.STL)|*.STL";
		else if (!stlCodeLoaded)
			openFileDialog.Filter = "STL Files (*.STL)|*.STL";
		else if (!dmcCodeLoaded)
			openFileDialog.Filter = "DMC Files (*.DMC)|*.DMC";
		//openFileDialog.Filter = "JOB Files (*.JOB)|*.JOB|DMC Files (*.DMC)|*.DMC|STL Files (*.STL)|*.STL";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = false;

		if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			try
			{
				var fileName = openFileDialog.FileName;
				if (fileName.EndsWith("JOB"))
					type = Type.JOB;
				else if (fileName.EndsWith("STL"))
					type = Type.STL; 
				else if (fileName.EndsWith("DMC"))
					type = Type.DMC; 
				if (fileName != null)
				{
					var reader = new StreamReader(fileName);
					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();
						switch (type)
						{
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
					    case Type.JOB:
						    Draw (Type.JOB);
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
						    break;
					    default:
						    break;
					}
					Camera.main.GetComponent<CameraControl>().target.position = new Vector3 ((xMax + xMin) / 2, (yMax + yMin) / 2, (yMax + yMin) / 2);
					transform.LookAt(Camera.main.GetComponent<CameraControl>().target);
					loading = false;
					drawn = false;
//					if (type == Type.STL)
//					{
//						MeshFilter[] meshFilters = stlHolder.GetComponentsInChildren<MeshFilter>();
//						CombineInstance[] combine = new CombineInstance[meshFilters.Length];
//						int i = 0;
//						while (i < meshFilters.Length)
//						{
//							combine[i].mesh = meshFilters[i].sharedMesh;
//							combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
//							meshFilters[i].gameObject.active = false;
//						}
//						stlHolder.transform.GetComponent<MeshFilter>().mesh = new Mesh();
//						stlHolder.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
//						stlHolder.transform.gameObject.active = true;
//					}
				}
			}
			catch {}
		}
	}

	void OnGUI()
	{
		GUI.skin = inspectorSkin;
        var groupRectL = GetComponent<InspectorL>().MainRect;
        GUI.Window(0, groupRectL, GetComponent<InspectorL>().InspectorWindowL, "Inspector");
        var groupRectR = GetComponent<InspectorR>().MainRect;
        GUI.Window(1, groupRectR, GetComponent<InspectorR>().InspectorWindowR, "Voxel Manager");
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
			var line = Instantiate (lineRenderer) as LineRenderer;
			if (_type == Type.JOB)
				line.transform.SetParent(jobHolder);
			else if (_type == Type.DMC)
				line.transform.SetParent(dmcHolder);
			line.SetVertexCount (2);
			line.SetPosition (0 , Vertex0);
			line.SetPosition (1 , Vertex1);
			line.material = mat;
			line.SetWidth (0.002f, 0.002f);
			if (_type == Type.JOB)
			{
				line.SetColors (Inspector.jobLineColor, Inspector.jobLineColor);
				newSegment.Line = line;
				newSegment.p1 = Vertex0;
				newSegment.p2 = Vertex1;
				jobLines.Add (newSegment);
			}
			else if (_type == Type.DMC)
			{
				line.SetColors (Inspector.dmcLineColor, Inspector.dmcLineColor);
				newSegment.Line = line;
				newSegment.p1 = Vertex0;
				newSegment.p2 = Vertex1;
				newSegment.step = dmcLines.Count;
				dmcLines.Add (newSegment);
			}
		}
	}

	void scanJOB(string _line)
	{
		_line = _line.Trim();
		jobCode.Add (_line.ToString () + "\r\n");
        jobCodeLong += _line.ToString() + "      " + "\r\n";
        var chunks = _line.Split(' ');
		switch (chunks[0][0])
		{
		case 'G':
			gCodeInterpreter.StartsWithG(_line);
			break;
		case '*':
			gCodeInterpreter.StartsWithStar(_line);
			break;
		default:
			break;
		}
	}

	void scanDMC(string _line)
	{
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
		_line = _line.Trim();
		stlCode.Add (_line.ToString () + "\r\n");
        stlCodeLong += _line.ToString() + "      " + "\r\n";
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
}
