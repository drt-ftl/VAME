using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class GCodeInterpreter
{
	float z = 0;
    public void StartsWithG(string _line)
    {
        var _chunks = _line.Split(' ');
        var _chunk = _chunks[0]; // Initial Line, which starts with 'G'
        switch (_chunk[1])
        {
            case '0':
                switch (_chunk[2])
                {
                    case '0':
                        
                        break;
                    case '1':
                        Vector3 newVertex = new Vector3();
                        var X = _chunks[1].TrimStart('X');
                        var Y = _chunks[2].TrimStart('Y');
                        float x;
                        float y;
                        if (float.TryParse(X, out x))
                            newVertex.x = x;
                        if (float.TryParse(Y, out y))
                            newVertex.y = y;
                        newVertex.z = z;

						if (x > LoadFile.xMax) LoadFile.xMax = x;
						if (x < LoadFile.xMin) LoadFile.xMin = x;
						if (y > LoadFile.yMax) LoadFile.yMax = y;
						if (y < LoadFile.yMin) LoadFile.yMin = y;
						if (z > LoadFile.zMax) LoadFile.zMax = z;
						if (z < LoadFile.zMin) LoadFile.zMin = z;
                        Camera.main.GetComponent<LoadFile>().vertices.Add(newVertex);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
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
				z += 0.015f;
		}
	}
}
