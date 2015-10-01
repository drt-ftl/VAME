using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class DmcInterpreter
{
	float y = 0;
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
		if (newVertex.x > LoadFile.xMax) LoadFile.xMax = newVertex.x;
		if (newVertex.x < LoadFile.xMin) LoadFile.xMin = newVertex.x;
		if (newVertex.y > LoadFile.yMax) LoadFile.yMax = newVertex.y;
		if (newVertex.y < LoadFile.yMin) LoadFile.yMin = newVertex.y;
		if (newVertex.z > LoadFile.zMax) LoadFile.zMax = newVertex.z;
		if (newVertex.z < LoadFile.zMin) LoadFile.zMin = newVertex.z;
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
