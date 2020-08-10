using Microsoft.Data.Analysis;
using System;
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

    public ScatterBox(ScatterModel model)
    {
        this.model = model;
        GraphStore.Instance.graphList.Add(this);
        DataObj dataObj = model.compModel.dataObj;

        xMax = dataObj.GetMax(model.xName);
        yMax = dataObj.GetMax(model.xName);
        zMax = dataObj.GetMax(model.xName);

        xMin = dataObj.GetMin(model.xName);
        yMin = dataObj.GetMin(model.xName);
        zMin = dataObj.GetMin(model.xName);

        maxDpSize = new Vector3(0.15f, 0.15f, 0.15f);
        minDpSize = new Vector3(0.01f, 0.01f, 0.01f);
    }

    //public void Update()
    //{
        //ScaleAxes();
    //}

    public List<List<float>> CreatePoints()
    {
        if(!(model is ScatterModel scatterModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        List<List<float>> retList = new List<List<float>>();
        DataObj dataObj = scatterModel.compModel.dataObj;

        for (var i = 0; i < dataObj.df.Rows.Count; i++)
        {
            List<float> currList = new List<float>();

            float xVal = (float)dataObj.df.Columns[scatterModel.xName][i];
            float x = (xVal - xMin) / (xMax - xMin);

            float yVal = (float)dataObj.df.Columns[scatterModel.yName][i];
            float y = (yVal - yMin) / (yMax - yMin);

            float zVal = (float)dataObj.df.Columns[scatterModel.zName][i];
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
        if (!(model is ScatterModel scatterModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        float lengthOfAxis = scatterModel.plotScale + scatterModel.plotScale * scatterModel.extraMargin;
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
        if (!(model is ScatterModel scatterModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        List<List<float>> retList = new List<List<float>>();
        for (int i = 1; i <= scatterModel.numMarkersPerAxis; i++)
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