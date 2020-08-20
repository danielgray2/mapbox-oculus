using System;

public class DataObjMenuContr : IAbsMenuContr
{

    public void UpdateModelDataObj(IAbsModel compModel, DataObj dataObj)
    {
        IAbsCompModel model = VizUtils.CastToCompModel(compModel);
        model.dataObj = dataObj;
    }
}
