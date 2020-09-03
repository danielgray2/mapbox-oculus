using System;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBoxWrapper : IAbstractWrapper
{
    [SerializeField]
    GameObject dataPointPrefab;

    [SerializeField]
    GameObject PointHolder;

    [SerializeField]
    GameObject xAxis;

    [SerializeField]
    GameObject yAxis;

    [SerializeField]
    GameObject zAxis;

    [SerializeField]
    GameObject xLabel;

    [SerializeField]
    GameObject yLabel;

    [SerializeField]
    GameObject zLabel;

    [SerializeField]
    GameObject markerPrefab;

    [SerializeField]
    Transform markerParent;

    ScatterBox wrapped;
    bool initialized = false;
    AddObjectManipulator oM;
    bool plottedOnce = false;
    public DataObj dataObj { get; set; }
    List<GameObject> pointList = new List<GameObject>();
    List<GameObject> currMarkerList = new List<GameObject>();

    // Update is called once per frame
    public void Plot()
    {
        if (initialized && !plottedOnce)
        {
            CreateDataPoints(wrapped.CreatePoints());
            DrawAxes(wrapped.ScaleAxes());
            AddLabels();
            DrawScale(wrapped.PlaceScale());

            oM = this.gameObject.AddComponent<AddObjectManipulator>();
            oM.PlaceObjectManipulator(this.transform);

            plottedOnce = true;
        }
    }

    public override void ReRender()
    {
        for (int i = 0; i < pointList.Count; i++)
        {
            Destroy(pointList[i]);
        }
        pointList = new List<GameObject>();

        for (int i = 0; i < currMarkerList.Count; i++)
        {
            Destroy(currMarkerList[i]);
        }
        currMarkerList = new List<GameObject>();

        CreateDataPoints(wrapped.CreatePoints());
        DrawAxes(wrapped.ScaleAxes());
        AddLabels();
        DrawScale(wrapped.PlaceScale());

        oM.UpdateObjectManipulator();
    }

    public void Initialize(ScatterModel model)
    {
        if (!initialized)
        {
            wrapped = new ScatterBox(model);
            this.model = model;
            model.modelUpdateEvent.AddListener(HandleModelUpdate);
            initialized = true;
        }
    }

    void CreateDataPoints(List<List<float>> vals)
    {
        ScatterModel scatterModel = CastToScatterModel();
        for (int i = 0; i < vals.Count; i++)
        {
            List<float> currVal = vals[i];
            GameObject dataPoint = Instantiate(dataPointPrefab, PointHolder.transform);
            float x = currVal[0];
            float y = currVal[1];
            float z = currVal[2];
            dataPoint.transform.localPosition = new Vector3(x, y, z) * scatterModel.plotScale;
            // TODO: Use the value from options
            dataPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            // TODO: Use the value from options
            dataPoint.transform.localRotation = Quaternion.identity;
            dataPoint.GetComponent<DataPoint>().SetOriginalValues();
            dataPoint.GetComponent<DataPoint>().SetData(scatterModel.dataObj.df.Rows[i]);

            // Assigns original values to dataPointName
            string dataPointName =
                x + " "
                + y + " "
                + z;

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // TODO: Maybe extract this so that we set it based on the DataPointOptions
            dataPoint.GetComponent<Renderer>().material.color =
                new Color(x, y, z, 1.0f);
            // Adds a Point object to this point
            dataPoint.GetComponent<DataPoint>().data = scatterModel.dataObj.df.Rows[i];
            pointList.Add(dataPoint);
        }
    }

    void DrawAxes(List<List<Vector3>> axesList)
    {
        LineRenderer xLine = xAxis.GetComponent<LineRenderer>();
        LineRenderer yLine = yAxis.GetComponent<LineRenderer>();
        LineRenderer zLine = zAxis.GetComponent<LineRenderer>();

        xLine.SetPositions(axesList[0].ToArray());
        yLine.SetPositions(axesList[1].ToArray());
        zLine.SetPositions(axesList[2].ToArray());

        // TODO: Set an option instead of 0.01f
        xLine.startWidth = 0.01f * transform.localScale.x;
        yLine.startWidth = 0.01f * transform.localScale.y;
        zLine.startWidth = 0.01f * transform.localScale.z;
    }

    void AddLabels()
    {
        ScatterModel scatterModel = CastToScatterModel();
        TextMesh xTextMesh = xLabel.GetComponent<TextMesh>();
        TextMesh yTextMesh = yLabel.GetComponent<TextMesh>();
        TextMesh zTextMesh = zLabel.GetComponent<TextMesh>();

        xTextMesh.text = scatterModel.xName;
        yTextMesh.text = scatterModel.yName;
        zTextMesh.text = scatterModel.zName;

        float plotScale = scatterModel.plotScale;
        float extraMargin = scatterModel.extraMargin;

        xLabel.transform.localPosition = new Vector3(plotScale / 1.5f + extraMargin * plotScale, plotScale / 2.5f, 0);
        yLabel.transform.localPosition = new Vector3(-plotScale / 2.5f, plotScale + extraMargin * plotScale, 0);
        zLabel.transform.localPosition = new Vector3(0, plotScale / 2.5f, plotScale / 2 + extraMargin * plotScale);
    }

    void DrawScale(List<List<float>> scaleList)
    {
        ScatterModel scatterModel = CastToScatterModel();

        float plotScale = scatterModel.plotScale;

        for (int i = 1; i <= scaleList.Count; i++)
        {
            GameObject currMarker;
            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(plotScale / i, plotScale / 5, 0);
            currMarker.transform.rotation = xLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = scaleList[i-1][0].ToString("0.0");
            currMarkerList.Add(currMarker);

            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(0, plotScale / i, 0);
            currMarker.transform.rotation = yLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = scaleList[i-1][1].ToString("0.0");
            currMarkerList.Add(currMarker);

            currMarker = Instantiate(markerPrefab, markerParent);
            currMarker.transform.localPosition = new Vector3(0, plotScale / 5, plotScale / i);
            currMarker.transform.rotation = zLabel.transform.rotation;
            currMarker.GetComponent<TextMesh>().text = scaleList[i-1][2].ToString("0.0");
            currMarkerList.Add(currMarker);
        }
    }

    ScatterModel CastToScatterModel()
    {
        if (!(model is ScatterModel scatterModel))
        {
            throw new ArgumentException("Model must be of type ScatterModel");
        }
        return scatterModel;
    }

    public override void Create()
    {
        Debug.Log("Alrighty");
    }
}
