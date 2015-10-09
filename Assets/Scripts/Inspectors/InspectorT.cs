using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    private Rect HelpRect;
    public GUISkin skin;
    public enum Dropdown {File,Edit,Window,Help, None };
    public Dropdown dropdown;
    private int width = 100;
    int index = 100;
    Ray ray;
    bool buttonPressed = false;
    

    public void InspectorWindowT(int id)
    {
        GUI.skin = skin;
        var mp = Input.mousePosition;
        mp.y = Screen.height - mp.y;
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
                    index = 3;
                }
                else if (CheckForHover())
                    index = 3;
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
        mp.y = Screen.height - mp.y;
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
            if (GUILayout.Button("Save"))
            {
                index = 100;
                dropdown = Dropdown.None;
                return;
            }
            if (GUILayout.Button("Load"))
            {
                Camera.main.GetComponent<LoadFile>().loadFile();
                index = 100;
                dropdown = Dropdown.None;
                return;
            }
            if (GUILayout.Button("Clear"))
            {
                Restart();
                index = 100;
                dropdown = Dropdown.None;
                return;
            }

            if (GUILayout.Button("Quit"))
            {
                Application.Quit();
            }
        }
        GUILayout.EndVertical();
        var mp = Input.mousePosition;
        mp.y = Screen.height - mp.y;
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
                if (GUILayout.Button("Inspector"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("Voxel Manager"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }
                if (GUILayout.Button("Temp History"))
                {
                    index = 100;
                    dropdown = Dropdown.None;
                    return;
                }

                if (GUILayout.Button("Main Panel"))
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
        mp.y = Screen.height - mp.y;
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
}
