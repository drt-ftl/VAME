using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathVoxelizer : MonoBehaviour
{

    public GameObject voxel;
    public int divisions = 2;

    public Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public List<Vector3> highlights = new List<Vector3>();
    GcdInterpreter gcdI;

    void Start()
    {
        gcdI = LoadFile.gcdInterpreter;
    }

    public static int round(float value)
    {
        // Add or subtract 0.5 depending on whether the value is positive
        // or negative, and truncate the result.
        return Mathf.RoundToInt(value);
        //return (int)(value + 0.5f * Mathf.Sign(value));
    }

    public static float roundf(float value)
    {
        // Add or subtract 0.5 depending on whether the value is positive
        // or negative, and truncate the result.
        return Mathf.Round(value);
        //return (int)(value + 0.5f * Mathf.Sign(value));
    }

    public void MakeNew(Vector3 pos, LineSegment line)
    {
        pos = pos / divisions;
        if (!voxels.ContainsKey(pos))
        {
            var vc = new VoxelClass();
            vc.Voxel = Instantiate(voxel, pos, Quaternion.identity) as GameObject;
            var sc = (1.0f / divisions) * 0.9f;
            vc.Voxel.transform.localScale = new Vector3(sc, sc, sc);
            vc.Voxel.GetComponent<vTest>().Id = voxels.Count;
            vc.IntersectedByLines = new List<LineSegment>();
            voxels.Add(pos, vc);
            highlights.Add(pos);
        }
        voxels[pos].IntersectedByLines.Add(line);
    }

    public void VoxelizePaths(int d)
    {
        divisions = d;
        foreach (var l in LoadFile.gcdLines)
        {
            var p1 = l.p1 * divisions;
            var p2 = l.p2 * divisions;
            if (p1.x == p2.x && p1.z != p2.z && p1.y == p2.y) // travels along z, stationary in x and y
            {
                if (p1.z < p2.z)
                {
                    for (int z = round(p1.z); z <= round(p2.z); z++)
                    {
                        var pos = new Vector3(round(p1.x), round(p1.y), z);
                        MakeNew(pos, l);
                    }
                }

                else
                {
                    for (int z = round(p2.z); z <= round(p1.z); z++)
                    {
                        var pos = new Vector3(round(p1.x), round(p1.y), z);
                        MakeNew(pos, l);
                    }
                }
            }
            else if (p1.z == p2.z && p1.x != p2.x && p1.y == p2.y) // travels along x, stationary in z and y
            {
                if (p1.x < p2.x)
                {
                    for (int x = round(p1.x); x <= round(p2.x); x++)
                    {
                        var pos = new Vector3(x, round(p1.y), round(p1.z));
                        MakeNew(pos, l);
                    }
                }

                else
                {
                    for (int x = round(p2.x); x <= round(p1.x); x++)
                    {
                        var pos = new Vector3(x, roundf(p1.y), roundf(p1.z));
                        MakeNew(pos, l);
                    }
                }
            }
        }
    }

    public void VoxelizePaths2 (int d)
    {
        divisions = d;
        var emptyVoxels = new List<Vector3>();
        var s = 2.0f;
        s *= divisions;
        var Min = -Vector3.one * s;
        var Max = Vector3.one * s;

        for (int x = (int)Min.x; x <= Max.x; x++)
        {
            for (int y = (int)Min.y; y <= Max.y; y++)
            {
                for (int z = (int)Min.z; z <= Max.z; z++)
                {
                    emptyVoxels.Add(new Vector3((float)x, (float)y, (float)z));
                }
            }
        }
        foreach (var line in LoadFile.gcdLines)
        {
            foreach (var slot in emptyVoxels)
            {
                CheckPaths(line, slot);
            }
        }
    }

    public void CheckPaths(LineSegment line, Vector3 slot)
    {
        var p1 = line.p1 * divisions;
        var p2 = line.p2 * divisions;
        var th = 0.01f;
        th += 0.5f;
        if (p1.x == p2.x && p1.z != p2.z && p1.y == p2.y) // travels along z, stationary in x and y
        {
            var line2d = new Vector2(p1.x, p1.y);
            var slot2d = new Vector2(slot.x, slot.y);
            if (Vector2.Distance(line2d, slot2d) <= th)
            {
                if (p1.z < p2.z && p1.z <= slot.z - th && p2.z >= slot.z + th)
                    MakeNew(slot, line);
            }
        }


        else if (p1.z == p2.z && p1.x != p2.x && p1.y == p2.y) // travels along x, stationary in z and y
        {
            var line2d = new Vector2(p1.y, p1.z);
            var slot2d = new Vector2(slot.y, slot.z);
            if (Vector2.Distance(line2d, slot2d) <= th)
                if (p1.x < p2.x && p1.x <= slot.x - th && p2.x >= slot.x + th)
                    MakeNew(slot, line);
        }
    }
}
