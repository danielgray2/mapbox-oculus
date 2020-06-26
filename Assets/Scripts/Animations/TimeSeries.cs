using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSeries : MonoBehaviour
{
    [SerializeField]
    public GameObject dataPointPrefab;

    [SerializeField]
    public float secondsPerHour = 1;

    bool beginAnimation = false;
    bool animationBegunOnce = false;
    bool stopped = false;
    DateTime startTime;

    void Update()
    {
        if (beginAnimation)
        {
            beginAnimation = false;
            StartCoroutine("GraphObjects");
        }
    }

    public void BeginAnimation(DateTime startTime, float secondsPerHour)
    {
        if (!animationBegunOnce)
        {
            beginAnimation = true;
            this.startTime = startTime;
            this.secondsPerHour = secondsPerHour;
        }
        animationBegunOnce = true;
    }

    IEnumerator GraphObjects()
    {
        GameObject parent = new GameObject{ name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandler>();
        int currIndex = DataStore.Instance.GetIndexTime(this.startTime);
        float hoursToAdd = 3;
        DataSlice dataSlice;
        while (!stopped)
        {
            Debug.Log("new swarm");
            dataSlice = DataStore.Instance.SliceIndexTime(currIndex, hoursToAdd);
            foreach(SData sData in dataSlice.dataList)
            {
                Vector3 position = MapStore.Instance.map.GeoToWorldPosition(sData.latLon);
                GameObject go = Instantiate(dataPointPrefab, position, Quaternion.identity);
                go.transform.parent = parent.transform;
                MapStore.Instance.iconGOs.Add(go);
                LerpAnimation lA = go.GetComponent<LerpAnimation>();
                lA.Setup(sData, Vector3.zero);
            }
            currIndex = dataSlice.lastIndex;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
