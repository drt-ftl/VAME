using UnityEngine;
using System.Collections;

public class StlInterpreter
{
    private Vector3 Normal;

    public void normal (string _line)
    {
        float x;
        float y;
        float z;
        var split = _line.Split('l');
        split[1].TrimStart(' ');
        var coords = split[1].Split(' ');
        var xString = coords[0];
        var yString = coords[1];
        var zString = coords[2];
        var xStrSplit = xString.Split('e');
        if (float.TryParse(xStrSplit[0], out x))
        {
            float xE;
            if (float.TryParse(xStrSplit[1], out xE))
                x *= (Mathf.Pow(10f, xE));
        }

        var yStrSplit = yString.Split('e');
        if (float.TryParse(yStrSplit[0], out y))
        {
            float yE;
            if (float.TryParse(yStrSplit[1], out yE))
                y *= (Mathf.Pow(10f, yE));
        }

        var zStrSplit = zString.Split('e');
        if (float.TryParse(zStrSplit[0], out z))
        {
            float zE;
            if (float.TryParse(zStrSplit[1], out zE))
                z *= (Mathf.Pow(10f, zE));
        }
        Normal = new Vector3(x, y, z);
    }

    public void outerloop()
    {
        if (LoadFile.model_code_xrefSTL.Count == 0)
            LoadFile.firstStlLineInCode = LoadFile.stlCode.Count - 1;
        LoadFile.model_code_xrefSTL.Add(LoadFile.stlCode.Count - 1);
    }

	public void endloop (string _line, LoadFile _loadFile)
	{
		try
		{
            var mm = GameObject.Find("MESH").GetComponent<MakeMesh>();
            mm.AddTriangle(LoadFile.currentVertices[0], LoadFile.currentVertices[1], LoadFile.currentVertices[2], Normal);
            LoadFile.currentVertices.Clear ();
		}
		catch{}
	}

	public void vertex (string _line)
	{
		{
			float x;
			float y;
			float z;
			var coordSep = _line.Split('x');
			var coords = coordSep[1].TrimStart(' ').Split(' ');
			
			var xString = coords[0];
			var yString = coords[1];
			var zString = coords[2];
			var xStrSplit = xString.Split ('e');
			if (float.TryParse(xStrSplit[0], out x))
			{
				float xE;
				if (float.TryParse(xStrSplit[1], out xE))
					x *= (Mathf.Pow (10f, xE));
			}
			
			var yStrSplit = yString.Split ('e');
			if (float.TryParse(yStrSplit[0], out y))
			{
				float yE;
				if (float.TryParse(yStrSplit[1], out yE))
					y *= (Mathf.Pow (10f, yE));
			}
			
			var zStrSplit = zString.Split ('e');
			if (float.TryParse(zStrSplit[0], out z))
			{
				float zE;
				if (float.TryParse(zStrSplit[1], out zE))
					z *= (Mathf.Pow (10f, zE));
			}
			var newVertex = new Vector3 (x,y,z) * LoadFile.stlScale;
			if (x > LoadFile.xMax) LoadFile.xMax = x;
			if (x < LoadFile.xMin) LoadFile.xMin = x;
			if (y > LoadFile.yMax) LoadFile.yMax = y;
			if (y < LoadFile.yMin) LoadFile.yMin = y;
			if (z > LoadFile.zMax) LoadFile.zMax = z;
			if (z < LoadFile.zMin) LoadFile.zMin = z;
			
			
			LoadFile.currentVertices.Add (newVertex);
		}
	}
}
