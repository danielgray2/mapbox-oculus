using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractGraph : IGraph
{
    public DataObj dataObj { get; set; }
    public Vector3 minDpSize { get; set; }
    public Vector3 maxDpSize { get; set; }

    public abstract void Plot() { }
}
