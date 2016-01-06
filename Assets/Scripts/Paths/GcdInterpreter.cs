using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class GcdInterpreter
{
    public static float y = 0;
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }
    public bool LaserOn = false;
    public Vector3 centroid
    {
        get
        {
            var c = (Min + Max) / 2.0f;
            return c;
        }
    }

    public GcdInterpreter()
    {
        y = 0;
        Min = new Vector3(1000, 1000, 1000);
        Max = new Vector3(-1000, -1000, -1000);
    }

    public void StartsWithG(string _line)
    {
        if (!_line.Contains(' ')) return;
        var _chunks = _line.Split(' ');
        var _chunk = _chunks[0]; // Initial Line, which starts with 'G'
        if (_chunks.Length < 2) return;
        switch (_chunk[1])
        {
            case '1':
                DoG(_chunks);
                break;
            default:
                break;
        }
    }

    public void StartsWithOU(string _line)
    {
        if (!_line.Contains(' ')) return;
        var _chunks = _line.Split(' ');
        var onOff = _chunks[1].Split(',')[1];
        switch (onOff[1])
        {
            case '1':
                LaserOn = true;
                break;
            case '0':
                LaserOn = false;
                break;
            default:
                break;
        }
    }

    void DoG(string[] _chunks)
    {
        if (_chunks[1].Contains('F')) return;
        if (_chunks[1].StartsWith("Z"))
        {
            var Y = _chunks[1].TrimStart('Z');
            float _y;
            if (float.TryParse(Y, out _y))
            {
                y = _y;
                return;
            }
        }
        Vector3 newVertex = new Vector3();
        var X = _chunks[1].TrimStart('X');
        var Z = _chunks[2].TrimStart('Y');
        float x;
        float z;
        if (float.TryParse(X, out x))
        {
            x -= 0.1f;
            newVertex.x = x;
        }
        if (float.TryParse(Z, out z))
        {
            z -= 0.1f;
            newVertex.z = z;
        }
        newVertex.y = y;
        var max = Max;
        var min = Min;
        if (x > Max.x) max.x = x;
        if (x < Min.x) min.x = x;
        if (y > Max.y) max.y = y;
        if (y < Min.y) min.y = y;
        if (z > Max.z) max.z = z;
        if (z < Min.z) min.z = z;
        Max = max;
        Min = min;
        if (min.z <= LoadFile.Min.z)
            LoadFile.Min.z = min.z;
        LoadFile.gcdVerts.Add(newVertex);
        Camera.main.GetComponent<LoadFile>().vertices.Add(newVertex);
        if (LoadFile.model_code_xrefGCD.Count == 0)
            LoadFile.firstGcdLineInCode = LoadFile.gcdCode.Count - 1;
        LoadFile.LaserOnModelRef.Add(LoadFile.model_code_xrefGCD.Count, LaserOn);
        LoadFile.model_code_xrefGCD.Add(LoadFile.model_code_xrefGCD.Count, LoadFile.gcdCode.Count - 1);
    }

    public void StartsWithO(string _line)
    {
        var _chunks = _line.Split(' ');
        var _chunk = _chunks[0]; // Initial Line, which starts with 'G'
        switch (_chunk[1])
        {
            case 'U':
                switch (_chunks[2][3])
                {
                    case '0':
                        MessageBox.Show("Off");
                        break;
                    case '1':
                        MessageBox.Show("On");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
