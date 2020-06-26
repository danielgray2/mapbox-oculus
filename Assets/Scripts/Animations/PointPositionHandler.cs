using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPositionHandler : MonoBehaviour
{
    void Update()
    {
        foreach (GameObject go in MapStore.Instance.iconGOs)
        {
            SData sData = go.GetComponent<LerpAnimation>().sData;
            go.transform.localPosition = MapStore.Instance.map.GeoToWorldPosition(sData.latLon, true);
        }
    }
}