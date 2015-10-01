using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelizeSTL2 : MonoBehaviour
{
    public GameObject voxel;
    public int divisions = 2;
    Vector3 size;
    int i = 0;

    public Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public List<Vector3> highlights = new List<Vector3>();
    public static float finalDim;
    public static float offset = 1;
    Vector3 MaxGrid;
    Vector3 MinGrid;
    Vector3 c;
    float f;
    float dim;
    public Transform centroidTransform;

    void Start()
    {
        
    }

    public void voxelize()
    {
        dim = 1 / (float)divisions;
        var Min = new Vector3(LoadFile.xMin, LoadFile.yMin, LoadFile.zMin);
        var Max = new Vector3(LoadFile.xMax, LoadFile.yMax, LoadFile.zMax);
        c = (Min + Max) / 2;

        MinGrid = Min;
        MaxGrid = Max;

        MinGrid.x = Mathf.Round(Min.x * divisions);
        MinGrid.y = Mathf.Round(Min.y * divisions);
        MinGrid.z = Mathf.Round(Min.z * divisions);
        MinGrid /= divisions;

        MaxGrid.x = Mathf.Round(Max.x * divisions);
        MaxGrid.y = Mathf.Round(Max.y * divisions);
        MaxGrid.z = Mathf.Round(Max.z * divisions);
        MaxGrid /= divisions;

        centroidTransform.transform.position = c;
        f = (Max.x - Min.x) / (MaxGrid.x - MinGrid.x);

        Fill();      
    }

    public void InstVoxel(Vector3 hitSpot, Vector3 dir)
    {
        var pos = hitSpot;
        pos.x = Mathf.Round(hitSpot.x * divisions) / divisions;
        pos.y = Mathf.Round(hitSpot.y * divisions) / divisions;
        pos.z = Mathf.Round(hitSpot.z * divisions) / divisions;
        var scale = f / (float)divisions;
        finalDim = scale;
        pos.x = c.x + (c.x - pos.x) * f + scale / 2;
        pos.y = c.y + (c.y - pos.y) * f + scale / 2;
        pos.z = c.z + (c.z - pos.z) * f + scale / 2;
        if (pos.x % 1 < offset)
            offset = pos.x % 1;
        if (!voxels.ContainsKey(pos))
        {
            var v = Instantiate(voxel, pos, Quaternion.identity) as GameObject;
            v.transform.localScale = new Vector3(scale, scale, scale);
            v.transform.SetParent(GameObject.Find("voxelHolder").transform);
            var color = new Color(hitSpot.x, hitSpot.y, hitSpot.z, 1f);
            var spec = new Color(hitSpot.x, hitSpot.y, hitSpot.z, 1f);
            v.GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            v.GetComponent<Renderer>().material.color = color;
            var vc = new VoxelClass();
            vc.Voxel = v;
            vc.ScanDirection = dir;
            v.GetComponent<vTest>().Id = voxels.Count;
            voxels.Add(pos, vc);
            highlights.Add(pos);
        }
    }

    public void Fill()
    {
        var meshObject = GameObject.Find("MESH");
        var mesh = meshObject.GetComponent<MeshFilter>().mesh;
        foreach (var t in mesh.triangles)
        {
            mesh.vertices[t] = mesh.vertices[t] * 2f;
        }
        for (float y = MaxGrid.y + dim; y >= MinGrid.y - dim; y -= dim)
        {
            for (float z = MaxGrid.z + dim; z >= MinGrid.z - dim; z -= dim)
            {
                for (float x = MaxGrid.x + dim; x >= MinGrid.x - dim; x -= dim)
                {
                    if (IsPointInside(mesh, new Vector3(x, y, z)))
                    {
                        if (x!=0 && y!=0 && z!=0)
                        InstVoxel(new Vector3(x, y, z), Vector3.zero);
                    }
                }
            }
        }
    }

    public bool IsPointInside(Mesh aMesh, Vector3 aLocalPoint)
    {
        var verts = aMesh.vertices;
        var tris = aMesh.triangles;
        int triangleCount = tris.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            var V1 = verts[tris[i * 3]];
            var V2 = verts[tris[i * 3 + 1]];
            var V3 = verts[tris[i * 3 + 2]];
            var P = new Plane(V1, V2, V3);
            if (P.GetSide(aLocalPoint))
                return false;
        }
        return true;
    }
}

