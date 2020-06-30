using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PointPositionHandler : MonoBehaviour
{
    public enum DEPTH_REP {DEPTH, GRADIENT};
    public DEPTH_REP currSetting = DEPTH_REP.DEPTH;
    void Update()
    {
        if(currSetting == DEPTH_REP.DEPTH)
        {
            foreach (GameObject go in MapStore.Instance.iconGOs)
            {
                UpdateLocation(go);
                UpdateScale(go);
            }
        }
        else if(currSetting == DEPTH_REP.GRADIENT)
        {
            foreach (GameObject go in MapStore.Instance.iconGOs)
            {
                UpdateScale(go);
            }
        }
    }

    void UpdateLocation(GameObject go)
    {
        SData sData = go.GetComponent<LerpAnimation>().sData;
        Vector3 unadjustedPos = MapStore.Instance.map.GeoToWorldPosition(sData.latLon, true);
        Vector3 adjustedPos = go.GetComponent<LerpAnimation>().sData.AdjustPosForDepth(unadjustedPos);
        go.transform.localPosition = adjustedPos;
    }

    void UpdateScale(GameObject go)
    {
        int numDecimals = 3;
        float newSizeMultiplier = MapStore.Instance.map.UnityTileSize * MapStore.Instance.map.transform.localScale.x / MapStore.Instance.iconSize;
        float newSizeMultiplierRounded = (float)Math.Round(newSizeMultiplier, numDecimals);
        float currSizeMultiplier = go.GetComponent<LerpAnimation>().sizeMultiplier;
        float currSizeMultiplierRounded = (float)Math.Round(currSizeMultiplier, numDecimals);
        if (!Mathf.Approximately(newSizeMultiplierRounded, currSizeMultiplierRounded))
        {
            go.GetComponent<LerpAnimation>().UpdateLargestSize(newSizeMultiplier);
            Vector3 intermediate = go.transform.localScale / currSizeMultiplier;
            go.transform.localScale = intermediate * newSizeMultiplier;
        }
    }
}