using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class HistWrapper : IAbstractWrapper
{
    [SerializeField]
    public GameObject BarHolder;

    [SerializeField]
    public GameObject barPrefab;

    [SerializeField]
    public GameObject markerPrefab;

    [SerializeField]
    GameObject markerParent;

    [SerializeField]
    GameObject xAxis;

    [SerializeField]
    GameObject yAxis;

    [SerializeField]
    GameObject xLabel;

    bool initialized = false;
    bool firstRun = true;
    Histogram wrapped;
    List<GameObject> barList = new List<GameObject>();

    // Start is called before the first frame update
    void Update()
    {
        if (initialized && firstRun)
        {
            DrawBins(wrapped.CalcBins());
            DrawAxes();
            DrawScale();
            AddLabels();

            AddObjectManipulator oM = this.gameObject.AddComponent<AddObjectManipulator>();
            oM.PlaceObjectManipulator(this.transform);

            firstRun = false;
        }
    }

    public void Initialize(HistModel model)
    {
        if (!initialized)
        {
            wrapped = new Histogram(model);
            this.model = model;
            initialized = true;
        }
    }

    public void DrawBins(List<int> binNums)
    {
        HistModel histModel = CastToHistModel();
        for (int i = 0; i < binNums.Count; i++)
        {
            float binHeight = wrapped.NormalizeValue(binNums.ElementAt(i), histModel.minBin, histModel.maxBin);
            GameObject bar = Instantiate(barPrefab, BarHolder.transform);
            bar.transform.localScale = new Vector3(histModel.normedWidth, binHeight, 0.25f);
            bar.transform.localPosition = new Vector3((i * histModel.normedWidth) + histModel.offset + bar.transform.localScale.x, binHeight / 2 + histModel.offset, 0) * histModel.plotScale;
            bar.transform.rotation = Quaternion.identity;
            bar.GetComponent<DataPoint>().SetOriginalValues();
            // This is just a place holder for now, we need to come back and change
            // "currDataObj.df.Rows[0]" to be something that actually has a meaning.
            // This is fine for now though.
            bar.GetComponent<DataPoint>().SetData(histModel.compModel.dataObj.df.Rows[0]);

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
            bar.GetComponent<DataPoint>().data = histModel.compModel.dataObj.df.Rows[i];
            barList.Add(bar);
        }
    }

    protected void DrawAxes()
    {
        float lengthOfAxis = wrapped.ScaleAxes();
        xAxis.transform.localScale = new Vector3(lengthOfAxis, 0.01f, 0.25f);
        yAxis.transform.localScale = new Vector3(0.01f, lengthOfAxis, 0.25f);

        xAxis.transform.position = new Vector3(lengthOfAxis / 2, 0, 0);
        yAxis.transform.position = new Vector3(0, lengthOfAxis / 2, 0);
    }

    protected void DrawLabels()
    {
        HistModel histModel = CastToHistModel();
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        xTextMesh.text = histModel.xName;
        float xPos = xAxis.transform.localScale.x / 2;
        xLabel.transform.position = new Vector3(xPos, -histModel.offset * 2, 0);
    }

    void DrawScale()
    {
        HistModel histModel = CastToHistModel();
        int numMarkersPerAxis = 2;
        GameObject currMarker;
        Transform markerParent = this.markerParent.transform;

        for (int i = 1; i <= numMarkersPerAxis; i++)
        {
            float value = (histModel.binWidth * histModel.numBins / i) + histModel.xMin;
            // We are estimating an upper bound for each box,
            // so make sure that we don't over estimate.
            if(value > histModel.xMax)
            {
                value = histModel.xMax;
            }
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3((histModel.normedWidth * histModel.numBins / i) + histModel.offset, -histModel.offset / 2, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            value = (float)Math.Round(value, 1);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            currMarker.GetComponent<TextMesh>().anchor = TextAnchor.UpperCenter;

            value = ((histModel.maxBin - histModel.minBin) / i) + histModel.minBin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.position = new Vector3(-histModel.offset, histModel.plotScale / i + histModel.offset, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            value = (float)Math.Round(value, 1);
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
        }
    }

    void AddLabels()
    {
        HistModel histModel = CastToHistModel();
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        xTextMesh.text = histModel.xName;
        float xPos = xAxis.transform.localScale.x / 2;
        xLabel.transform.position = new Vector3(xPos, -histModel.offset * 2, 0);
    }

    HistModel CastToHistModel()
    {
        if (!(model is HistModel histModel))
        {
            throw new ArgumentException("Model must be of type HistModel");
        }
        return histModel;
    }

    public override void Create()
    {
        Debug.Log("Alrighty");
    }
}
