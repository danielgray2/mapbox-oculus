using System;

public class BoxMenuContr : IAbsMenuContr
{
    public void UpdateModelDataObj(IAbsCompModel compModel, DataObj dataObj)
    {
        compModel.dataObj = dataObj;
    }
}
