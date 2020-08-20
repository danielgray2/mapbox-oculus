using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;

public class Histogram : IAbstractGraph
{

    public Histogram(HistModel model)
    {

        this.model = model;

        GraphStore.Instance.graphList.Add(this);

        model.xMax = model.dataObj.GetMax(model.xName);
        model.xMin = model.dataObj.GetMin(model.xName);

        maxDpSize = new Vector3(1f, 1.2f, 1f);
        minDpSize = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public List<int> CalcBins()
    {
        if (!(model is HistModel histModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        DataObj dataObj = histModel.dataObj;
        float iqr = dataObj.GetIQR(histModel.xName);
        histModel.binWidth = CalcBinWidth(iqr, dataObj.df.Rows.Count);
        histModel.numBins = Mathf.CeilToInt((histModel.xMax - histModel.xMin) / histModel.binWidth);
        histModel.normedWidth = 1.0f / (float)histModel.numBins;
        List<int> binNums = new List<int>();
        for (var i = 0; i < histModel.numBins; i++)
        {
            List<float> selectedVals = new List<float>();
            for (int j = 0; j < dataObj.df.Rows.Count; j++)
            {
                string stringVal = dataObj.df.Columns[histModel.xName][j].ToString();
                var currVal = float.Parse(stringVal, CultureInfo.InvariantCulture.NumberFormat);
                if(currVal >= histModel.xMin + (histModel.binWidth * i) && currVal < histModel.xMin + (histModel.binWidth * (i + 1)))
                {
                    selectedVals.Add(currVal);
                }
            }
            int val = selectedVals.Count;
            binNums.Add(val);
        }

        histModel.minBin = binNums.Min();
        histModel.maxBin = binNums.Max();

        return binNums;
    }

    public float NormalizeValue(float rawValue, float minValue, float maxValue)
    {
        return (rawValue - minValue)
            / (maxValue - minValue);
    }

    // n is the number of observations
    public float CalcBinWidth(float iqr, float n)
    {
        double exponent = 1.0 / 3.0;
        return (float)(2 * iqr / Math.Pow((double)n, exponent));
    }

    public float ScaleAxes()
    {
        if (!(model is HistModel histModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        return histModel.plotScale + histModel.plotScale * histModel.extraMargin;
    }
}