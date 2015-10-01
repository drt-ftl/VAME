using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class vTest : MonoBehaviour
{
    private Color thisColor;
    private Color specColor;
    private Color emissColor;
    private VoxelizeSTL2 vs2;
    private int id;

    void Start()
    {
        thisColor = GetComponent<MeshRenderer>().material.color;
        vs2 = Camera.main.GetComponent<VoxelizeSTL2>();
        specColor = GetComponent<MeshRenderer>().material.GetColor("_SpecColor");
        emissColor = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
    }

    public int Id { get; set; }

    void Update()
    {
        if ((int)InspectorR.highlight == Id)
        {
            var color = Color.white;
            GetComponent<MeshRenderer>().material.color = color;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", color);
            Color finalColor = Color.yellow * Mathf.LinearToGammaSpace(1);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
            if (!(GetComponent("Halo") as Behaviour).enabled)
                (GetComponent ("Halo") as Behaviour).enabled = true;
        }
        else
        {
            var color = thisColor;
            color.a = InspectorR.voxelVis / 100;
            GetComponent<MeshRenderer>().material.color = color;
            var spec = specColor;
            spec.a = color.a;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", emissColor);
            if ((GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = false; ;
        }
    }

}
