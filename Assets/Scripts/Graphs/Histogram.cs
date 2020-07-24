using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.Data.Analysis;
using System.Globalization;

public class Histogram : MonoBehaviour, IGraph
{
    public DataObj currDataObj;
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
    int NUMDECIMALS = 4;

    bool plottedOnce = false;

    [SerializeField]
    GameObject xLabel;

    public void InitializeHistogram(DataObj dataObj, string xName)
    {
        this.currDataObj = dataObj;
        GraphStore.Instance.graphList.Add(this);

        xMax = FindMax(xName);
        xMin = FindMin(xName);

        this.xName = xName;

        Plot();
    }

    public DataObj GetData()
    {
        return currDataObj;
    }

    public void Plot()
    {
        if (!plottedOnce)
        {
            CreateBars();
            ScaleAxes();
            AddLabels();
            PlaceScale();
            plottedOnce = true;
        }
    }

    public void CreateBars()
    {
        float iqr = CalcIQR(xName);
        binWidth = CalcBinWidth(iqr, currDataObj.df.Rows.Count);
        numBins = Mathf.CeilToInt((xMax - xMin) / binWidth);
        normedWidth = 1.0f / (float)numBins;
        List<int> binNums = new List<int>();
        for (var i = 0; i < numBins; i++)
        {
            //float x = i * normedWidth;

            /*
            int val = dataList
                .Select(s => (float)s.GetType().GetProperty(xName).GetValue(s, null))
                .Where(s => s >= binWidth * i && s < binWidth * (i + 1))
                .Count();
            */
            List<float> selectedVals = new List<float>();
            for (int j = 0; j < currDataObj.df.Rows.Count; j++)
            {
                string stringVal = currDataObj.df.Columns[xName][j].ToString();
                var currVal = float.Parse(stringVal, CultureInfo.InvariantCulture.NumberFormat);
                if(currVal >= xMin + (binWidth * i) && currVal < xMin + (binWidth * (i + 1)))
                {
                    selectedVals.Add(currVal);
                }
            }
            int val = selectedVals.Count;
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
            bar.GetComponent<DataPoint>().SetOriginalValues();
            // This is just a place holder for now, we need to come back and change
            // "currDataObj.df.Rows[0]" to be something that actually has a meaning.
            // This is fine for now though.
            bar.GetComponent<DataPoint>().SetData(currDataObj.df.Rows[0]);

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
            bar.GetComponent<DataPoint>().data = currDataObj.df.Rows[i];
            barList.Add(bar);
        }
    }

    public float NormalizeValue(float rawValue, float minValue, float maxValue)
    {
        return (rawValue - minValue)
            / (maxValue - minValue);
    }

    public void SetData(DataObj data)
    {
        this.currDataObj = data;
    }

    public float FindMax(string attrName)
    {
        return currDataObj.GetMax(attrName);
    }

    public float FindMin(string attrName)
    {
        return currDataObj.GetMin(attrName);
    }

    public float CalcIQR(string attrName)
    {
        return currDataObj.GetIQR(attrName);
    }

    // n is the number of observations
    public float CalcBinWidth(float iqr, float n)
    {
        double exponent = 1.0 / 3.0;
        return (float)(2 * iqr / Math.Pow((double)n, exponent));
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
        float xPos = xAxis.transform.localScale.x / 2;
        xLabel.transform.position = new Vector3(xPos, -offset * 2, 0);
    }

    void PlaceScale()
    {
        int numMarkersPerAxis = 2;
        GameObject currMarker;
        Transform markerParent = this.markerParent.transform;

        for (int i = 1; i <= numMarkersPerAxis; i++)
        {
            float value = (binWidth * numBins / i) + xMin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3((normedWidth * numBins / i) + offset, -offset/2, 0);
            //Vector3 origRotation = xLabel.transform.rotation.eulerAngles;
            //currMarker.transform.rotation = Quaternion.Euler(origRotation.x, origRotation.y, origRotation.z - 45);
            currMarker.transform.rotation = xLabel.transform.rotation;
            value = (float)Math.Round(value, 1);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            currMarker.GetComponent<TextMesh>().anchor = TextAnchor.UpperCenter;

            value = ((maxBin - minBin) / i) + minBin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3(-offset, plotScale / i + offset, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            value = (float)Math.Round(value, 1);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
        }
    }

    public List<GameObject> GetDataPoints()
    {
        return this.barList;
    }

    public Vector3 GetDataPointScale()
    {
        return Vector3.zero;        
    }

    public Vector3 GetMaxDpScale()
    {
        return new Vector3(1f, 1.2f, 1f);
    }

    public Vector3 GetMinDpScale()
    {
        return new Vector3(0.01f, 0.01f, 0.01f);
    }
}