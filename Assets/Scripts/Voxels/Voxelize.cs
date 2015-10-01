using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Voxelize : MonoBehaviour
{
    public GameObject voxel;
    int divisions = 25;
    Vector3 size;
    int i = 0;

    Dictionary<Vector3, GameObject> voxels = new Dictionary<Vector3, GameObject>();

	public void voxelize(List<LineSegment> lines)
    {
        var dim = (float)(1f / divisions);
        size = new Vector3(dim, dim, dim);
        foreach (var line in lines)
        {
            var vector = (line.p2 - line.p1);
            var pLow = line.p1;
            var pHigh = line.p2;
            if (vector.x < 0)
            {
                pLow = line.p2;
                pHigh = line.p1;
                vector = (line.p1 - line.p2);
            }
            var increment = ((pHigh.x - pLow.x) / divisions) * vector;
            var steps = (int)(pHigh.x - pLow.x) * divisions;
            for (int step = 0; step <= steps; step++)
            {
                var pos = pLow + step * increment;
                pos.x = Mathf.Floor(pos.x * divisions);
                pos.y = Mathf.Floor(pos.y * divisions);
                pos.z = Mathf.Floor(pos.z * divisions);
                pos = pos / divisions;
                if (!voxels.ContainsKey(pos))
                {
                    var v = Instantiate(voxel, pos, Quaternion.identity) as GameObject;
                    v.transform.localScale = size;
                    voxels.Add(pos, v);
                }
            }
        }
	}
}
