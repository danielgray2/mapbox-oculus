using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TimeSeries : MonoBehaviour
{
    public enum DataSelectionType { TIME, INDEXED, SCRUBBED };
    [SerializeField]
    public GameObject dataPointPrefab;

    [SerializeField]
    public float secondsPerHour = 1;

    // This param was added so that we could
    // initialize the parent for scrubbing and
    // access it later. Should probably follow
    // the same pattern for the rest, just for
    // consistency.
    GameObject scrubbingParent;
    GameObject scrubbingMenuPrefab;
    GameObject scrubbingMenuGo;
    bool beginAnimation = false;
    bool animationBegunOnce = false;
    bool timeAnimStopped = false;
    bool indexedAnimStopped = false;
    bool scrubbing = false;
    bool menuActive = false;
    DataSelectionType dSType;
    DateTime startTime;
    int startIndex;
    int numberOfValues;
    Gradient gradient;

    void Update()
    {
        if (beginAnimation)
        {
            beginAnimation = false;
            if (this.dSType == DataSelectionType.TIME)
            {
                StartCoroutine("GraphObjectsTime");
            }
            else if(this.dSType == DataSelectionType.INDEXED)
            {
                StartCoroutine("GraphObjectsIndexed");
            }
            else if(this.dSType == DataSelectionType.SCRUBBED)
            {
                PrepForScrubbing();
            }
        }
        HandleMenuToggle();
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

    public void BeginScrubbedAnimation(GameObject scrubbingMenuPrefab, Gradient gradient)
    {
        if (!animationBegunOnce)
        {
            beginAnimation = true;
            this.scrubbingMenuPrefab = scrubbingMenuPrefab;
            this.dSType = DataSelectionType.SCRUBBED;
            this.gradient = gradient;
        }
        animationBegunOnce = true;
    }

    public void PrepForScrubbing()
    {
        this.scrubbing = true;
        scrubbingMenuGo = Instantiate(scrubbingMenuPrefab, Vector3.zero, Quaternion.identity);
        ActivateMenu();
        CalendarMenu cM = scrubbingMenuGo.GetComponent<CalendarMenu>();
        cM.DateUpdated.AddListener(HandleScrubbing);
        GameObject parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandler>();
        this.scrubbingParent = parent;
    }

    IEnumerator GraphObjectsTime()
    {
        GameObject parent = new GameObject{ name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandler>();
        int currIndex = DataStore.Instance.GetIndexTime(this.startTime);
        float hoursToAdd = 3;
        DataSlice dataSlice;
        while (!timeAnimStopped)
        {
            dataSlice = DataStore.Instance.SliceIndexTime(currIndex, hoursToAdd);
            foreach(SData sData in dataSlice.dataList)
            {
                Vector3 position = MapStore.Instance.map.GeoToWorldPosition(sData.latLon);
                Vector3 adjPos = sData.AdjustPosForDepth(position);
                GameObject go = Instantiate(dataPointPrefab, adjPos, Quaternion.identity);
                go.transform.parent = parent.transform;
                MapStore.Instance.iconGOs.Add(go);
                LerpAnimation lA = go.GetComponent<LerpAnimation>();
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
        GameObject parent = new GameObject { name = "DataPoints" };
        parent.transform.position = Vector3.zero;
        parent.AddComponent<PointPositionHandler>();
        int currIndex = this.startIndex;

        while (!indexedAnimStopped)
        {
            while(MapStore.Instance.iconGOs.Count < numberOfValues)
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

    IEnumerator GraphObjectsIndexedGradient()
    {
        GameObject parent = new GameObject { name = "DataPoints" };
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

    public void HandleScrubbing(List<DateTime> startAndEndTimes)
    {
        DateTime startTime = startAndEndTimes[0];
        DateTime endTime = startAndEndTimes[1];
        List<SData> newDataList = DataStore.Instance.SliceTimes(startTime, endTime).dataList;
        List<GameObject> newIconList = new List<GameObject>();

        // Remove the previous go's since we don't do this is in the
        // lerp animation since we are scrubbing. We should find a better
        // solution for handling animations on data points
        foreach (GameObject go in MapStore.Instance.iconGOs)
        {
            Destroy(go);
        }
        
        foreach(SData sData in newDataList)
        {
            Vector3 position = MapStore.Instance.map.GeoToWorldPosition(sData.latLon);
            position = sData.AdjustPosForDepth(position);
            GameObject go = Instantiate(dataPointPrefab, position, Quaternion.identity);
            go.transform.parent = this.scrubbingParent.transform;
            newIconList.Add(go);
            LerpAnimation lA = go.GetComponent<LerpAnimation>();
            lA.Setup(sData, false, gradient);
        }

        MapStore.Instance.iconGOs = newIconList;
    }

    public void HandleMenuToggle()
    {
        if (scrubbing)
        {
            if (menuActive && (Input.GetKeyDown(KeyCode.I) || OVRInput.GetDown(OVRInput.RawButton.Y)))
            {
                DeactivateMenu();
            }
            else if(!menuActive && (Input.GetKeyDown(KeyCode.I) || OVRInput.GetUp(OVRInput.RawButton.Y)))
            {
                ActivateMenu();
            }
        }
    }

    protected void ActivateMenu()
    {
        scrubbingMenuGo.SetActive(true);
        menuActive = true;
    }

    protected void DeactivateMenu()
    {
        scrubbingMenuGo.SetActive(false);
        menuActive = false;
    }
}