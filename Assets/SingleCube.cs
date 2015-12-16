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
    private Color blank;
    private int id;
    public static float scale = 0.9f;


    void Start()
    {
        thisColor = GetComponent<MeshRenderer>().material.color;
        specColor = GetComponent<MeshRenderer>().material.GetColor("_SpecColor");
        emissColor = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        blank = new Color(0, 0, 0, 0);
        //Origin = Vector3.zero;
    }
    public int Id { get; set; }
    public Vector3 Origin { get; set; }
    public VoxelClass voxelClass { get; set; }
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
                InspectorR.highlightVector = Origin;
                var sloxels = voxelClass.Sloxels;
                if (sloxels.Count > 0)
                {
                    var sloxel = sloxels[0];
                    if (cSectionGCD.layerHeights.Contains(sloxel.Position.y))
                    {
                        InspectorT.slicerForm.LayerTrackbar.Value = cSectionGCD.layerHeights.IndexOf(sloxel.Position.y);
                        InspectorT.slicerForm.LayerUpDown.Value = cSectionGCD.layerHeights.IndexOf(sloxel.Position.y);
                        if (cSectionGCD.layers[sloxel.Position.y].Sloxels.Contains(voxelClass.Sloxels[0]))
                        {
                            InspectorT.slicerForm.SloxelNumber.Maximum = cSectionGCD.layers[sloxel.Position.y].Sloxels.Count;
                            InspectorT.slicerForm.SloxelNumber.Value = cSectionGCD.layers[sloxel.Position.y].Sloxels.IndexOf(voxelClass.Sloxels[0]);
                            if (InspectorR.highlightType != InspectorR.HighlighType.PathNumbers)
                                InspectorL.gcdTimeSlider = InspectorT.slicerForm.LayerTrackbar.Value;
                        }
                        InspectorT.slicerForm.panel1.Invalidate();
                    }
                }
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
                    if (cSectionGCD.voxels[cSectionGCD.highlights[Id]].IntersectedByLines.Count >= InspectorR.minPathIntersects
                && cSectionGCD.voxels[cSectionGCD.highlights[Id]].IntersectedByLines.Count <= InspectorR.maxPathIntersects)
                    {
                        color = Color.white;
                        spec = color;
                        emiss = Color.magenta * Mathf.LinearToGammaSpace(0.2f);
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
                    var standard = true;
                    if ((voxelClass.MinLine > InspectorL.gcdTimeSliderMin
                        && voxelClass.MinLine < InspectorL.gcdTimeSlider)
                        || (voxelClass.MaxLine > InspectorL.gcdTimeSliderMin
                        && voxelClass.MaxLine < InspectorL.gcdTimeSlider)
                        && voxelClass.MaxLine >= 0
                        && voxelClass.MinLine < 10000000)
                        standard = false;
                    if (!standard)
                    {
                        color = Color.white;
                        spec = color;
                        emiss = Color.blue * Mathf.LinearToGammaSpace(0.2f);
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
                case InspectorR.HighlighType.PathSeparation:
                    var stand = true;
                    if ((voxelClass.MinDistance >= InspectorR.minIntDist
                        && voxelClass.MinDistance <= InspectorR.maxIntDist)
                        || (voxelClass.MaxDistance >= InspectorR.minIntDist
                        && voxelClass.MaxDistance <= InspectorR.maxIntDist))
                        stand = false;
                    if (!stand)
                    {
                        color = Color.white;
                        spec = color;
                        emiss = Color.blue * Mathf.LinearToGammaSpace(0.2f);
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
            if (color.a <= 0.1f && GetComponent<BoxCollider>().enabled)
                GetComponent<BoxCollider>().enabled = false;
            else if (color.a > 0.1f && !GetComponent<BoxCollider>().enabled)
                GetComponent<BoxCollider>().enabled = true;
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
            colors.Add(Camera.main.GetComponent<InspectorR>().voxelColor);
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
