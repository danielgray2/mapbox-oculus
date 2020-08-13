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

    public ComposableModel(ComposableType compType)
    {
        this.compType = compType;
    }
}
