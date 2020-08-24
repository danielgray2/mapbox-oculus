using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensorGlyph : IAbstractGraph
{
    public TensorGlyph(TensorGlyphModel model)
    {
        this.model = model;
        GraphStore.Instance.graphList.Add(this);
        DataObj dataObj = model.dataObj;

        model.axisOneMax = dataObj.GetMax(model.axisOneName);
        model.axisTwoMax = dataObj.GetMax(model.axisTwoName);
        model.axisThreeMax = dataObj.GetMax(model.axisThreeName);

        model.axisOneMin = dataObj.GetMin(model.axisOneName);
        model.axisTwoMin = dataObj.GetMin(model.axisTwoName);
        model.axisThreeMin = dataObj.GetMin(model.axisThreeName);

        maxDpSize = new Vector3(0.15f, 0.15f, 0.15f);
        minDpSize = new Vector3(0.01f, 0.01f, 0.01f);
    }

    // Format of inner list (xPos, yPos, zPos, valOne, xComp, yComp, zComp, valTwo, ...)
    // Normalize: (val - min) / (max - min)
    public List<List<float>> CreateGlyphs()
    {
        if(!(model is TensorGlyphModel tGModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }

        List<List<float>> retList = new List<List<float>>();
        DataObj dataObj = tGModel.dataObj;

        for (var i = 0; i < dataObj.df.Rows.Count; i++)
        {
            List<float> currList = new List<float>();
            DataFrame df = tGModel.dataObj.df;

            currList.Add((float)df.Columns[tGModel.xPosColName][i]);
            currList.Add((float)df.Columns[tGModel.yPosColName][i]);
            currList.Add((float)df.Columns[tGModel.zPosColName][i]);

            currList.Add((float)df.Columns[tGModel.axisOneName][i]);
            currList.Add((float)df.Columns[tGModel.oneCompAxisOneName][i]);
            currList.Add((float)df.Columns[tGModel.oneCompAxisTwoName][i]);
            currList.Add((float)df.Columns[tGModel.oneCompAxisThreeName][i]);

            currList.Add((float)df.Columns[tGModel.axisTwoName][i]);
            currList.Add((float)df.Columns[tGModel.twoCompAxisOneName][i]);
            currList.Add((float)df.Columns[tGModel.twoCompAxisTwoName][i]);
            currList.Add((float)df.Columns[tGModel.twoCompAxisThreeName][i]);

            currList.Add((float)df.Columns[tGModel.axisThreeName][i]);
            currList.Add((float)df.Columns[tGModel.threeCompAxisOneName][i]);
            currList.Add((float)df.Columns[tGModel.threeCompAxisTwoName][i]);
            currList.Add((float)df.Columns[tGModel.threeCompAxisThreeName][i]);

            retList.Add(currList);
        }
        return retList;
    }

    public float NormalizeValue(float rawValue, float minValue, float maxValue)
    {
        return (rawValue - minValue)
            / (maxValue - minValue);
    }
}