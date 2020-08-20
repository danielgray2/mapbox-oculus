
using System;

public class MeshMenuContr : IAbsCompContr
{
    public void UpdateXColName(IAbsModel model, string newName)
    {
        MeshModel meshModel = CastToMeshModel(model);
        meshModel.xCol = newName;
        meshModel.modelUpdateEvent.Invoke();
    }

    public void UpdateYColName(IAbsModel model, string newName)
    {
        MeshModel meshModel = CastToMeshModel(model);
        meshModel.yCol = newName;
        meshModel.modelUpdateEvent.Invoke();
    }

    public void UpdateZColName(IAbsModel model, string newName)
    {
        MeshModel meshModel = CastToMeshModel(model);
        meshModel.zCol = newName;
        meshModel.modelUpdateEvent.Invoke();
    }

    public void UpdateValColName(IAbsModel model, string newName)
    {
        MeshModel meshModel = CastToMeshModel(model);
        meshModel.valueCol = newName;
        meshModel.modelUpdateEvent.Invoke();
    }

    MeshModel CastToMeshModel(IAbsModel model)
    {
        if (!(model is MeshModel meshModel))
        {
            throw new ArgumentException("Model must be of type MeshModel");
        }
        return meshModel;
    }
}