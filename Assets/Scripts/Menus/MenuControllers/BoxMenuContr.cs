using System;
using UnityEngine;

public class BoxMenuContr : IAbstractMenu
{
    ComposableModel compModel;
    public BoxMenuContr(IAbstractView view) : base(view) { }

    public override void Initialize(IModel iModel)
    {
        model = iModel;
        view.gameObject.SetActive(true);
        view.Initialize(iModel);

        if (!(iModel is ComposableModel compModel))
        {
            throw new ArgumentException("Controller must be of type BoxMenuContr");
        }

        this.compModel = compModel;
    }

    public override void Transition(IAbstractMenu next)
    {
        view.gameObject.SetActive(false);
        next.Initialize(model);
    }

    public override void Update() { }

    public void UpdateModelDataObj(DataObj dataObj)
    {
        this.compModel.dataObj = dataObj;
    }
}
