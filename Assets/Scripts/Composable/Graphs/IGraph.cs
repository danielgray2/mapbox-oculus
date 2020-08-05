using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraph
{
    public DataObj dataObj { get; set; }
    public Vector3 minDpSize { get; set; }
    public Vector3 maxDpSize { get; set; }
    public void Plot();
}