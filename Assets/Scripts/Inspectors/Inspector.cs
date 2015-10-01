using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Inspector : MonoBehaviour 
{
	private int buttonW = 210;
	private int buttonH = 20;
	private bool jobToggle = false;
	private bool dmcToggle = false;
	private bool stlToggle = false;
	private bool jobTogglePrev = false;
	private bool dmcTogglePrev = false;
	private bool stlTogglePrev = false;
	private float stlVisSlider = 1;
	private float dmcVisSlider = 1;
	private float jobVisSlider = 1;
	private float stlTimeSlider = 1;
	public float stlTimeSliderPrev = 1;
	private float stlTimeSliderMin = 1;
	public float dmcTimeSlider = 1;
	public float dmcTimeSliderPrev = 1;
	private float dmcTimeSliderMin = 1;
	private float dmcCodeSlider = 1;
	private float jobTimeSlider = 1;
	public static Color stlColor = new Color (1.0f, 0.5f, 0.5f, 1f);
	public static Color dmcLineColor = new Color (0f, 0.7f, 0f, 1f);
	public static Color jobLineColor = new Color (1f, 0f, 0f, 1f);
	int maxCodeLines = 50;
	private GUIStyle codeStyle;
	private GUIStyle pointsStyle;
	private GUIStyle codeStyleH;
	private GUISkin skin;
	private int dmcCodeOffset = 0;
	private int stlCodeOffset = 0;
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

	void Start()
	{
		skin = Camera.main.GetComponent<LoadFile> ().inspectorSkin;
		codeStyle = new GUIStyle ();
		codeStyle.fontSize = 10;
		codeStyle.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 0.8f);

		codeStyleH = new GUIStyle ();
		codeStyleH.fontSize = 10;
		codeStyleH.normal.textColor = new Color (0.0f, 1.0f, 0.0f, 1.0f);

		pointsStyle = new GUIStyle ();
		pointsStyle.fontSize = 10;
		pointsStyle.normal.textColor = new Color (0f, 1.0f, 0f, 0.8f);
	}

	public void InspectorWindow (int id)
	{
		#region basic
		GUI.skin = null;
		var guiRect = new Rect (20, 40, buttonW, buttonH);
		var space = (int)buttonH * 1.1f;
		if (GUI.Button(guiRect, "QUIT")) 
			Application.Quit ();
		guiRect.y += space ;
		if (!LoadFile.loading && (!LoadFile.stlCodeLoaded || ! LoadFile.dmcCodeLoaded))
		{
			if (GUI.Button(guiRect, "LOAD"))
			{
				LoadFile.loading = true;
				GetComponent<LoadFile>().loadFile();
			}
		}
		guiRect.y += space;
		var togglePos = guiRect;
		togglePos.width = 60;
		guiRect.y += space;
		
		if (LoadFile.jobCodeLoaded)
		{
			jobToggle = GUI.Toggle (togglePos, jobToggle, "JOB");
			togglePos.x += 70;
		}
		if (LoadFile.dmcCodeLoaded)
		{
			dmcToggle = GUI.Toggle (togglePos, dmcToggle, "DMC");
			togglePos.x += 70;
		}
		if (LoadFile.stlCodeLoaded)
		{
			stlToggle = GUI.Toggle (togglePos, stlToggle, "STL");
		}
		#endregion
		#region stlSelected
		if (stlToggle) 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				stlCodeOffset -= 1;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				stlCodeOffset += 1;
			if (stlTimeSlider != stlTimeSliderPrev)
				stlCodeOffset = 0;
			stlTimeSliderPrev = stlTimeSlider;
			var max = maxCodeLines;
			if (max > LoadFile.stlCode.Count - 1)
				max = LoadFile.stlCode.Count - 1;
			if (stlTogglePrev == false) 
			{
				dmcToggle = false;
				jobToggle = false;
				//stlTimeSlider = (float)LoadFile.stlSurfaces.Count - 1;
			}
			
			
			stlVisSlider = GUI.HorizontalSlider (guiRect, stlVisSlider, 0, 1.0f);
			//foreach (var surface in LoadFile.stlSurfaces)
			//{
			//	if (surface != null)
			//	{
			//		stlColor.a = stlVisSlider * stlVisSlider;
			//		surface.GetComponent<TriangleMaker>().color = stlColor;
			//	}
			//}
			guiRect.y += 10;	
			GUI.Label (guiRect, "Visibility: " + (stlVisSlider * 100).ToString("f2") + "%");
			
			guiRect.y += space;	
			guiRect.height = 210;
			var str = "";
			if (LoadFile.stlCode.Count < max)
				max = LoadFile.stlCode.Count - 1;
			var highlightLine = "";
			var layoutRect = guiRect;
			var margin = 10f;
			layoutRect.x += margin;
			layoutRect.y += margin;
			layoutRect.width -= margin * 2;
			layoutRect.height -= margin * 2;
			if (stlTimeSlider >= 0)
			{
				var firstLineIndex = LoadFile.model_code_xrefSTL[(int)stlTimeSlider] - 4;
				if (firstLineIndex + stlCodeOffset + max > LoadFile.stlCode.Count)
				{
					//stlCodeOffset = LoadFile.stlCode.Count - firstLineIndex - max;
				}
				else if (firstLineIndex + stlCodeOffset < 0)
				{
					stlCodeOffset = 0;
				}
				highlightLine = LoadFile.stlCode [firstLineIndex + stlCodeOffset];
				for (int i = firstLineIndex + stlCodeOffset + 1; i < firstLineIndex + stlCodeOffset + max; i++) 
				{
					if (LoadFile.stlCode.Count > i)
						str += LoadFile.stlCode [i];
				}
				GUI.Box (guiRect, "");
				GUILayout.BeginArea(layoutRect);
				{
					GUILayout.Label (highlightLine, codeStyleH);
					GUILayout.Space (-10);
					GUILayout.Label (str, codeStyle);
				}
				GUILayout.EndArea();
			}
			else
			{
				for (int i = (int)stlTimeSlider; i < (int)stlTimeSlider + max; i++) 
				{
					if (LoadFile.stlCode.Count > i)
						str += LoadFile.stlCode [i];
				}
				GUI.Box (guiRect, "");
				GUILayout.BeginArea(layoutRect);
				{
					GUILayout.Label (str, codeStyle);
				}
				GUILayout.EndArea();
			}
			
			guiRect.y += space + guiRect.height;
			guiRect.height = buttonH;
			//stlTimeSlider = GUI.HorizontalSlider (guiRect, stlTimeSlider, 0, (float)LoadFile.stlSurfaces.Count - 1);
			guiRect.y += 10;
			GUI.Label (guiRect, "Time: " + ((int)stlTimeSlider).ToString());
			guiRect.y += space;
			if (stlTimeSliderMin > stlTimeSlider)
				stlTimeSliderMin = stlTimeSlider;
			stlTimeSliderMin = GUI.HorizontalSlider (guiRect, stlTimeSliderMin, 0, stlTimeSlider);
			guiRect.y += 10;
			GUI.Label (guiRect, "MinTime: " + ((int)stlTimeSliderMin).ToString());
			var index = 0;
			//foreach (var surface in LoadFile.stlSurfaces)
			//{
			//	if (surface != null)
			//	{
			//		if ((int)stlTimeSlider < index || (int)stlTimeSliderMin > index)
			//		{
			//			surface.GetComponent<TriangleMaker>().color = new Color(stlColor.r, stlColor.g, stlColor.b, 0f);
			//		}
			//		else if ((int)stlTimeSlider == index)
			//		{
			//			surface.GetComponent<TriangleMaker>().color = new Color (0.2f,1f,0.2f,stlColor.a);
			//			var points = surface.GetComponent<TriangleMaker>().GetPoints();
			//			_p1 = points[0];
			//			_p2 = points[1];
			//			_p3 = points[2];
			//		}
			//		else
			//		{
			//			surface.GetComponent<TriangleMaker>().color = stlColor;
			//		}
			//	}
			//	index++;
			//}
			guiRect.y += space;
			if (GUI.Button(guiRect, "Save DXF"))
			{
				//var dxf = new DxfSurfaceConverter(LoadFile.stlSurfaces);
				var tw = File.CreateText(Application.dataPath + "/surfaceDXF.dxf");
				//tw.WriteLine(dxf.GetDxf);
				tw.Close ();
			}
			guiRect.y += space;
			if (GUI.Button(guiRect, "Save Visible To DXF"))
			{
				var surfs = new List<GameObject>();
				//for (int i = (int)stlTimeSliderMin; i < stlTimeSlider; i++)
				//	surfs.Add (LoadFile.stlSurfaces[i]);
				var dxf = new DxfSurfaceConverter(surfs);
				var tw = File.CreateText(Application.dataPath + "/surfaceDXFSelection.dxf");
				tw.WriteLine(dxf.GetDxf);
				tw.Close ();
			}
			guiRect.y += space;
			guiRect.width = 40;

            var P = 0; // LoadFile.stlSurfaces[(int)stlTimeSlider].GetComponent<TriangleMaker>().GetPoints();
			if (stlTimeSlider != stlTimeSliderPrev)
			{
				_p1x = _p1.x;
				_p1y = _p1.y;
				_p1z = _p1.z;
				_p2x = _p2.x;
				_p2y = _p2.y;
				_p2z = _p2.z;
				_p3x = _p3.x;
				_p3y = _p3.y;
				_p3z = _p3.z;
				
			}
			else
			{
				guiRect.y += space;
				GUI.Label(guiRect,"X:", pointsStyle);
				guiRect.y += space;
				GUI.Label(guiRect,"Y:", pointsStyle);
				guiRect.y += space;
				GUI.Label(guiRect,"Z:", pointsStyle);
				guiRect.y -= space * 3;
				guiRect.x += 30;

				GUI.Label (guiRect, "P1");
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1x.ToString ("f4"), pointsStyle), out _p1x))
				{
					_p1.x = _p1x;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1y.ToString ("f4"), pointsStyle), out _p1y))
				{
					_p1.y = _p1y;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1z.ToString ("f4"), pointsStyle), out _p1z))
				{
					_p1.z = _p1z;
				}

				guiRect.y -= space * 3;
				guiRect.x += 50;
				GUI.Label (guiRect, "P2");
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p2x.ToString ("f4"), pointsStyle), out _p2x))
				{
					_p2.x = _p2x;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p2y.ToString ("f4"), pointsStyle), out _p2y))
				{
					_p2.y = _p2y;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p2z.ToString ("f4"), pointsStyle), out _p2z))
				{
					_p2.z = _p2z;
				}

				guiRect.y -= space * 3;
				guiRect.x += 50;
				GUI.Label (guiRect, "P3");
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p3x.ToString ("f4"), pointsStyle), out _p3x))
				{
					_p3.x = _p3x;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p3y.ToString ("f4"), pointsStyle), out _p3y))
				{
					_p3.y = _p3y;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p3z.ToString ("f4"), pointsStyle), out _p3z))
				{
					_p3.z = _p3z;
				}
				//LoadFile.stlSurfaces[(int)stlTimeSlider].GetComponent<TriangleMaker>().SetPoints(_p1, _p2, _p3);
			}
            guiRect.y += space;
        }
		#endregion

		#region dmcSelected
		if (dmcToggle) 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				dmcCodeOffset -= 1;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				dmcCodeOffset += 1;
			if (dmcTimeSlider != dmcTimeSliderPrev)
				dmcCodeOffset = 0;
			dmcTimeSliderPrev = dmcTimeSlider;
			var max = maxCodeLines;
			if (dmcTogglePrev == false) 
			{
				stlToggle = false;
				jobToggle = false;
				dmcTimeSlider = (float)LoadFile.dmcLines.Count;
			}

			
			dmcVisSlider = GUI.HorizontalSlider (guiRect, dmcVisSlider, 0, 1.0f);
			foreach (var line in LoadFile.dmcLines)
			{
				if (line.Line != null)
				{
					dmcLineColor.a = dmcVisSlider * dmcVisSlider;
					line.LineColor = dmcLineColor;
				}
			}
			guiRect.y += 10;	
			GUI.Label (guiRect, "Visibility: " + (dmcVisSlider * 100).ToString("f2") + "%");

			guiRect.y += space;	
			guiRect.height = 400;
			var str = "";
			if (LoadFile.dmcCode.Count < max)
				max = LoadFile.dmcCode.Count;
			var highlightLine = "";
			var layoutRect = guiRect;
			var margin = 10f;
			layoutRect.x += margin;
			layoutRect.y += margin;
			layoutRect.width -= margin * 2;
			layoutRect.height -= margin * 2;
			if (dmcTimeSlider >= LoadFile.firstDmcLineInCode)
			{
				//str += "\r\n";
				var firstLineIndex = LoadFile.model_code_xrefDMC[(int)dmcTimeSlider];
				if (firstLineIndex + dmcCodeOffset + max > LoadFile.dmcCode.Count)
				{
					dmcCodeOffset = LoadFile.dmcCode.Count - firstLineIndex - max;
				}
				else if (firstLineIndex + dmcCodeOffset < 0)
				{
					dmcCodeOffset = 0;
				}
				highlightLine = LoadFile.dmcCode [firstLineIndex + dmcCodeOffset];
				for (int i = firstLineIndex + dmcCodeOffset + 1; i < firstLineIndex + dmcCodeOffset + max; i++) 
				{
					if (LoadFile.dmcCode.Count > i)
						str += LoadFile.dmcCode [i];
				}
				GUI.Box (guiRect, "");
				GUILayout.BeginArea(layoutRect);
				{
					GUILayout.Label (highlightLine, codeStyleH);
					GUILayout.Space (-10);
					GUILayout.Label (str, codeStyle);
				}
				GUILayout.EndArea();
			}
			else
			{
				for (int i = (int)dmcTimeSlider; i < (int)dmcTimeSlider + max; i++) 
				{
					if (LoadFile.dmcCode.Count > i)
						str += LoadFile.dmcCode [i];
				}
				GUI.Box (guiRect, "");
				GUILayout.BeginArea(layoutRect);
				{
					GUILayout.Label (str, codeStyle);
				}
				GUILayout.EndArea();
			}

			guiRect.y += space + guiRect.height;
			guiRect.height = buttonH;
			guiRect.y += space;
			dmcTimeSlider = GUI.HorizontalSlider (guiRect, dmcTimeSlider, 1, (float)LoadFile.dmcLines.Count);
			guiRect.y += 10;
			GUI.Label (guiRect, "Time: " + ((int)dmcTimeSlider).ToString());
			guiRect.y += space;
			if (dmcTimeSliderMin > dmcTimeSlider)
				dmcTimeSliderMin = dmcTimeSlider;
			dmcTimeSliderMin = GUI.HorizontalSlider (guiRect, dmcTimeSliderMin, 0, dmcTimeSlider);
			guiRect.y += 10;
			GUI.Label (guiRect, "MinTime: " + ((int)dmcTimeSliderMin).ToString());
			var index = 0;
			foreach (var line in LoadFile.dmcLines)
			{
				if (line.Line != null)
				{
					if ((int)dmcTimeSlider < index || (int)dmcTimeSliderMin >index)
					{
						line.LineColor = new Color(dmcLineColor.r, dmcLineColor.g, dmcLineColor.b, 0f);
					}
					else if ((int)dmcTimeSlider == index)
					{
						line.LineWidth = 0.01f;
						line.LineColor = new Color (1f,1f,1f,dmcLineColor.a);
						_p1 = line.p1;
						_p2 = line.p2;
					}
					else
					{
						line.LineWidth = 0.002f;
						line.LineColor = dmcLineColor;
					}
				}
				index++;
			}
			guiRect.y += space;
			if (GUI.Button(guiRect, "Save DXF"))
			{
				var dxf = new DxfLineConverter();
				var tw = File.CreateText(Application.dataPath + "/lineDXF.dxf");
				dxf.ConvertFull(LoadFile.dmcLines);
				tw.WriteLine(dxf.GetDxf);
				tw.Close ();
			}
			guiRect.y += space;
			if (GUI.Button(guiRect, "Save Visible To DXF"))
			{
				var dxf = new DxfLineConverter();
				var tw = File.CreateText(Application.dataPath + "/lineDXFSelection.dxf");
				dxf.ConvertSelected(new Vector2(dmcTimeSliderMin, dmcTimeSlider), LoadFile.dmcLines);
				tw.WriteLine(dxf.GetDxf);
				tw.Close ();
			}
			guiRect.y += space;
            var gRx = guiRect.x;
            var gRw = guiRect.width;
			guiRect.width = 40;
			if (dmcTimeSlider != dmcTimeSliderPrev)
			{
				_p1x = _p1.x;
				_p1y = _p1.y;
				_p1z = _p1.z;				
				_p2x = _p2.x;
				_p2y = _p2.y;
				_p2z = _p2.z;

			}
			else
		    {
				guiRect.y += space;
				GUI.Label(guiRect,"X:", pointsStyle);
				guiRect.y += space;
				GUI.Label(guiRect,"Y:", pointsStyle);
				guiRect.y += space;
				GUI.Label(guiRect,"Z:", pointsStyle);
				guiRect.y -= space * 3;
				guiRect.x += 30;
				
				GUI.Label (guiRect, "P1");
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1x.ToString ("f4"), pointsStyle), out _p1x))
				{
					_p1.x = _p1x;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p1 = _p1;
					if ((int)dmcTimeSlider - 2 >= 0)
						LoadFile.dmcLines[(int)dmcTimeSlider - 2].p2 = _p1;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1y.ToString ("f4"), pointsStyle), out _p1y))
				{
					_p1.y = _p1y;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p1 = _p1;
					if ((int)dmcTimeSlider - 2 >= 0)
						LoadFile.dmcLines[(int)dmcTimeSlider - 2].p2 = _p1;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p1z.ToString ("f4"), pointsStyle), out _p1z))
				{
					_p1.z = _p1z;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p1 = _p1;
					if ((int)dmcTimeSlider - 2 >= 0)
						LoadFile.dmcLines[(int)dmcTimeSlider - 2].p2 = _p1;
				}

				guiRect.y -= space * 3;
				guiRect.x += 50;
				GUI.Label (guiRect, "P3");
				guiRect.y += space;

				if (float.TryParse(GUI.TextField(guiRect, _p2x.ToString ("f4"), pointsStyle), out _p2x))
				{
					_p2.x = _p2x;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p2 = _p2;
					if ((int)dmcTimeSlider + 1 < LoadFile.dmcLines.Count)
						LoadFile.dmcLines[(int)dmcTimeSlider +1].p1 = _p2;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p2y.ToString ("f4"), pointsStyle), out _p2y))
				{
					_p2.y = _p2y;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p2 = _p2;
					if ((int)dmcTimeSlider + 1 < LoadFile.dmcLines.Count)
						LoadFile.dmcLines[(int)dmcTimeSlider + 1].p1 = _p2;
				}
				guiRect.y += space;
				if (float.TryParse(GUI.TextField(guiRect, _p2z.ToString ("f4"), pointsStyle), out _p2z))
				{
					_p2.z = _p2z;
					LoadFile.dmcLines[(int)dmcTimeSlider - 1].p2 = _p2;
					if ((int)dmcTimeSlider + 1 < LoadFile.dmcLines.Count)
						LoadFile.dmcLines[(int)dmcTimeSlider + 1].p1 = _p2;
				}


			}
		} 
		#endregion

		#region JOB Selected
		if (jobToggle) 
		{
//			if (jobTogglePrev == false) 
//			{
//				stlToggle = false;
//				dmcToggle = false;
//			}
//			var str = "";
//			for (int i = 0; i <= 10; i++) 
//			{
//				str += LoadFile.jobCode [i];
//			}
//			GUI.Box (guiRect, str);
		}
		#endregion
		stlTogglePrev = stlToggle;
		dmcTogglePrev = dmcToggle;
		jobTogglePrev = jobToggle;
	}
}
