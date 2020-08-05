using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBox : IAbstractGraph
{
    float xMax;
    float yMax;
    float zMax;
    float xMin;
    float yMin;
    float zMin;

    ScatterBoxOptions options;

    public ScatterBox(ScatterBoxOptions options)
    {
        this.options = options;
        dataObj = options.dataObj;

        GraphStore.Instance.graphList.Add(this);

        xMax = options.dataObj.GetMax(options.xName);
        yMax = options.dataObj.GetMax(options.yName);
        zMax = options.dataObj.GetMax(options.zName);

        xMin = options.dataObj.GetMin(options.xName);
        yMin = options.dataObj.GetMin(options.yName);
        zMin = options.dataObj.GetMin(options.zName);

        maxDpSize = new Vector3(0.15f, 0.15f, 0.15f);
        minDpSize = new Vector3(0.01f, 0.01f, 0.01f);
    }

    //public void Update()
    //{
        //ScaleAxes();
    //}

    public List<List<float>> CreatePoints()
    {
        List<List<float>> retList = new List<List<float>>();

        for (var i = 0; i < options.dataObj.df.Rows.Count; i++)
        {
            List<float> currList = new List<float>();

            float xVal = (float)options.dataObj.df.Columns[options.xName][i];
            float x = (xVal - xMin) / (xMax - xMin);

            float yVal = (float)options.dataObj.df.Columns[options.yName][i];
            float y = (yVal - yMin) / (yMax - yMin);

            float zVal = (float)options.dataObj.df.Columns[options.zName][i];
            float z = (zVal - zMin) / (zMax - zMin);
            
            currList.Add(x);
            currList.Add(y);
            currList.Add(z);

            retList.Add(currList);
        }
        return retList;
    }

    public List<List<Vector3>> ScaleAxes()
    {
        float lengthOfAxis = options.plotScale + options.plotScale * options.extraMargin;
        Vector3 origin = new Vector3(0, 0, 0);

        List<Vector3> xPoints = new List<Vector3>();
        List<Vector3> yPoints = new List<Vector3>();
        List<Vector3> zPoints = new List<Vector3>();

        xPoints.Add(origin);
        yPoints.Add(origin);
        zPoints.Add(origin);

        xPoints.Add(new Vector3(lengthOfAxis, 0, 0));
        yPoints.Add(new Vector3(0, lengthOfAxis, 0));
        zPoints.Add(new Vector3(0, 0, lengthOfAxis));

        return new List<List<Vector3>> { xPoints, yPoints, zPoints };
    }

    public List<List<float>> PlaceScale()
    {
        List<List<float>> retList = new List<List<float>>();
        for (int i = 1; i <= options.numMarkersPerAxis; i++)
        {
            List<float> currList = new List<float>();
            
            currList.Add(((xMax - xMin) / i) + xMin);
            currList.Add(((yMax - yMin) / i) + yMin);
            currList.Add(((zMax - zMin) / i) + zMin);

            retList.Add(currList);
        }
        return retList;
    }
}