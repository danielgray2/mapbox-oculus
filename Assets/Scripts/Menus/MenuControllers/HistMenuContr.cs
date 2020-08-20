using System;
using UnityEngine;

public class HistMenuContr : IAbsCompContr
{

    public void UpdateColName(IAbsModel model, string newName)
    {
        if (!(model is HistModel histModel))
        {
            throw new ArgumentException("Model must be of type HistModel");
        }
        histModel.xName = newName;
        histModel.modelUpdateEvent.Invoke();
    }
}
