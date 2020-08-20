
using System;
using UnityEngine;

public class ScatterMenuContr : IAbsCompContr
{
    public void UpdateXName(IAbsModel model, string text)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.xName = text;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateYName(IAbsModel model, string text)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.yName = text;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateZName(IAbsModel model, string text)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.zName = text;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateExtraMargin(IAbsModel model, float extraMargin)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.extraMargin = extraMargin;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdatePlotScale(IAbsModel model, float plotScale)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.plotScale = plotScale;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateDataPointScale(IAbsModel model, Vector3 dataPointScale)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.dataPointScale = dataPointScale;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateNumMarkersPerAxis(IAbsModel model, int numMarkersPerAxis)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.numMarkersPerAxis = numMarkersPerAxis;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public void UpdateParent(IAbsModel model, GameObject parent)
    {
        ScatterModel scatterModel = CastToScatterModel(model);
        scatterModel.parent = parent;
        scatterModel.modelUpdateEvent.Invoke();
    }

    public ScatterModel CastToScatterModel(IAbsModel iAbsModel)
    {
        if(!(iAbsModel is ScatterModel sM))
        {
            throw new ArgumentException("Model must be of type ScatterModel");
        }
        return sM;
    }
}