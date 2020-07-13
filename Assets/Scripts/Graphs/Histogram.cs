using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Histogram : MonoBehaviour, IGraph
{
    public List<SData> dataList = new List<SData>();
    private List<GameObject> barList = new List<GameObject>();
    public float extraMargin = 0.1f;
    public GameObject barPrefab;
    public GameObject markerPrefab;

    // Full column names
    public string xName { get; set; }
    public string yName { get; set; }

    public float plotScale = 1;

    [SerializeField]
    public GameObject BarHolder;

    [SerializeField]
    GameObject markerParent;

    [SerializeField]
    GameObject xAxis;

    [SerializeField]
    GameObject yAxis;

    [SerializeField]
    float offset = 0.1f;

    float binWidth;
    float normedWidth;
    int numBins;

    float xMax;
    float xMin;
    float maxBin;
    float minBin;

    bool initialized = false;

    [SerializeField]
    GameObject xLabel;

    public void InitializeHistogram(List<SData> data, string xName)
    {
        this.dataList = data;
        GraphStore.Instance.graphList.Add(this);

        xMax = FindMax(xName);
        xMin = FindMin(xName);

        this.xName = xName;

        Plot();
        initialized = true;
    }

    public List<SData> GetData()
    {
        return dataList;
    }

    public void Plot()
    {
        CreateBars();
        ScaleAxes();
        AddLabels();
        PlaceScale();
    }

    public void CreateBars()
    {
        float iqr = CalcIQR(xName);
        binWidth = CalcBinWidth(iqr, xMin, xMax, dataList.Count);
        normedWidth = NormalizeValue(binWidth, xMin, xMax);
        numBins = (int)Mathf.Floor(1 / normedWidth);

        List<int> binNums = new List<int>();
        for (var i = 0; i < numBins - 1; i++)
        {
            float x = i * normedWidth;

            int val = dataList
                .Select(s => (float)s.GetType().GetProperty(xName).GetValue(s, null))
                .Where(s => s >= binWidth * i && s < binWidth * (i + 1))
                .Count();

            binNums.Add(val);
        }

        minBin = binNums.Min();
        maxBin = binNums.Max();

        for(int i = 0; i < binNums.Count; i++)
        {
            float binHeight = NormalizeValue(binNums.ElementAt(i), minBin, maxBin);
            GameObject bar = Instantiate(barPrefab, BarHolder.transform);
            bar.transform.localScale = new Vector3(normedWidth, binHeight, 0.25f);
            bar.transform.localPosition = new Vector3((i * normedWidth) + offset + bar.transform.localScale.x, binHeight / 2 + offset, 0) * plotScale;
            bar.transform.rotation = Quaternion.identity;

            // Assigns original values to dataPointName
            string barName = i.ToString();

            // Assigns name to the prefab
            bar.transform.name = barName;

            // Gets material color and sets it to a new RGB color we define
            Vector3 startColorVector = new Vector3(1, 0, 0);
            float endVal = 0.1f * binNums.Count;
            Vector3 endColorVector = startColorVector - new Vector3(endVal, endVal, endVal);
            Color startColor = new Color(startColorVector.x, startColorVector.y, startColorVector.z);
            Color endColor = new Color(endColorVector.x, endColorVector.y, endColorVector.z);
            bar.GetComponent<Renderer>().material.color =
                Color.Lerp(startColor, endColor, i / binNums.Count);
            // Adds a Point object to this point
            bar.GetComponent<DataHolder>().data = dataList[i];
            barList.Add(bar);
        }
    }

    public float NormalizeValue(float rawValue, float minValue, float maxValue)
    {
        return (rawValue - minValue)
            / (maxValue - minValue);
    }

    public void SetData(List<SData> data)
    {
        this.dataList = data;
    }

    public float FindMax(string attrName)
    {
        float maxValue = -1000000f;
        foreach (SData sData in dataList)
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

    public float CalcIQR(string attrName)
    {
        List<float> dataVals = dataList.Select(s => (float)s.GetType().GetProperty(attrName).GetValue(s, null)).ToList();
        float median = DataStore.Instance.CalculateMedian(dataVals);
        float qOne = DataStore.Instance.CalculateMedian(dataVals.Where(v => v < median).ToList());
        float qThree = DataStore.Instance.CalculateMedian(dataVals.Where(v => v >= median).ToList());

        return qThree - qOne;
    }

    // n is the number of observations
    public float CalcBinWidth(float iqr, float min, float max, float n)
    {
        float range = max - min;
        float h = (float)(2 * iqr * Math.Pow((double)n, -1 / 3));
        return range / h;
    }

    void ScaleAxes()
    {
        float lengthOfAxis = plotScale + plotScale * extraMargin;
        xAxis.transform.localScale = new Vector3(lengthOfAxis, 0.01f, 0.25f);
        yAxis.transform.localScale = new Vector3(0.01f, lengthOfAxis, 0.25f);

        xAxis.transform.position = new Vector3(lengthOfAxis / 2, 0, 0);
        yAxis.transform.position = new Vector3(0, lengthOfAxis / 2, 0);
    }

    void AddLabels()
    {
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        xTextMesh.text = xName;
        float xPos = (xAxis.transform.localScale.x + xLabel.transform.localScale.x) / 2;
        xLabel.transform.position = new Vector3(xPos, -offset * 2, 0);
    }

    void PlaceScale()
    {
        int numMarkersPerAxis = 2;
        GameObject currMarker;
        Transform markerParent = this.markerParent.transform;

        for (int i = 1; i <= numMarkersPerAxis; i++)
        {
            float value = binWidth * numBins / i;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3((normedWidth * numBins / i) + offset, -offset/2, 0);
            //Vector3 origRotation = xLabel.transform.rotation.eulerAngles;
            //currMarker.transform.rotation = Quaternion.Euler(origRotation.x, origRotation.y, origRotation.z - 45);
            currMarker.transform.rotation = xLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");

            value = ((maxBin - minBin) / i) + minBin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3(-offset, plotScale / i + offset, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
        }
    }
}