﻿using UnityEngine;
using System.Collections;

public class cctPointScript : MonoBehaviour
{
    private Color thisColor;
    private Color specColor;
    private Color emissColor;
    private Color blank;
    private bool go = false;
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
    }
    public int Id { get; set; }
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
            //color.a = InspectorR.voxelVis / 100;
            if (color.a <= 0.1f && GetComponent<SphereCollider>().enabled)
                GetComponent<SphereCollider>().enabled = false;
            else if (color.a > 0.1f && !GetComponent<SphereCollider>().enabled)
                GetComponent<SphereCollider>().enabled = true;
            GetComponent<MeshRenderer>().material.color = color;
            spec.a = color.a;
            GetComponent<MeshRenderer>().material.SetColor("_SpecColor", spec);
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", emiss);
            if ((GetComponent("Halo") as Behaviour).enabled)
                (GetComponent("Halo") as Behaviour).enabled = false;
        }
    }

}