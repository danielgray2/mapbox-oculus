using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractGraph : IGraph
{
    public DataObj dataObj { get; set; }
    public List<GameObject> dataPoints { get; set; }
    public Vector3 maxDpScale { get; set; }
    public Vector3 minDpScale { get; set; }
    public IOptions options { get; set; }

    public abstract void Plot();
}
