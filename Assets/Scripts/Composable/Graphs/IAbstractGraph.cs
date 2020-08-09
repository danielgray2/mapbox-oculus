using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractGraph : IGraph
{
    public IModel model { get; set; }
    public Vector3 minDpSize { get; set; }
    public Vector3 maxDpSize { get; set; }
    public DataObj dataObj { get; set; }
}
