using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBox : IAbstractGraph
{
    public DataObj currDataObj;
    private List<GameObject> pointList = new List<GameObject>();
    public float extraMargin;
    public GameObject dataPointPrefab;
    public GameObject markerPrefab;

    public ScatterBox(ScatterBoxOptions options)
    {
        this.options = options;
    }

    // Full column names
    public string xName { get; set; }
    public string yName { get; set; }
    public string zName { get; set; }

    public float plotScale = 1;

    Vector3 dataPointScale = new Vector3(0.05f, 0.05f, 0.05f);

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

    bool plottedOnce = false;

    [SerializeField]
    GameObject xLabel;

    [SerializeField]
    GameObject yLabel;

    [SerializeField]
    GameObject zLabel;

    public void InitializeScatterplot(DataObj currDataObj, string xName, string yName, string zName)
    {
        this.currDataObj = currDataObj;
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
    }

    public DataObj GetData()
    {
        return currDataObj;
    }

    public void Plot()
    {
        if (!plottedOnce)
        {
            PlotPoints();
            ScaleAxes();
            AddLabels();
            PlaceScale();
            plottedOnce = true;
        }
    }

    //public void Update()
    //{
        ScaleAxes();
    //}

    public void PlotPoints()
    {
        for (var i = 0; i < currDataObj.df.Rows.Count; i++)
        {
            float xVal = (float)currDataObj.df.Columns[xName][i];
            float x =
                (xVal - xMin)
                / (xMax - xMin);

            float yVal = (float)currDataObj.df.Columns[yName][i];
            float y =
                (yVal - yMin)
                / (yMax - yMin);

            float zVal = (float)currDataObj.df.Columns[zName][i];
            float z =
                (zVal - zMin)
                / (zMax - zMin);

            GameObject dataPoint = Instantiate(dataPointPrefab, PointHolder.transform);
            dataPoint.transform.localPosition = new Vector3(x, y, z) * plotScale;
            dataPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            dataPoint.transform.rotation = Quaternion.identity;
            dataPoint.GetComponent<DataPoint>().SetOriginalValues();
            dataPoint.GetComponent<DataPoint>().SetData(currDataObj.df.Rows[i]);

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
            dataPoint.GetComponent<DataPoint>().data = currDataObj.df.Rows[i];
            pointList.Add(dataPoint);
        }
    }

    public void SetData(DataObj currDataObj)
    {
        this.currDataObj = currDataObj;
    }

    public float FindMax(string attrName)
    {
        return currDataObj.GetMax(attrName);
    }

    public float FindMin(string attrName)
    {
        return currDataObj.GetMin(attrName);
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

        xLine.startWidth = 0.01f * transform.localScale.x;
        yLine.startWidth = 0.01f * transform.localScale.y;
        zLine.startWidth = 0.01f * transform.localScale.z;
    }

    void AddLabels()
    {
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        TextMesh yTextMesh = yLabel.GetComponent<TextMesh>();
        TextMesh zTextMesh = zLabel.GetComponent<TextMesh>();

        xTextMesh.text = xName;
        yTextMesh.text = yName;
        zTextMesh.text = zName;

        xLabel.transform.localPosition = new Vector3(plotScale / 2 + extraMargin * plotScale, plotScale / 2.5f, 0);
        yLabel.transform.localPosition = new Vector3(-plotScale / 2.5f, plotScale + extraMargin * plotScale, 0);
        zLabel.transform.localPosition = new Vector3(0, plotScale / 2.5f, plotScale / 2 + extraMargin * plotScale);
    }

    void PlaceScale()
    {
        int numMarkersPerAxis = 2;
        GameObject currMarker;
        Transform markerParent = this.markerParent.transform;

        for (int i = 1; i <= numMarkersPerAxis; i++)
        {
            float value = ((xMax - xMin) / i) + xMin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(plotScale / i, plotScale / 5, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");

            value = ((yMax - yMin) / i) + yMin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(0, plotScale / i, 0);
            currMarker.transform.rotation = yLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");

            value = ((zMax - zMin) / i) + zMin;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(0, plotScale / 5, plotScale / i);
            currMarker.transform.rotation = zLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
            
        }
    }

    public List<GameObject> GetDataPoints()
    {
        return pointList;
    }

    public Vector3 GetMaxDpScale()
    {
        return new Vector3(0.15f, 0.15f, 0.15f);
    }

    public Vector3 GetMinDpScale()
    {
        return new Vector3(0.01f, 0.01f, 0.01f);
    }

    public override void Plot()
    {
        throw new System.NotImplementedException();
    }
}