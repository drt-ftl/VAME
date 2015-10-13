using UnityEngine;
using System.Collections;

public class InspectorManager : MonoBehaviour
{
    public static int margin = 10;
    public static GUIStyle KeyStyle;
    public static GUIStyle codeStyle;
    public static bool InspectorWindow = true;
    public static bool VoxelManager = false;


    void Awake ()
    {
        KeyStyle = new GUIStyle();
        KeyStyle.fontSize = 9;
        KeyStyle.fontStyle = FontStyle.Italic;
        KeyStyle.normal.textColor = Color.black;
        codeStyle = new GUIStyle(KeyStyle);
        codeStyle.fontStyle = FontStyle.Normal;
        codeStyle.fontSize = 10;
    }

    public void Restart()
    {
        InspectorR.voxelsLoaded = false;
        InspectorR.voxelsFitted = false;
        InspectorR.highlightType = InspectorR.HighlighType.None;
        LoadFile.stlCodeLoaded = false;
        LoadFile.dmcCodeLoaded = false;
        LoadFile.jobCodeLoaded = false;
        LoadFile.gcdCodeLoaded = false;
        LoadFile.Max = new Vector3(-1000, -1000, -1000);
        LoadFile.Min = new Vector3(1000, 1000, 1000);
        GameObject.Find("MESH").GetComponent<MakeMesh>().ClearAll();
        foreach (var v in MeshVoxelizer.voxels)
        {
            Destroy(v.Value.Voxel);
        }
        MeshVoxelizer.voxels.Clear();
        MeshVoxelizer.highlights.Clear();
        MeshVoxelizer.ClearAll();
        foreach (var gl in LoadFile.gcdLines)
        {
            Destroy(gl.Line.gameObject);
        }
        LoadFile.gcdLines.Clear();
        LoadFile.firstGcdLineInCode = 0;
        LoadFile.gcdCode.Clear();
        LoadFile.model_code_xrefGCD.Clear();

        foreach (var dl in LoadFile.dmcLines)
        {
            Destroy(dl.Line.gameObject);
        }
        LoadFile.dmcLines.Clear();
        LoadFile.firstGcdLineInCode = 0;
        LoadFile.dmcCode.Clear();
        LoadFile.model_code_xrefGCD.Clear();

        foreach (var jl in LoadFile.jobLines)
        {
            Destroy(jl.Line.gameObject);
        }
        LoadFile.jobLines.Clear();
        LoadFile.firstGcdLineInCode = 0;
        LoadFile.jobCode.Clear();
        LoadFile.model_code_xrefGCD.Clear();
        GameObject.Find("MESH").GetComponent<MakeMesh>().Begin();
        PathFitter.maxPathsThrough = 0;
    }
	

}
