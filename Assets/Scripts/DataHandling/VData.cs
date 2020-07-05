using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VData
{
    public float easting { get; set; }
    public float northing { get; set; }
    public float depth { get; set; }
    public float vPvS { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public Vector2d latLon { get; private set; }

    public void SetLatLon()
    {
        string stringLat = lat.ToString();
        string stringLon = lon.ToString();

        string locString = lat + ", " + lon;
        latLon = Conversions.StringToLatLon(locString);
    }

    public Vector3 GetUnityPosition()
    {
        Vector3 unadjustedPos = MapStore.Instance.map.GeoToWorldPosition(this.latLon);
        return AdjustPosForDepth(unadjustedPos);
    }

    public Vector3 AdjustPosForDepth(Vector3 unadjustedPos)
    {
        AbstractMap map = MapStore.Instance.map;
        float currentElevMeter = map.QueryElevationInMetersAt(this.latLon);
        float currentElevUnity = map.QueryElevationInUnityUnitsAt(this.latLon) * map.transform.localScale.y;
        currentElevMeter = currentElevMeter == 0 ? 0.001f : currentElevMeter; // Cheat if necessary
        float ratio = currentElevUnity / currentElevMeter;
        // Convert kilometers to meters => Negative numbers denote being above sea level
        //                              => Positive numbers denote being below sea level
        float depthInMeters = -this.depth * 1000;
        float adjUnityUnits = ratio * depthInMeters;
        float adjElevUnity = unadjustedPos.y + adjUnityUnits - currentElevUnity;
        return new Vector3(unadjustedPos.x, adjElevUnity, unadjustedPos.z);
    }
}
