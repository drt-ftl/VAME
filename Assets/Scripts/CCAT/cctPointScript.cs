using UnityEngine;
using System.Collections;


public class cctPointScript : MonoBehaviour
{
    private Color thisColor;
    private Color specColor;
    private Color emissColor;
    private Color blank;
    private bool go = false;
    private enum CctMode { Color,Hide, None}
    private CctMode cctMode;
    public bool partOfHighlightedSet = false;

    void Start()
    {
        thisColor = GetComponent<MeshRenderer>().material.color;
        specColor = GetComponent<MeshRenderer>().material.GetColor("_SpecColor");
        emissColor =  GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        blank = new Color(0, 0, 0, 0);
    }

    public void Go()
    {
        go = true;
        cctMode = CctMode.Color;
    }
    public int Id { get; set; }

    public float Temperature { get; set; }
    public Vector3 ErrorDisplacement
    {
        get
        {
            var predicted = LoadFile.gcdPointVerts[Id].Position;
            var actual = transform.position;
            return (predicted - actual);
        }
    }
    public float ErrorDistance
    {
        get
        {
            return (Vector3.Magnitude(ErrorDisplacement));
        }
    }

    public LineSegment Line { get; set; }
    public float t { get; set; }

    public void SetColor (Color col)
    {
        thisColor = col;
        emissColor = col * 0.5f;
        GetComponent<MeshRenderer>().material.color = thisColor;
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", emissColor);
    }
    public Color GetColor()
    {
        return thisColor;
    }

    public bool LaserOn { get; set; }

    void Update()
    {
        if (!go) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (LoadFile.cctHighlight == Id)
        {
            var color = Color.white;
            GetComponent<MeshRenderer>().material.color = color;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", color);
            Color finalColor = Color.yellow * Mathf.LinearToGammaSpace(1);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
            if (!(GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = true;
            var predicted = LoadFile.gcdPointVerts[Id].Position;
            var actual = transform.position;
            var txt = "";
            txt += "Point: " + Id.ToString() + "\r\n";
            txt += "Predicted: " + predicted.ToString("f3") + "\r\n";
            txt += "Actual: " + actual.ToString("f3") + "\r\n";
            txt += "Delta: " + (predicted - actual).ToString("f3") + "\r\n";
            txt += "Distance: " + Vector3.Distance(predicted, actual).ToString("f3") + "\r\n";
            txt += "Temperature: " + Temperature.ToString ("f2");
            LoadFile.ccatExplorer.label1.Text = txt;
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
                LoadFile.cctHighlight = Id;
            }
        }
        else
        {
            var color = thisColor;
            var spec = specColor;
            var emiss = emissColor;
            var halo = false;
            switch (cctMode)
            {
                case CctMode.Color:
                    if (Temperature < 200 && !LoadFile.ccatExplorer.showWhenLaserOff.Checked)
                    {
                        color = new Color(0, 0, 0, 0);
                        spec = color;
                        emiss = color;
                        halo = false;
                    }
                    else if (ErrorDistance < LoadFile.cctErrorThreshold)
                    {
                        color = thisColor;
                        spec = specColor;
                        emiss = emissColor;
                        color.a = LoadFile.cctVis;
                        spec.a = LoadFile.cctVis;
                        emiss.a = LoadFile.cctVis;
                        halo = false;
                        partOfHighlightedSet = false;
                    }
                    else
                    {
                        color = thisColor;
                        spec = specColor;
                        emiss = emissColor;
                        halo = true;
                        partOfHighlightedSet = true;
                    }
                    break;
                case CctMode.Hide:
                    if (Temperature < 200 && !LoadFile.ccatExplorer.showWhenLaserOff.Checked)
                    {
                        color = new Color(0, 0, 0, 0);
                        spec = color;
                        emiss = color;
                        halo = false;
                    }
                    else if (ErrorDistance < LoadFile.cctErrorThreshold)
                    {
                        color = blank;
                        spec = color;
                        emiss = blank;
                        halo = false;
                        partOfHighlightedSet = false;
                    }
                    else
                    {
                        color = thisColor;
                        spec = specColor;
                        emiss = emissColor;
                        color.a = LoadFile.cctVis;
                        spec.a = LoadFile.cctVis;
                        emiss.a = LoadFile.cctVis;
                        halo = true;
                        partOfHighlightedSet = true;
                    }
                    break;
                default:
                    color = thisColor;
                    spec = specColor;
                    emiss = emissColor;
                    color.a = (float)LoadFile.ccatExplorer.Visibility.Value / 100f;
                    partOfHighlightedSet = false;
                    break;
            }
            if (color.a <= 0.1f && GetComponent<SphereCollider>().enabled)
                GetComponent<SphereCollider>().enabled = false;
            else if (color.a > 0.1f && !GetComponent<SphereCollider>().enabled)
                GetComponent<SphereCollider>().enabled = true;
            GetComponent<MeshRenderer>().material.color = color;
            spec.a = color.a;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", emiss);
            if (halo) return;
            if ((GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = false;
        }
    }

}
