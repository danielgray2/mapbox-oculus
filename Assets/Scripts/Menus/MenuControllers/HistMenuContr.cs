using System;
using UnityEngine;

public class HistMenuContr : IAbsMenuContr
{
    public HistMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update(){}

    public void UpdateHistModel(HistModel model)
    {
        this.model = model;
    }

    public void UpdateColName(string newName)
    {
        if (!(model is HistModel histModel))
        {
            throw new ArgumentException("Model must be of type ScatterplotModel");
        }
        histModel.xName = newName;
    }
}
