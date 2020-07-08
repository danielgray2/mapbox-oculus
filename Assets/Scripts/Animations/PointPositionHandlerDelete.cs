using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PointPositionHandlerDelete : MonoBehaviour
{
    public enum DEPTH_REP { DEPTH, GRADIENT };
    public DEPTH_REP currSetting = DEPTH_REP.GRADIENT;
    void Update()
    {
        if (currSetting == DEPTH_REP.DEPTH)
        {
            foreach (GameObject go in MapStore.Instance.secondaryIconGOs)
            {
                UpdateLocation(go);
                UpdateScale(go);
            }
        }
        else if (currSetting == DEPTH_REP.GRADIENT)
        {
            foreach (GameObject go in MapStore.Instance.secondaryIconGOs)
            {
                UpdateLocationNoDepth(go);
                UpdateScale(go);
            }
        }
    }

    void UpdateLocation(GameObject go)
    {
        SData sData = go.GetComponent<LerpAnimationDelete>().sData;
        Vector3 unadjustedPos = MapStore.Instance.secondaryMap.GeoToWorldPosition(sData.latLon, true);
        Debug.Log("Here is the pos: " + unadjustedPos);
        Vector3 adjustedPos = go.GetComponent<LerpAnimationDelete>().sData.AdjustPosForDepth(unadjustedPos);
        go.transform.localPosition = adjustedPos;
    }

    void UpdateLocationNoDepth(GameObject go)
    {
        SData sData = go.GetComponent<LerpAnimationDelete>().sData;
        Vector3 unadjustedPos = MapStore.Instance.secondaryMap.GeoToWorldPosition(sData.latLon, true);
        go.transform.localPosition = unadjustedPos;
    }

    void UpdateScale(GameObject go)
    {
        int numDecimals = 3;
        float newSizeMultiplier = MapStore.Instance.secondaryMap.UnityTileSize * MapStore.Instance.secondaryMap.transform.localScale.x / MapStore.Instance.iconSize;
        float newSizeMultiplierRounded = (float)Math.Round(newSizeMultiplier, numDecimals);
        float currSizeMultiplier = go.GetComponent<LerpAnimationDelete>().sizeMultiplier;
        float currSizeMultiplierRounded = (float)Math.Round(currSizeMultiplier, numDecimals);
        if (!Mathf.Approximately(newSizeMultiplierRounded, currSizeMultiplierRounded))
        {
            go.GetComponent<LerpAnimationDelete>().UpdateLargestSize(newSizeMultiplier);
            Vector3 intermediate = go.transform.localScale / currSizeMultiplier;
            go.transform.localScale = intermediate * newSizeMultiplier;
        }
    }
}