using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointOptions : IOptions 
{ 
    public Vector3 size { get; set; }
    public Color color { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 position { get; set; }
    public GameObject parent { get; set; }
}
