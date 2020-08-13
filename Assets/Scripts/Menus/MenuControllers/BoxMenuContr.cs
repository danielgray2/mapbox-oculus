using System;

public class BoxMenuContr : IAbsMenuContr
{
    public BoxMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }

    public void UpdateModelDataObj(DataObj dataObj)
    {
        ComposableModel compModel = CastToCompModel();
        compModel.dataObj = dataObj;
    }

    public ComposableModel CastToCompModel()
    {
        if(!(model is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type COmposableModel");
        }
        return compModel;
    }
}
