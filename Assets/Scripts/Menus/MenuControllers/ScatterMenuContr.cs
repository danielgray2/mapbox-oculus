
using UnityEngine;

public class ScatterMenuContr : IAbstractMenu
{
    ScatterplotModel scatterModel;
    public ScatterMenuContr(IAbstractView view) : base(view){}

    public override void Initialize(IModel iModel) 
    {
        view.gameObject.SetActive(true);
        view.Initialize(iModel);
    }

    public override void Transition(IAbstractMenu next)
    {
        view.gameObject.SetActive(false);
        next.Initialize(model);
    }

    public override void Update(){}

    public void UpdateScatterModel(ScatterplotModel scatterModel)
    {
        this.scatterModel = scatterModel;
    }

    public void updateXName(string text)
    {
        scatterModel.xName = text;
    }

    public void updateYName(string text)
    {
        scatterModel.yName = text;
    }

    public void updateZName(string text)
    {
        scatterModel.zName = text;
    }

    public void updateCompModel(ComposableModel compModel)
    {
        scatterModel.compModel = compModel;
    }

    public void updateExtraMargin(float extraMargin)
    {
        scatterModel.extraMargin = extraMargin;
    }

    public void updatePlotScale(float plotScale)
    {
        scatterModel.plotScale = plotScale;
    }

    public void updateDataPointScale(Vector3 dataPointScale)
    {
        scatterModel.dataPointScale = dataPointScale;
    }

    public void updateNumMarkersPerAxis(int numMarkersPerAxis)
    {
        scatterModel.numMarkersPerAxis = numMarkersPerAxis;
    }

    public void updateParent(GameObject parent)
    {
        scatterModel.parent = parent;
    }
}