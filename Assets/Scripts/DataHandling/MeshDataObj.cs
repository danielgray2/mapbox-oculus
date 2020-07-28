using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDataObj
{
    // Must be value between 0 and 1. I would
    // suggest generating it with an inverse
    // lerp with minVal in data, maxVal in data
    // and this val.
    public float colorValue { get; set; }
    public Vector3 position { get; set; }

    public MeshDataObj(float colorValue, Vector3 position)
    {
        this.colorValue = colorValue;
        this.position = position;
    }
}
