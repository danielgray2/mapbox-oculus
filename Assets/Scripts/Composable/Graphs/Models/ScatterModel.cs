using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterModel : IAbsGraphModel
{
    public float extraMargin { get; set; } = 0.1f;
    public string xName { get; set; }
    public string yName { get; set; }
    public string zName { get; set; }
    public float plotScale { get; set; } = 1f;
    public Vector3 dataPointScale { get; set; } = new Vector3(0.05f, 0.05f, 0.05f);
    public int numMarkersPerAxis { get; set; } = 2;
    public GameObject parent { get; set; }
    public MenuView menuView { get; set; }
}
