using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComposableType
{
    GRAPH,
    CONTEXT,
    DASHBOARD,
    MESH
}
public class ComposableModel : IAbsModel
{
    public DataObj dataObj { get; set; }
    public ComposableType compType { get; set; }
    public List<IAbsTransf> transformations { get; set; }
    public DataPointOptions dataPointOptions { get; set; }
    public List<IComposable> subComps { get; set; }
    public IComposable superComp { get; set; }
    public bool beingCreated { get; set; }

    public ComposableModel()
    {
        beingCreated = true;
    }
}
