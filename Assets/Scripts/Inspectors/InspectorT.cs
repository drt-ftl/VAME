using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

public class InspectorT : InspectorManager
{
    public static string[] TopToolbarStrings = { "File", "Edit", "Window", "Help" };
    public static string[] FileStrings = { "Save", "Crazy", "Clear", "Quit" };
    bool FileUp = false;
    private Rect mainRect;
    private Rect thisRect;
    private Rect fileRect;
    private Rect editRect;
    private Rect windowRect;
    private Rect helpRect;
    public GUISkin skin;
    public enum Dropdown {File,Edit,Window,Help, None };
    public Dropdown dropdown;
    private int width = 100;
    int index = 100;
    Ray ray;
    bool buttonPressed = false;
    [DllImport("user32.dll")]
    private static extern void SaveFileDialog();



    public void InspectorWindowT(int id)
    {
        GUI.skin = skin;
        var mp = Input.mousePosition;
        mp.y = UnityEngine.Screen.height - mp.y;
        GUILayout.BeginArea(mainRect);
        {
            GUILayout.BeginHorizontal(GUILayout.Width(width), GUILayout.Height(15));
            {
                if (GUILayout.Button("<b>File</b>"))
                {
                    fileRect = GUILayoutUtility.GetLastRect();
                    index = 0;
                }
                else if (CheckForHover())
                {
                    fileRect = GUILayoutUtility.GetLastRect();
                    index = 0;
                }
                if (GUILayout.Button("<b>Edit</b>"))
                {
                    index = 1;
                }
                else if (CheckForHover())
                    index = 1;
                if (GUILayout.Button("<b>Window</b>"))
                {
                    windowRect = GUILayoutUtility.GetLastRect();
                    index = 2;
                }
                else if (CheckForHover())
                {
                    windowRect = GUILayoutUtility.GetLastRect();
                    index = 2;
                }
                if (GUILayout.Button("<b>Help</b>"))
                {
                    helpRect = GUILayoutUtility.GetLastRect();
                    index = 3;
                }
                else if (CheckForHover())
                {
                    helpRect = GUILayoutUtility.GetLastRect();
                    index = 3;
                }
            }
            GUILayout.EndHorizontal();

            switch (dropdown)
            {
                case Dropdown.File:
                    _File(fileRect, FileStrings.Length);
                    index = 0;
                    break;
                case Dropdown.Window:
                    _Window(windowRect, 4);
                    index = 2;
                    break;
                case Dropdown.Help:
                    _Help(helpRect, 3);
                    index = 3;
                    break;
                default:
                    break;
            }
        }
        GUILayout.EndArea();
    }

    void Update()
    {
        switch (index)
        {
            case 0:
                dropdown = Dropdown.File;
                break;
            case 2:
                dropdown = Dropdown.Window;
                break;
            case 3:
                dropdown = Dropdown.Help;
                break;
            default:
                dropdown = Dropdown.None;
                break;
        }
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

    public bool CheckForHover()
    {
        var mp = Input.mousePosition;
        mp.y = UnityEngine.Screen.height - mp.y;
        if (mp.x <= GUILayoutUtility.GetLastRect().xMax
            && mp.x >= GUILayoutUtility.GetLastRect().xMin
            && mp.y <= GUILayoutUtility.GetLastRect().yMax
            && mp.y >= GUILayoutUtility.GetLastRect().yMin)
        {
            return true;
        }
        return false;
    }
    public void _File(Rect rect, int slots)
    {
        rect.height += slots * rect.height;
        //if (dropdown != Dropdown.File) return;
        GUILayout.BeginVertical();
        {
            if (GUILayout.Button("Save", "dd"))
            {
                saveFile();
                index = 100;
                dropdown = Dropdown.None;
                return;
            }
            if (GUILayout.Button("Load", "dd"))
            {
                Camera.main.GetComponent<LoadFile>().loadFile();
                index = 100;
                dropdown = Dropdown.None;
                return;
            }
            if (GUILayout.Button("Clear", "dd"))
            {
                Restart();
                index = 100;
                dropdown = Dropdown.None;
                return;
            }

            if (GUILayout.Button("Quit", "dd"))
            {
                UnityEngine.Application.Quit();
            }
        }
        GUILayout.EndVertical();
        var mp = Input.mousePosition;
        mp.y = UnityEngine.Screen.height - mp.y;
        if (mp.x <= rect.xMax
            && mp.x >= rect.xMin
            && mp.y <= rect.yMax
            && mp.y >= rect.yMin)
        {
            index = 0;
        }
        else
        {
            index = 100;
            dropdown = Dropdown.None;
        }
    }
    public void saveFile()
    {
        System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
        saveFileDialog.InitialDirectory = UnityEngine.Application.dataPath + "/Samples";
        var sel = "VAME Files (*.vme)|*.vme";
        saveFileDialog.Filter = sel;
        saveFileDialog.RestoreDirectory = true;
        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            var tw = File.CreateText(saveFileDialog.FileName);
            tw.WriteLine("|||ftlVAME AMF BEGIN");
            {
                #region STL
                if (LoadFile.stlCodeLoaded)
                {
                    tw.WriteLine("|||ftlVAME STL BEGIN");
                    {
                        foreach (var line in LoadFile.stlCode)
                        {
                            tw.Write(line);
                        }
                    }
                    tw.WriteLine("|||ftlVAME STL END");
                }
                #endregion
                #region GCD
                if (LoadFile.gcdCodeLoaded)
                {
                    tw.WriteLine("|||ftlVAME GCD BEGIN");
                    {
                        foreach (var line in LoadFile.gcdCode)
                        {
                            tw.Write(line);
                        }
                    }
                    tw.WriteLine("|||ftlVAME GCD END");
                }
                #endregion
                #region JOB
                if (LoadFile.jobCodeLoaded)
                {
                    tw.WriteLine("|||ftlVAME JOB BEGIN");
                    {
                        foreach (var line in LoadFile.jobCode)
                        {
                            tw.Write(line);
                        }
                    }
                    tw.WriteLine("|||ftlVAME JOB END");
                }
                #endregion
                #region DMC
                if (LoadFile.dmcCodeLoaded)
                {
                    tw.WriteLine("|||ftlVAME DMC BEGIN");
                    {
                        foreach (var line in LoadFile.dmcCode)
                        {
                            tw.Write(line);
                        }
                    }
                    tw.WriteLine("|||ftlVAME DMC END");
                }
                #endregion
                #region Voxels
                if (MeshVoxelizer.voxels.Count > 0)
                {
                    tw.WriteLine("|||ftlVAME Voxels BEGIN " + InspectorR.voxelVis.ToString("f2") + " " + InspectorR.resolution.ToString("f2"));
                    {
                        foreach (var voxel in MeshVoxelizer.voxels)
                        {
                            tw.WriteLine(voxel.Key.ToString());
                        }
                    }
                    tw.WriteLine("|||ftlVAME Voxels END");
                }
                #endregion
            }
            tw.WriteLine("|||ftlVAME AMF END");
            tw.Close();
        }
    }
    public void _Window(Rect rect, int slots)
    {
        rect.height += slots * rect.height;
        //rect.x += 2 * rect.width;
        //if (dropdown != Dropdown.File) return;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(rect.xMin);
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Inspector", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("Voxel Manager", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("Temp History", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }

                if (GUILayout.Button("Main Panel", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        var mp = Input.mousePosition;
        mp.y = UnityEngine.Screen.height - mp.y;
        if (mp.x <= rect.xMax
            && mp.x >= rect.xMin
            && mp.y <= rect.yMax
            && mp.y >= rect.yMin)
        {
            index = 2;
        }
        else
        {
            index = 100;
            dropdown = Dropdown.None;
        }
    }

    void _Help(Rect rect, int slots)
    {
        rect.height += slots * rect.height;
        //rect.x += 2 * rect.width;
        //if (dropdown != Dropdown.File) return;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(rect.xMin);
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Manual","dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("Keyboard Commands", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("About", "dd"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        var mp = Input.mousePosition;
        mp.y = UnityEngine.Screen.height - mp.y;
        if (mp.x <= rect.xMax
            && mp.x >= rect.xMin
            && mp.y <= rect.yMax
            && mp.y >= rect.yMin)
        {
            index = 3;
        }
        else
        {
            index = 100;
            dropdown = Dropdown.None;
        }
    }
}
