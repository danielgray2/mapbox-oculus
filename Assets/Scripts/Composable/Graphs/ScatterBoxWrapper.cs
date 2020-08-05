using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBoxWrapper : MonoBehaviour
{
    [SerializeField]
    GameObject dataPointPrefab;

    [SerializeField]
    GameObject PointHolder;
    public ScatterBoxOptions options { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

    }

    void CreateDataPoints(List<List<float>> vals)
    {
        for(int i = 0; i < vals.Count; i++)
        {
            List<float> currVal = vals[i];
            GameObject dataPoint = Instantiate(dataPointPrefab, PointHolder.transform);
            float x = currVal[0];
            float y = currVal[1];
            float z = currVal[2];
            dataPoint.transform.localPosition = new Vector3(x, y, z) * plotScale;
            // TODO: Use the value from options
            dataPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            // TODO: Use the value from options
            dataPoint.transform.rotation = Quaternion.identity;
            dataPoint.GetComponent<DataPoint>().SetOriginalValues();
            dataPoint.GetComponent<DataPoint>().SetData(currDataObj.df.Rows[i]);

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
            dataPoint.GetComponent<DataPoint>().data = currDataObj.df.Rows[i];
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
        currMarker = Instantiate(markerPrefab, markerParent);
        currMarker.transform.localPosition = new Vector3(plotScale / i, plotScale / 5, 0);
        currMarker.transform.rotation = xLabel.transform.rotation;
        currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");

        currMarker = Instantiate(markerPrefab, markerParent);
        currMarker.transform.localPosition = new Vector3(0, plotScale / i, 0);
        currMarker.transform.rotation = yLabel.transform.rotation;
        currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");

        currMarker = Instantiate(markerPrefab, markerParent);
        currMarker.transform.localPosition = new Vector3(0, plotScale / 5, plotScale / i);
        currMarker.transform.rotation = zLabel.transform.rotation;
        currMarker.GetComponent<TextMesh>().text = value.ToString("0.0");
    }
}
