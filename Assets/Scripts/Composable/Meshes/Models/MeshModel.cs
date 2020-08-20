using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModel : IAbsMeshModel
{
    public string yCol { get; set; }
    public string xCol { get; set; }
    public string zCol { get; set; }
    public string valueCol { get; set; }
    public List<MeshVizLayer> meshLayerList { get; set; }
    public MenuView menuView { get; set; }
}
