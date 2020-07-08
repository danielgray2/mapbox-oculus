using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TimeSeriesDelete : MonoBehaviour
{
    public enum DataSelectionType { TIME, INDEXED, SCRUBBED };

    public GameObject dataPointPrefab { get; set; }

    [SerializeField]
    public float secondsPerHour = 1;

    GameObject parent;

    // This param was added so that we could
    // initialize the parent for scrubbing and
    // access it later. Should probably follow
    // the same pattern for the rest, just for
    // consistency.
    GameObject scrubbingMenuGo;
    bool beginAnimation = false;
    bool animationBegunOnce = false;
    bool timeAnimStopped = false;
    bool indexedAnimStopped = false;
    DataSelectionType dSType;
    DateTime startTime;
    int startIndex;
    int numberOfValues;
    Gradient gradient;
    CalendarMenu cM;

    void Update()
    {
        if (beginAnimation)
        {
            beginAnimation = false;
            if (this.dSType == DataSelectionType.TIME)
            {
                StartCoroutine("GraphObjectsTime");
            }
            else if (this.dSType == DataSelectionType.INDEXED)
            {
                StartCoroutine("GraphObjectsIndexed");
            }
            else if (this.dSType == DataSelectionType.SCRUBBED)
            {
                PrepForScrubbing();
            }
        }
    }

    public void BeginTimeAnimation(DateTime startTime, float secondsPerHour, Gradient gradient)
    {
        if (!animationBegunOnce)
        {
            beginAnimation = true;
            this.startTime = startTime;
            this.secondsPerHour = secondsPerHour;
            this.dSType = DataSelectionType.TIME;
            this.gradient = gradient;
        }
        animationBegunOnce = true;
    }

    public void BeginIndexedAnimation(int startIndex, int numberOfValues, float secondsPerHour, Gradient gradient)
    {
        if (!animationBegunOnce)
        {
            beginAnimation = true;
            this.startIndex = startIndex;
            this.secondsPerHour = secondsPerHour;
            this.numberOfValues = numberOfValues;
            this.dSType = DataSelectionType.INDEXED;
            this.gradient = gradient;
        }
        animationBegunOnce = true;
    }

    public void EndIndexedAnimation()
    {
        animationBegunOnce = false;
        indexedAnimStopped = true;
        MapStore.Instance.secondaryIconGOs = new List<GameObject>();
        parent.Destroy();
    }

    public void BeginScrubbedAnimation(GameObject scrubbingMenuGo, Gradient gradient)
    {
        if (!animationBegunOnce)
        {
            beginAnimation = true;
            this.scrubbingMenuGo = scrubbingMenuGo;
            this.dSType = DataSelectionType.SCRUBBED;
            this.gradient = gradient;
        }
        animationBegunOnce = true;
    }

    public void EndScrubbedAnimation()
    {
        animationBegunOnce = false;
        cM.DateUpdated.RemoveListener(HandleScrubbing);
        MapStore.Instance.secondaryIconGOs = new List<GameObject>();
        parent.Destroy();
    }

    public void PrepForScrubbing()
    {
        cM = scrubbingMenuGo.GetComponent<CalendarMenu>();
        // TODO: I think that the following line will add the same listener
        // to the callback every time we change to scrubbing. We should
        // fix this at some point.
        cM.DateUpdated.AddListener(HandleScrubbing);
        parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandlerDelete>();
    }

    IEnumerator GraphObjectsTime()
    {
        parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandlerDelete>();
        int currIndex = DataStore.Instance.GetIndexTime(this.startTime);
        float hoursToAdd = 3;
        DataSlice dataSlice;
        while (!timeAnimStopped)
        {
            dataSlice = DataStore.Instance.SliceIndexTime(currIndex, hoursToAdd);
            foreach (SData sData in dataSlice.dataList)
            {
                Vector3 position = MapStore.Instance.secondaryMap.GeoToWorldPosition(sData.latLon);
                Vector3 adjPos = sData.AdjustPosForDepth(position);
                GameObject go = Instantiate(dataPointPrefab, adjPos, Quaternion.identity);
                go.transform.parent = parent.transform;
                MapStore.Instance.secondaryIconGOs.Add(go);
                LerpAnimationDelete lA = go.GetComponent<LerpAnimationDelete>();
                lA.Setup(sData, true, gradient);
            }
            if (dataSlice.lastIndex < DataStore.Instance.sDataRecords.Count)
                currIndex = dataSlice.lastIndex;
            else
                currIndex = 0;
            yield return new WaitForSeconds(secondsPerHour);
        }
    }

    IEnumerator GraphObjectsIndexed()
    {
        parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandlerDelete>();
        int currIndex = this.startIndex;
        indexedAnimStopped = false;

        while (!indexedAnimStopped)
        {
            while (MapStore.Instance.secondaryIconGOs.Count < numberOfValues)
            {
                SData sData = DataStore.Instance.sDataRecords.ElementAt(currIndex);
                Vector3 pos = MapStore.Instance.secondaryMap.GeoToWorldPosition(sData.latLon);
                Vector3 adjPos = sData.AdjustPosForDepth(pos);
                GameObject go = Instantiate(dataPointPrefab, adjPos, Quaternion.identity);
                go.transform.parent = parent.transform;
                MapStore.Instance.secondaryIconGOs.Add(go);
                LerpAnimationDelete lA = go.GetComponent<LerpAnimationDelete>();
                lA.Setup(sData, true, gradient);
                if (currIndex < DataStore.Instance.sDataRecords.Count - 1)
                    currIndex++;
                else
                    currIndex = 0;
            }
            yield return new WaitForSeconds(secondsPerHour);
        }
    }

    /*
    IEnumerator GraphObjectsIndexedGradient()
    {
        parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandler>();
        int currIndex = this.startIndex;

        while (!indexedAnimStopped)
        {
            while (MapStore.Instance.iconGOs.Count < numberOfValues)
            {
                SData sData = DataStore.Instance.sDataRecords.ElementAt(currIndex);
                Vector3 position = MapStore.Instance.map.GeoToWorldPosition(sData.latLon);
                position = sData.AdjustPosForDepth(position);
                GameObject go = Instantiate(dataPointPrefab, position, Quaternion.identity);
                go.transform.parent = parent.transform;
                MapStore.Instance.iconGOs.Add(go);
                LerpAnimation lA = go.GetComponent<LerpAnimation>();
                lA.Setup(sData, true, gradient);
                if (currIndex < DataStore.Instance.sDataRecords.Count - 1)
                    currIndex++;
                else
                    currIndex = 0;
            }
            yield return new WaitForSeconds(secondsPerHour);
        }
    }
    */

    public void HandleScrubbing(List<DateTime> startAndEndTimes)
    {
        DateTime startTime = startAndEndTimes[0];
        DateTime endTime = startAndEndTimes[1];
        List<SData> newDataList = DataStore.Instance.SliceTimes(startTime, endTime).dataList;
        List<GameObject> newIconList = new List<GameObject>();

        // Remove the previous go's since we don't do this is in the
        // lerp animation since we are scrubbing. We should find a better
        // solution for handling animations on data points
        foreach (GameObject go in MapStore.Instance.secondaryIconGOs)
        {
            Destroy(go);
        }

        foreach (SData sData in newDataList)
        {
            Vector3 position = MapStore.Instance.secondaryMap.GeoToWorldPosition(sData.latLon);
            position = sData.AdjustPosForDepth(position);
            GameObject go = Instantiate(dataPointPrefab, position, Quaternion.identity);
            go.transform.parent = this.parent.transform;
            newIconList.Add(go);
            LerpAnimationDelete lA = go.GetComponent<LerpAnimationDelete>();
            lA.Setup(sData, false, gradient);
        }

        MapStore.Instance.secondaryIconGOs = newIconList;
    }
}
