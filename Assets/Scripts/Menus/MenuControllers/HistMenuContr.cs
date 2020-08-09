using System;
using UnityEngine;

public class HistMenuContr : IAbstractMenu
{
    public HistMenuContr(IAbstractView view) : base(view) { }
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
