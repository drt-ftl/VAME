using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DxfSurfaceConverter
{
	private string dxfCode = "";

	public DxfSurfaceConverter (List<GameObject> _triangles)
	{
		AddHeader();
		int faceCounter = 0;
		//foreach (var triangleObject in _triangles)
		//{
		//	if (triangleObject.GetComponent<TriangleMaker>() == null)
		//		continue;
		//	var triangle = triangleObject.GetComponent<TriangleMaker>();
		//	var t = triangle.GetPoints();
		//	var offset = new Vector3(0.5f,0.5f,0.5f);
		//	var scale = 1f;
		//	var p1 = (t[0] - offset) * scale;
		//	var p2 = (t[1] - offset) * scale;
		//	var p3 = (t[2] - offset) * scale;
		//	var p4 = (t[3] - offset) * scale;
		//	dxfCode += "\n0\n3DFACE\n8\n_0";
		//	var i = 0;
		//	dxfCode += "\n" + "1" + i.ToString () + "\n" + p1.x.ToString ("f7");
		//	dxfCode += "\n" + "2" + i.ToString () + "\n" + p1.y.ToString ("f7");
		//	dxfCode += "\n" + "3" + i.ToString () + "\n" + p1.z.ToString ("f7");
		//	i++;
		//	dxfCode += "\n" + "1" + i.ToString () + "\n" + p2.x.ToString ("f7");
		//	dxfCode += "\n" + "2" + i.ToString () + "\n" + p2.y.ToString ("f7");
		//	dxfCode += "\n" + "3" + i.ToString () + "\n" + p2.z.ToString ("f7");
		//	i++;
		//	dxfCode += "\n" + "1" + i.ToString () + "\n" + p3.x.ToString ("f7");
		//	dxfCode += "\n" + "2" + i.ToString () + "\n" + p3.y.ToString ("f7");
		//	dxfCode += "\n" + "3" + i.ToString () + "\n" + p3.z.ToString ("f7");
		//	i++;
		//	dxfCode += "\n" + "1" + i.ToString () + "\n" + p4.x.ToString ("f7");
		//	dxfCode += "\n" + "2" + i.ToString () + "\n" + p4.y.ToString ("f7");
		//	dxfCode += "\n" + "3" + i.ToString () + "\n" + p4.z.ToString ("f7");
		//}
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
