﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

public class GcdInterpreter
{
    float y = 0;
    bool on = true;

    public void StartsWithG(string _line)
    {
        if (!on || !_line.Contains(' ')) return;
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


        if (x > LoadFile.xMax) LoadFile.xMax = x;
        if (x < LoadFile.xMin) LoadFile.xMin = x;
        if (y > LoadFile.yMax) LoadFile.yMax = y;
        if (y < LoadFile.yMin) LoadFile.yMin = y;
        if (z > LoadFile.zMax) LoadFile.zMax = z;
        if (z < LoadFile.zMin) LoadFile.zMin = z;
        Camera.main.GetComponent<LoadFile>().vertices.Add(newVertex);
        if (LoadFile.model_code_xrefGCD.Count == 0)
            LoadFile.firstGcdLineInCode = LoadFile.gcdCode.Count - 1;
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
                        //MessageBox.Show("G00");
                        break;
                    case '1':
                        //MessageBox.Show("G01");
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
