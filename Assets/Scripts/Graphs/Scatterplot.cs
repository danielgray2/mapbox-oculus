﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterplot : MonoBehaviour, IGraph
{
    public List<SData> dataList = new List<SData>();
    private List<GameObject> pointList = new List<GameObject>();
    public float extraMargin = 0.1f;
    public GameObject dataPointPrefab;
    public GameObject markerPrefab;

    // Full column names
    public string xName { get; set; }
    public string yName { get; set; }
    public string zName { get; set; }

    public float plotScale = 10;

    [SerializeField]
    public GameObject PointHolder;

    [SerializeField]
    GameObject markerParent;

    [SerializeField]
    GameObject xAxis;

    [SerializeField]
    GameObject yAxis;
    
    [SerializeField]
    GameObject zAxis;

    float xMax;
    float yMax;
    float zMax;
    float xMin;
    float yMin;
    float zMin;

    bool initialized = false;

    [SerializeField]
    GameObject xLabel;

    [SerializeField]
    GameObject yLabel;

    [SerializeField]
    GameObject zLabel;

    public void InitializeScatterplot(List<SData> data, string xName, string yName, string zName)
    {
        this.dataList = data;
        GraphStore.Instance.graphList.Add(this);

        xMax = FindMax(xName);
        yMax = FindMax(yName);
        zMax = FindMax(zName);

        xMin = FindMin(xName);
        yMin = FindMin(yName);
        zMin = FindMin(zName);

        this.xName = xName;
        this.yName = yName;
        this.zName = zName;

        Plot();
        initialized = true;
    }

    public List<SData> GetData()
    {
        return dataList;
    }

    public void Plot()
    {
        PlotPoints();
        ScaleAxes();
        AddLabels();
        PlaceScale();
    }

    public void PlotPoints()
    {
        for (var i = 0; i < dataList.Count; i++)
        {
            float xVal = (float)dataList[i].GetType().GetProperty(xName).GetValue(dataList[i], null);
            float x =
                (xVal - xMin)
                / (xMax - xMin);

            float yVal = (float)dataList[i].GetType().GetProperty(yName).GetValue(dataList[i], null);
            float y =
                (yVal - yMin)
                / (yMax - yMin);

            float zVal = (float)dataList[i].GetType().GetProperty(zName).GetValue(dataList[i], null);
            float z =
                (zVal - zMin)
                / (zMax - zMin);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    dataPointPrefab,
                    new Vector3(x, y, z) * plotScale,
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.parent = PointHolder.transform;

            // Assigns original values to dataPointName
            string dataPointName =
                xVal + " "
                + yVal + " "
                + zVal;

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // Gets material color and sets it to a new RGB color we define
            dataPoint.GetComponent<Renderer>().material.color =
                new Color(x, y, z, 1.0f);
            // Adds a Point object to this point
            dataPoint.GetComponent<DataHolder>().data = dataList[i];
            pointList.Add(dataPoint);
        }
    }

    public void SetData(List<SData> data)
    {
        this.dataList = data;
    }

    public float FindMax(string attrName)
    {
        float maxValue = -1000000f;
        foreach(SData sData in dataList)
        {
            float currVal = (float)sData.GetType().GetProperty(attrName).GetValue(sData, null);
            if (currVal > maxValue)
            {
                maxValue = currVal;
            }
        }
        return maxValue;
    }

    public float FindMin(string attrName)
    {
        float minValue = 1000000f;
        foreach (SData sData in dataList)
        {
            float currVal = (float)sData.GetType().GetProperty(attrName).GetValue(sData, null);
            if (currVal < minValue)
            {
                minValue = currVal;
            }
        }
        return minValue;
    }

    void ScaleAxes()
    {
        int numPoints = 2;
        float lengthOfAxis = plotScale + plotScale * extraMargin;
        var origin = new Vector3(0, 0, 0);

        Vector3[] xPoints = new Vector3[numPoints];
        Vector3[] yPoints = new Vector3[numPoints];
        Vector3[] zPoints = new Vector3[numPoints];

        xPoints[0] = origin;
        yPoints[0] = origin;
        zPoints[0] = origin;

        xPoints[1] = new Vector3(lengthOfAxis, 0, 0);
        yPoints[1] = new Vector3(0, lengthOfAxis, 0);
        zPoints[1] = new Vector3(0, 0, lengthOfAxis);

        LineRenderer xLine = xAxis.GetComponent<LineRenderer>();
        LineRenderer yLine = yAxis.GetComponent<LineRenderer>();
        LineRenderer zLine = zAxis.GetComponent<LineRenderer>();

        xLine.SetPositions(xPoints);
        yLine.SetPositions(yPoints);
        zLine.SetPositions(zPoints);
    }

    void AddLabels()
    {
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        TextMesh yTextMesh = yLabel.GetComponent<TextMesh>();
        TextMesh zTextMesh = zLabel.GetComponent<TextMesh>();

        xTextMesh.text = xName;
        yTextMesh.text = yName;
        zTextMesh.text = zName;

        xLabel.transform.position = new Vector3(plotScale / 2 + extraMargin * plotScale, plotScale / 3, 0);
        yLabel.transform.position = new Vector3(0, plotScale + extraMargin * plotScale, 0);
        zLabel.transform.position = new Vector3(0, plotScale / 3, plotScale / 2 + extraMargin * plotScale);
    }

    void PlaceScale()
    {
        int numMarkersPerAxis = 2;
        GameObject currMarker;
        Transform markerParent = this.markerParent.transform;

        for (int i = 1; i <= numMarkersPerAxis; i++)
        {
            float value = xMax / i;
            currMarker = Instantiate(markerPrefab, new Vector3(plotScale / i, plotScale / 5, 0), xLabel.transform.rotation);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            currMarker.transform.parent = markerParent;

            value = yMax / i;
            currMarker = Instantiate(markerPrefab, new Vector3(0, plotScale / i, 0), xLabel.transform.rotation);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            currMarker.transform.parent = markerParent;

            value = zMax / i;
            currMarker = Instantiate(markerPrefab, new Vector3(0, plotScale / 5, plotScale / i), xLabel.transform.rotation);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            currMarker.transform.parent = markerParent;
        }
    }
}
