using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InspectorT : InspectorManager
{
    public static string[] TopToolbarStrings = { "File", "Edit", "Window", "Help" };
    private Rect mainRect;
    private Rect thisRect;
    int index = 0;
    public GUISkin skin;

    public void InspectorWindowT(int id)
    {
        GUI.skin = skin;
        GUILayout.BeginArea(mainRect);
        {
            index = GUILayout.Toolbar(index, TopToolbarStrings);
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
        GUILayout.EndArea();
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

    public void _File()
    {
        GUILayout.Box("FUN");
        
        
    }
}
