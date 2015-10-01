using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DxfLineConverter
{
	private string dxfCode = "";
	
	public void ConvertFull (List<LineSegment> _lines)
	{
		dxfCode = "";
		AddHeader();
		foreach (var line in _lines)
		{
			//if (line.Line == null)
			//	continue;

			var p1 = line.p1;
			var p2 = line.p2;

			dxfCode += "\n0\nLINE\n8\nPolygon";
			dxfCode += "\n10\n" + p1.x.ToString ("f7");
			dxfCode += "\n20\n" + p1.y.ToString ("f7");
			dxfCode += "\n30\n" + p1.z.ToString ("f7");

			dxfCode += "\n11\n" + p2.x.ToString ("f7");
			dxfCode += "\n21\n" + p2.y.ToString ("f7");
			dxfCode += "\n31\n" + p2.z.ToString ("f7");
		}
		AddFooter ();
	}

	public void ConvertSelected (Vector2 timeSliders, List<LineSegment> _lines)
	{
		dxfCode = "";
		AddHeader();
		foreach (var line in _lines)
		{
			//if (line.Line == null || line.p1 == null || line.p2 == null || line.step < timeSliders.x || line.step > timeSliders.y)
			//	continue;
			
			var p1 = line.p1;
			var p2 = line.p2;
			
			dxfCode += "\n0\nLINE\n8\nPolygon";
			dxfCode += "\n10\n" + p1.x.ToString ("f7");
			dxfCode += "\n20\n" + p1.y.ToString ("f7");
			dxfCode += "\n30\n" + p1.z.ToString ("f7");
			
			dxfCode += "\n11\n" + p2.x.ToString ("f7");
			dxfCode += "\n21\n" + p2.y.ToString ("f7");
			dxfCode += "\n31\n" + p2.z.ToString ("f7");
		}
		AddFooter ();
	}
	
	private void AddHeader ()
	{
		dxfCode += "  0\nSECTION\n2\nHEADER\n9\n$ACADVER\n1\nAC1009\n0\nENDSEC\n0\nSECTION\n2\nTABLES\n0\nTABLE\n2\nLAYER\n70\n1\n0\nLAYER\n2\n_0\n0\nENDTAB\n0\nENDSEC\n0\nSECTION\n2\nBLOCKS\n0\nENDSEC\n0\nSECTION\n2\nENTITIES";
	}
	
	private void AddFooter ()
	{
		dxfCode += "\n0\nENDSEC\n0\nEOF";
	}
	
	public string GetDxf 
	{
		get { return dxfCode;}
	}
	
}
