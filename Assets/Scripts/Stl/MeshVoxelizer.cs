using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshVoxelizer : MonoBehaviour
{

    public GameObject voxel;
    public int divisions = 2;
    public Material material;
    public Color color;

    public static Dictionary<Vector3, VoxelClass> voxels = new Dictionary<Vector3, VoxelClass>();
    public static List<Vector3> highlights = new List<Vector3>();
    MakeMesh MM;

    public static Mesh mesh;
    public static List<int> triangles = new List<int>();
    public static List<Vector3> vertices = new List<Vector3>();
    public static List<Vector3> normals = new List<Vector3>();
    public static List<Color> colors = new List<Color>();
    public static List<Vector2> uvs = new List<Vector2>();
    int d = 0;
    public static Vector3 Min { get; set; }
    public static Vector3 Max { get; set; }

    void Start()
    {
        MM = GameObject.Find("MESH").GetComponent<MakeMesh>();
        Min = new Vector3(1000, 1000, 1000);
        Max = new Vector3(-1000, -1000, -1000);
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        var rnd = GetComponent<MeshRenderer>();
        rnd.material = material;
        rnd.receiveShadows = true;
        rnd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void VoxelizeSurfaces(int divs)
    {
        divisions = divs;
        var Min = (MM.Min - MM.centroid) * divisions;
        var Max = (MM.Max - MM.centroid) * divisions;
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

    public void HandleIt(Vector3 testPosition, Vector3 position)
    {
        var coords = (position / divisions);
        if (IsPointInside(MM.GetMesh(), testPosition / divisions) && !voxels.ContainsKey(coords))
        {
            var v = Instantiate(voxel, coords, Quaternion.identity) as GameObject;
            v.transform.SetParent(transform);
            v.GetComponent<SingleCube>().Voxel();
            var scale = 1.0f / divisions;
            v.transform.localScale = new Vector3(scale, scale, scale);
            var small = SingleCube.scale;
            v.GetComponent<BoxCollider>().size = new Vector3(small, small, small);
            var vc = new VoxelClass();
            vc.Voxel = v;
            v.GetComponent<SingleCube>().Id = voxels.Count;
            voxels.Add(coords, vc);
            highlights.Add(coords);
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

    public static void ClearAll()
    {
        mesh.Clear();
        triangles.Clear();
        vertices.Clear();
        normals.Clear();
        colors.Clear();
        Min = new Vector3(1000, 1000, 1000);
        Max = new Vector3(-1000, -1000, -1000);
    }
}
