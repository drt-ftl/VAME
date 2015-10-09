using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slicer : MonoBehaviour
{
    Vector3 scanPosition = new Vector3(0, -1.1f, 0);
    Vector3 iNorm = new Vector3(0f, 1f, 0f);
    public List<LineSegment> lines = new List<LineSegment>();
    public int xSlices = 10;
    public int ySlices = 10;
    public int zSlices = 10;
    private Mesh mesh;
    public Material material;
    public MakeMesh MM;
    private List<int> _tris = new List<int>();
    private List<Vector3> _verts = new List<Vector3>();
    private List<Vector3> _normals = new List<Vector3>();

    void Start ()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        var rnd = GetComponent<MeshRenderer>();
        rnd.material = material;
        rnd.receiveShadows = true;
        rnd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        _tris = new List<int>();
        _verts = new List<Vector3>();
        _normals = new List<Vector3>();
}
	
	void Update ()
    {
	
	}

    public void Slice()
    {
        for (int i = 0; i < MakeMesh.verts.Count; i ++)
        {
            var vert = MakeMesh.verts[i];
            vert.x = Mathf.Round(vert.x * xSlices) / xSlices;
            vert.y = Mathf.Round(vert.y * ySlices) / ySlices;
            vert.z = Mathf.Round(vert.z * zSlices) / zSlices;
            _verts.Add(vert);
            _normals.Add(MakeMesh.normals[i]);
            _tris.Add(MakeMesh.tris[i]);
        }
        mesh.vertices = _verts.ToArray();
        mesh.triangles = _tris.ToArray();
        mesh.normals = _normals.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

    }

    public bool CheckForCross(Vector3[] v)
    {
        var y = scanPosition.y;
        if ((v[0].y <= y || v[1].y <= y || v[2].y <= y) && (v[0].y > y || v[1].y > y || v[2].y > y))
        {
            return true;
        }
        return false;
    }

    public void GetPlane(Vector3[] v)
    {
        var P = v[0];
        var Q = v[1];
        var R = v[2];
        var PQ = Q - P;
        var PR = R - P;
        var RQ = Q - R;
        var n = Vector3.Cross(PQ, PR);
        n = Vector3.Normalize(n);
        var d = Vector3.Dot(n, P);
        var dirVector = Vector3.Cross(n, iNorm);
        dirVector = Vector3.Normalize(dirVector);
        var m = dirVector.z / dirVector.x; // z = mx + b
        var mPQ = PQ.z / PQ.x;
        var mPR = PR.z / PR.x;
        var mRQ = RQ.z / RQ.x;

        if (Mathf.Abs(m) != Mathf.Abs(mPQ)) // Not Parallel
        {

        }
    }
}
