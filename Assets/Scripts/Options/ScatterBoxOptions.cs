using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Factor out anything that has to do with Unity,
// and put it in the wrapper.
public class ScatterBoxOptions : IOptions
{
    // Not here => Inspector
    public GameObject PointHolder { get; set; }
    // Not here => Inspector
    public GameObject markerParent { get; set; }
    // Not here => Inspector
    public GameObject xAxis { get; set; }
    // Not here => Inspector
    public GameObject yAxis { get; set; }
    // Not here => Inspector
    public GameObject zAxis { get; set; }
    // Not here => Inspector
    public GameObject xLabel { get; set; }
    // Not here => Inspector
    public GameObject yLabel { get; set; }
    // Not here => Inspector
    public GameObject zLabel { get; set; }
}
