using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraph
{
    DataObj dataObj { get; set; }
    Vector3 minDpSize { get; set; }
    Vector3 maxDpSize { get; set; }
}