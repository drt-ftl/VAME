using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurfaceVoxelizer : MonoBehaviour
{

    public GameObject voxel;
    public int divisions = 2;

    public static Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public static List<Vector3> highlights = new List<Vector3>();
    MakeMesh MM;

    public static Mesh mesh;
    public static List<int> tris = new List<int>();
    public static List<Vector3> verts = new List<Vector3>();
    public static List<Vector3> normals = new List<Vector3>();
    private List<Color> colors = new List<Color>();
    private List<Vector2> uvs = new List<Vector2>();

    void Start ()
    {
        MM = GameObject.Find("MESH").GetComponent<MakeMesh>();
	}
	
	public void VoxelizeSurfaces (int divs)
    {
        divisions = divs;
        var Min = (MM.Min) * divisions;
        var Max = (MM.Max) * divisions;
        Min.x = Mathf.Round(Min.x);
        Min.y = Mathf.Round(Min.y);
        Min.z = Mathf.Round(Min.z);
        Max.x = Mathf.Round(Max.x);
        Max.y = Mathf.Round(Max.y);
        Max.z = Mathf.Round(Max.z);
        for (int x = (int)Min.x; x <= Max.x; x++)
        {
            for (int y = (int)Min.y; y <= Max.y; y++)
            {
                for (int z = (int)Min.z; z <= Max.z; z++)
                {
                    var position = new Vector3(x, y, z);
                    var testPosition = position;
                    HandleIt(testPosition, position);

                    testPosition.x += 0.5f;
                    HandleIt(testPosition, position);
                    testPosition.x -= 1.0f;
                    HandleIt(testPosition, position);
                    testPosition.x = position.x;

                    testPosition.y += 0.5f;
                    HandleIt(testPosition, position);
                    testPosition.y -= 1.0f;
                    HandleIt(testPosition, position);
                    testPosition.y = position.y;

                    testPosition.z += 0.5f;
                    HandleIt(testPosition, position);
                    testPosition.z -= 1.0f;
                    HandleIt(testPosition, position);
                    testPosition.z = position.z;
                }
            }
        }
    }

    public void HandleIt (Vector3 testPosition, Vector3 position)
    {
        if (IsPointInside(MM.GetMesh(), testPosition / divisions))
        {
            InstVoxel(position / divisions);
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

    public void InstVoxel(Vector3 pos)
    {
        pos -= MM.centroid;
        if (!voxels.ContainsKey(pos))
        {
            var v = Instantiate(voxel, pos, Quaternion.identity) as GameObject;
            var scale = (1.0f / divisions) * 0.9f;
            v.transform.localScale = new Vector3(scale, scale, scale);
            v.transform.SetParent(GameObject.Find("voxelHolder").transform);
            var color = new Color(pos.x, pos.y, pos.z, 1f);
            var spec = new Color(pos.x, pos.y, pos.z, 1f);
            v.GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            v.GetComponent<Renderer>().material.color = color;
            var vc = new VoxelClass();
            vc.Voxel = v;
            v.GetComponent<vTest>().Id = voxels.Count;
            voxels.Add(pos, vc);
            highlights.Add(pos);
        }
    }

    public void AddCube()
    {

    }
}
