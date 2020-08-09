using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistModel : IModel
{
    public ComposableModel compModel { get; set; }
    public float extraMargin { get; set; } = 0.1f;
    public string xName { get; set; }
    public float plotScale { get; set; } = 1f;
    public float offset { get; set; } = 0.1f;
    public float minBin { get; set; }
    public float maxBin { get; set; }
    public float binWidth { get; set; }
    public int numBins { get; set; }
    public float normedWidth { get; set; }
    public float xMax { get; set; }
    public float xMin { get; set; }
    public int numMarkersPerAxis { get; set; } = 2;
    public GameObject parent { get; set; }
    public HistModel(ComposableModel compModel)
    {
        this.compModel = compModel;
    }
}
