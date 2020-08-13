
using System;

public class MeshMenuContr : IAbsMenuContr
{
    public MeshMenuContr(IAbstractView view, IAbsModel model) : base(view, model) {}

    public override void Update() { }

    public void UpdateMeshModel(MeshModel meshModel)
    {
        model = meshModel;
    }

    public void UpdateXColName(string newName)
    {
        MeshModel meshModel = CastToMeshModel();
        meshModel.xCol = newName;
    }

    public void UpdateYColName(string newName)
    {
        MeshModel meshModel = CastToMeshModel();
        meshModel.yCol = newName;
    }

    public void UpdateZColName(string newName)
    {
        MeshModel meshModel = CastToMeshModel();
        meshModel.zCol = newName;
    }

    public void UpdateValueColName(string newName)
    {
        MeshModel meshModel = CastToMeshModel();
        meshModel.valueCol = newName;
    }

    MeshModel CastToMeshModel()
    {
        if (!(model is MeshModel meshModel))
        {
            throw new ArgumentException("Model must be of type MeshModel");
        }
        return meshModel;
    }
}