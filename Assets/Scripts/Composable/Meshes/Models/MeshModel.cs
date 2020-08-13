using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModel : IAbsModel
{
    public ComposableModel compModel { get; set; }
    public string yCol { get; set; }
    public string xCol { get; set; }
    public string zCol { get; set; }
    public string valueCol { get; set; }
    public List<MeshVizLayer> meshLayerList { get; set; }
    public MenuView menuView { get; set; }

    public MeshModel(ComposableModel compModel)
    {
        this.compModel = compModel;
    }
}
