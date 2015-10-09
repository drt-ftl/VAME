using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleCube : MonoBehaviour
{
    Mesh mesh;
    private List<int> triangles = new List<int>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Color> colors = new List<Color>();
    private List<Vector2> uvs = new List<Vector2>();
    public Material material;
    private int divisions;
    private Color thisColor;
    private Color specColor;
    private Color emissColor;
    private int id;
    public static float scale = 0.9f;


    void Start()
    {
        thisColor = GetComponent<MeshRenderer>().material.color;
        specColor = GetComponent<MeshRenderer>().material.GetColor("_SpecColor");
        emissColor = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
    }
    public int Id { get; set; }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if ((int)InspectorR.highlight == Id)
        {
            var color = Color.white;
            GetComponent<MeshRenderer>().material.color = color;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", color);
            Color finalColor = Color.yellow * Mathf.LinearToGammaSpace(1);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
            if (!(GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = true;
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            var color = Color.white;
            GetComponent<MeshRenderer>().material.color = color;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", color);
            Color finalColor = Color.green * Mathf.LinearToGammaSpace(1);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
            if (!(GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                InspectorR.highlight = Id;
            }
        }
        else
        {
            var color = thisColor;
            var spec = specColor;
            var emiss = emissColor;
            var halo = false;
            switch (InspectorR.highlightType)
            {
                case InspectorR.HighlighType.PathDensity:
                    if (MeshVoxelizer.voxels[MeshVoxelizer.highlights[Id]].IntersectedByLines.Count >= InspectorR.minPathIntersects
                && MeshVoxelizer.voxels[MeshVoxelizer.highlights[Id]].IntersectedByLines.Count <= InspectorR.maxPathIntersects)
                    {
                        color = Color.white;
                        spec = color;
                        emiss = Color.magenta * Mathf.LinearToGammaSpace(1);
                        halo = true;
                    }
                    else
                    {
                        color = thisColor;
                        spec = specColor;
                        emiss = emissColor;
                        color.a = InspectorR.voxelVis / 100;
                    }
                    break;
                case InspectorR.HighlighType.PathNumbers:
                    if (MeshVoxelizer.voxels[MeshVoxelizer.highlights[Id]].IntersectedByLines.Contains(LoadFile.gcdLines[(int)InspectorL.gcdTimeSlider]))
                    {
                        color = Color.white;
                        spec = color;
                        emiss = Color.magenta * Mathf.LinearToGammaSpace(1);
                        halo = true;
                    }
                    else
                    {
                        color = thisColor;
                        spec = specColor;
                        emiss = emissColor;
                        color.a = InspectorR.voxelVis / 100;
                    }
                    break;
                default:
                    color = thisColor;
                    spec = specColor;
                    emiss = emissColor;
                    color.a = InspectorR.voxelVis / 100;
                    break;
            }
            GetComponent<MeshRenderer>().material.color = color;
            spec.a = color.a;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", emiss);
            if ((GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = false;
        }
    }

    public void Voxel ()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        var rnd = GetComponent<MeshRenderer>();
        rnd.material = material;
        rnd.receiveShadows = true;
        rnd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        AddCube();
    }
    public void AddCube()
    {
        var position = Vector3.zero;
        var half = new Vector3(scale / 2, scale / 2, scale / 2);
        var startPosition = position;
        var Plus = position + half;
        var Minus = position - half;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;
        Vector3 norm;

        position = Minus;

        position.x = Plus.x;
        p1 = position;
        position.x = Minus.x;
        p2 = position;
        position.y = Plus.y;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.x = Plus.x;
        p2 = position;
        position.y = Minus.y;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.y = Plus.y;
        p2 = position;
        position.z = Plus.z;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.y = Minus.y;
        p2 = position;
        position.z = Minus.z;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.z = Plus.z;
        p2 = position;
        position.x = Minus.x;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.z = Minus.z;
        p2 = position;
        position.x = Plus.x;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        //This One
        position = Plus;
        p1 = position;
        position.y = Minus.y;
        p2 = position;
        position.x = Minus.x;
        p3 = position;
        norm = Normal(p1, p3, p2);
        AddTriangle(p1, p3, p2, norm);

        //This One
        p1 = position;
        position.y = Plus.y;
        p2 = position;
        position.x = Plus.x;
        p3 = position;
        norm = Normal(p1, p3, p2);
        AddTriangle(p1, p3, p2, norm);

        p1 = position;
        position.z = Minus.z;
        p2 = position;
        position.x = Minus.x;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.z = Plus.z;
        p2 = position;
        position.x = Plus.x;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        position = Minus;
        p1 = position;
        position.z = Plus.z;
        p2 = position;
        position.y = Plus.y;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        p1 = position;
        position.z = Minus.z;
        p2 = position;
        position.y = Minus.y;
        p3 = position;
        norm = Normal(p1, p2, p3);
        AddTriangle(p1, p2, p3, norm);

        MergeMesh();
    }

    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 norm)
    {
        var count = triangles.Count;

        triangles.Add(count);
        triangles.Add(count + 1);
        triangles.Add(count + 2);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        uvs.Add(p1);
        uvs.Add(p2);
        uvs.Add(p3);
        for (int i = 0; i < 3; i++)
            normals.Add(norm);
        for (int i = 0; i < 3; i++)
            colors.Add(InspectorL.stlColor);
    }

    public Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var dir = Vector3.Cross(p2 - p1, p3 - p1);
        var norm = Vector3.Normalize(dir);
        return norm;
    }

    public void MergeMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.colors = colors.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();
    }
}
