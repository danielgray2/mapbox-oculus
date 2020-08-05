using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraph
{
    void Plot();
    public DataObj dataObj { get; set; }
    public List<GameObject> dataPoints { get; set; }
    public Vector3 maxDpScale { get; set; }
    public Vector3 minDpScale { get; set; }
    public IOptions options { get; set; }
}