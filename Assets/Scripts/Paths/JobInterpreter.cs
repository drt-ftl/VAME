using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class JobInterpreter
{
	float y = 0;
    public void StartsWithG(string _line)
    {
        if (!_line.Contains(' ')) return;
        var _chunks = _line.Split(' ');
        var _chunk = _chunks[0]; // Initial Line, which starts with 'G'
        if (_chunks.Length < 3) return;
        switch (_chunk[1])
        {
            case '0':
                switch (_chunk[2])
                {
                    case '0':
                        
                        break;
                    case '1':
                        DoG(_chunks);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    void DoG(string[] _chunks)
    {
        Vector3 newVertex = new Vector3();
        var X = _chunks[1].TrimStart('X');
        var Z = _chunks[2].TrimStart('Y');
        float x;
        float z;
        if (float.TryParse(X, out x))
            newVertex.x = x;
        if (float.TryParse(Z, out z))
            newVertex.z = z;
        newVertex.y = y;

        if (x > LoadFile.xMax) LoadFile.xMax = x;
        if (x < LoadFile.xMin) LoadFile.xMin = x;
        if (y > LoadFile.yMax) LoadFile.yMax = y;
        if (y < LoadFile.yMin) LoadFile.yMin = y;
        if (z > LoadFile.zMax) LoadFile.zMax = z;
        if (z < LoadFile.zMin) LoadFile.zMin = z;
        Camera.main.GetComponent<LoadFile>().vertices.Add(newVertex);
        if (LoadFile.model_code_xrefJOB.Count == 0)
            LoadFile.firstJobLineInCode = LoadFile.jobCode.Count - 1;
        LoadFile.model_code_xrefJOB.Add(LoadFile.model_code_xrefJOB.Count, LoadFile.jobCode.Count - 1);
    }

    public void StartsWithM(string _line)
    {
        var _chunks = _line.Split(' ');
        var _chunk = _chunks[0]; // Initial Line, which starts with 'G'
        switch (_chunk[1])
        {
            case '0':
                switch (_chunk[2])
                {
                    case '0':
                        MessageBox.Show("G00");
                        break;
                    case '1':
                        MessageBox.Show("G01");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

	public void StartsWithStar(string _line)
	{
		var _chunks = _line.Split(' ');
		if (_chunks[0] == "*REM" && _chunks[1] == "Layer" && _chunks[2] == "Change")
		{
				y += 0.015f;
		}
	}
}
