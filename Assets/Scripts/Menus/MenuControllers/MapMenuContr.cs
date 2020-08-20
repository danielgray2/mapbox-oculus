using System;

public class MapMenuContr : IAbsCompContr
{
    public void UpdateLatName(IAbsModel model, string newName)
    {
        MapModel mapModel = CastToMapModel(model);
        mapModel.latColName = newName;
        mapModel.modelUpdateEvent.Invoke();
    }

    public void UpdateLonName(IAbsModel model, string newName)
    {
        MapModel mapModel = CastToMapModel(model);
        mapModel.lonColName = newName;
        mapModel.modelUpdateEvent.Invoke();
    }

    public void UpdateExaggeration(IAbsModel model, string newVal)
    {
        MapModel mapModel = CastToMapModel(model);
        mapModel.exaggerationFactor = int.Parse(newVal);
        mapModel.modelUpdateEvent.Invoke();
    }

    MapModel CastToMapModel(IAbsModel model)
    {
        if (!(model is MapModel mapModel))
        {
            throw new ArgumentException("Model must be of type MapModel");
        }
        return mapModel;
    }
}