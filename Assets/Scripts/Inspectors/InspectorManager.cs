using UnityEngine;
using System.Collections;

public class InspectorManager : MonoBehaviour
{
    public static int margin = 10;
    public static GUIStyle KeyStyle;
    public static GUIStyle KeyStyleR;
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
        KeyStyleR = new GUIStyle(KeyStyle);
        KeyStyleR.alignment = TextAnchor.UpperRight;
    }

    public void Restart()
    {
        ClearVoxels();
        LoadFile.Max = new Vector3(-1000, -1000, -1000);
        LoadFile.Min = new Vector3(1000, 1000, 1000);
        ClearSTL(0);
        ClearGCD();
        ClearJOB();
        ClearDMC();
        ClearSTL(1);
        PathFitter.maxPathsThrough = 0;
    }

    public void ClearSTL(int i)
    {
        cSection.ready = false;
        cSection.triangles.Clear();
        cSection.csLines.Clear();
        cSection.csPlaneEqn = new Vector4(0f, 1f, 0f, 0f);
        if (i == 0)
        {
            LoadFile.stlCodeLoaded = false;
            GameObject.Find("MESH").GetComponent<MakeMesh>().ClearAll();
        }
        else
            GameObject.Find("MESH").GetComponent<MakeMesh>().Begin();
    }

    public void ClearGCD()
    {
        LoadFile.gcdCodeLoaded = false;
        InspectorR.voxelsFitted = false;
        InspectorR.highlightType = InspectorR.HighlighType.None;
        
        LoadFile.gcdLines.Clear();
        LoadFile.firstGcdLineInCode = 0;
        LoadFile.gcdCode.Clear();
        LoadFile.model_code_xrefGCD.Clear();
        PathFitter.maxPathsThrough = 0;
        GcdInterpreter.y = 0;
    }

    public void ClearJOB()
    {
        LoadFile.jobCodeLoaded = false;
        LoadFile.jobLines.Clear();
        LoadFile.firstJobLineInCode = 0;
        LoadFile.jobCode.Clear();
        LoadFile.model_code_xrefJOB.Clear();
        JobInterpreter.y = 0;
    }

    public void ClearDMC()
    {
        LoadFile.dmcCodeLoaded = false;

        LoadFile.dmcLines.Clear();
        LoadFile.firstDmcLineInCode = 0;
        LoadFile.dmcCode.Clear();
        LoadFile.model_code_xrefDMC.Clear();
        DmcInterpreter.y = 0;
    }

    public void ClearVoxels()
    {
        InspectorR.voxelsLoaded = false;
        InspectorR.voxelsFitted = false;
        InspectorR.highlightType = InspectorR.HighlighType.None;
        foreach (var v in MeshVoxelizer.voxels)
        {
            Destroy(v.Value.Voxel);
        }
        MeshVoxelizer.voxels.Clear();
        MeshVoxelizer.highlights.Clear();
        MeshVoxelizer.ClearAll();
        PathFitter.maxPathsThrough = 0;
    }


}
