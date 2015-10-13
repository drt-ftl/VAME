using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class DmcInterpreter
{
	float y = 0;
    private Vector3 Min { get; set; }
    private Vector3 Max { get; set; }
    public Vector3 offsetGCD { get; set; }
    public Vector3 centroid
    {
        get
        {
            var c = (Min + Max) / 2.0f;
            return c;
        }
    }

    public DmcInterpreter()
    {
        Min = new Vector3(1000, 1000, 1000);
        Max = new Vector3(-1000, -1000, -1000);
    }
    public void StartsWithPA(string _line, float _scale)
	{
		var _chunks = _line.Split(' ');
		//var _chunk = _chunks[0]; // Initial Line, which starts with 'VP'
		var _xy = _chunks [1].Split (',');
		float x;
		float z;
		Vector3 newVertex = new Vector3();
		if (float.TryParse (_xy [0], out x))
			newVertex.x = x;
		if (float.TryParse (_xy [1], out z))
			newVertex.z = z;
		newVertex.y = y;
		newVertex *= _scale;
        var max = Max;
        var min = Min;
        if (newVertex.x > Max.x) max.x = newVertex.x;
        if (newVertex.x < Min.x) min.x = newVertex.x;
        if (newVertex.y > Max.y) max.y = newVertex.y;
        if (newVertex.y < Min.y) min.y = newVertex.y;
        if (newVertex.z > Max.z) max.z = newVertex.z;
        if (newVertex.z < Min.z) min.z = newVertex.z;
        Max = max;
        Min = min;
        if (min.z <= LoadFile.Min.z)
            LoadFile.Min.z = min.z;
            Camera.main.GetComponent<LoadFile>().vertices.Add(newVertex);
		if (LoadFile.model_code_xrefDMC.Count == 0)
			LoadFile.firstDmcLineInCode = LoadFile.dmcCode.Count - 1;
		LoadFile.model_code_xrefDMC.Add (Camera.main.GetComponent<LoadFile> ().vertices.Count - 1, LoadFile.dmcCode.Count - 1);
	}

	public void StartsWithREM(string _line)
	{
		var _chunks = _line.Split(' ');
		if (_chunks[1] == "Layer" && _chunks[2] == "Change")
		{
			y += 0.015f * 100000;
		}
	}
}
