using Mapbox.Unity.Map;
using Mapbox.Unity.Map.Strategies;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapContextWrapper : MonoBehaviour, IGraph
{
    [SerializeField]
    public Texture2D loadingTexture;

    [SerializeField]
    public Material tileMaterial;
}