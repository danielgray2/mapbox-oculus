
using System;
using UnityEngine;

public class ScatterMenuContr : IAbsMenuContr
{
    public ScatterMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update(){}

    public void UpdateScatterModel(ScatterModel scatterModel)
    {
        model = scatterModel;
    }

    public void updateXName(string text)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.xName = text;
    }

    public void updateYName(string text)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.yName = text;
    }

    public void updateZName(string text)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.zName = text;
    }

    public void updateCompModel(ComposableModel compModel)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.compModel = compModel;
    }

    public void updateExtraMargin(float extraMargin)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.extraMargin = extraMargin;
    }

    public void updatePlotScale(float plotScale)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.plotScale = plotScale;
    }

    public void updateDataPointScale(Vector3 dataPointScale)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.dataPointScale = dataPointScale;
    }

    public void updateNumMarkersPerAxis(int numMarkersPerAxis)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.numMarkersPerAxis = numMarkersPerAxis;
    }

    public void updateParent(GameObject parent)
    {
        ScatterModel scatterModel = CastToScatterModel();
        scatterModel.parent = parent;
    }

    public ScatterModel CastToScatterModel()
    {
        if(!(model is ScatterModel sM))
        {
            throw new ArgumentException("Model must be of type ScatterModel");
        }
        return sM;
    }
}