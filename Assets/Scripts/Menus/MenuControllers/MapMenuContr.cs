using System;

public class MapMenuContr : IAbsMenuContr
{
    public MapMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }

    public void UpdateMapModel(MapModel mapModel)
    {
        this.model = mapModel;
    }

    public void UpdateLatName(string newName)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.latColName = newName;
    }

    public void UpdateLonName(string newName)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.lonColName = newName;
    }

    public void UpdateExaggeration(string newVal)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.exaggerationFactor = int.Parse(newVal);
    }

    MapModel CastToMapModel()
    {
        if (!(model is MapModel mapModel))
        {
            throw new ArgumentException("Model must be of type MapModel");
        }
        return mapModel;
    }
}