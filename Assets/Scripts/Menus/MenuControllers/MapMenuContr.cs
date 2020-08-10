using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuContr : IAbstractMenu
{
    public MapMenuContr(IAbstractView view) : base(view) { }

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