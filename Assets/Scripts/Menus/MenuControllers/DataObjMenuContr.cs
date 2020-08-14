using System;

public class DataObjMenuContr : IAbsMenuContr
{
    public DataObjMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }

    public void UpdateModelDataObj(DataObj dataObj)
    {
        ComposableModel compModel = CastToCompModel();
        compModel.dataObj = dataObj;
    }

    public ComposableModel CastToCompModel()
    {
        ComposableModel compModel;
        if (model is ComposableModel)
        {
            compModel = (ComposableModel)model;
        }
        else if (model.compModel != null)
        {
            compModel = model.compModel;
        }
        else
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }
        return compModel;
    }
}
