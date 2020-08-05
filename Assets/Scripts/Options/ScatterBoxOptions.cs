using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Factor out anything that has to do with Unity,
// and put it in the wrapper.
public class ScatterBoxOptions : IOptions
{
    public DataObj dataObj { get; set; }
    public GameObject dataPointPrefab { get; set; }
    public GameObject markerPrefab { get; set; }
    public float extraMargin { get; set; } = 0.1f;
    public string xName { get; set; }
    public string yName { get; set; }
    public string zName { get; set; }
    public float plotScale { get; set; } = 1f;
    public Vector3 dataPointScale { get; set; } = new Vector3(0.05f, 0.05f, 0.05f);
    public GameObject PointHolder { get; set; }
    public GameObject markerParent { get; set; }
    public GameObject xAxis { get; set; }
    public GameObject yAxis { get; set; }
    public GameObject zAxis { get; set; }
    public GameObject xLabel { get; set; }
    public GameObject yLabel { get; set; }
    public GameObject zLabel { get; set; }
    public int numMarkersPerAxis { get; set; }
}
