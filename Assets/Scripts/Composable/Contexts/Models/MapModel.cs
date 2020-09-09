using Mapbox.Unity.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapModel : IAbsContextModel
{
    public string latColName { get; set; }
    public string lonColName { get; set; }
    public string depthColName { get; set; }
    public float iconSize { get; set; } = 5;
    public AbstractMap absMap { get; set; }
    public int zoom { get; set; } = 13;
    public bool snapMapToZero { get; set; } = true;
    public float unityTileSize { get; set; } = 1f;
    public int exaggerationFactor { get; set; } = 1;
    public MapOptions mO { get; set; }
    public ImagerySourceType imagerySourceType { get; set; } = ImagerySourceType.MapboxSatellite;
    public ElevationSourceType mapboxTerrain { get; set; } = ElevationSourceType.MapboxTerrain;
    public ElevationLayerType elevationLayerType { get; set; } = ElevationLayerType.TerrainWithElevation;
    public MapPlacementType mapPlacementType { get; set; } = MapPlacementType.AtLocationCenter;
    public MapExtentType mapExtentType { get; set; } = MapExtentType.Custom;
    public Vector3 maxDpSize { get; set; }
    public Vector3 minDpSize { get; set; }

    public MapModel()
    {
        availTransfs.Add(DepthLatLonTransf.friendlyName);
        availTransfs.Add(LatLonTransf.friendlyName);
    }
}
