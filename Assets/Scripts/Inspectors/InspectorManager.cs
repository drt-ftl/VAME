using UnityEngine;
using System.Collections;

public class InspectorManager : MonoBehaviour
{
    public static int margin = 10;
    public static GUIStyle KeyStyle;
    public static GUIStyle codeStyle;


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
	

}
