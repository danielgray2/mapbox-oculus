using System;
using UnityEngine;
public class NewGraphMenuContr : IAbstractMenu
{
    public NewGraphMenuContr(IAbstractView view) : base(view) { }
    public override void Initialize(IModel iModel)
    {
        view.gameObject.SetActive(true);
        model = iModel;
        view.Initialize(iModel);
    }

    public override void Transition(IAbstractMenu next)
    {
        view.gameObject.SetActive(false);
        next.Initialize(model);
    }

    public override void Update() { }
}