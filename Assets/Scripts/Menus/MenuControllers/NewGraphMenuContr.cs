using System;
using UnityEngine;
public class NewGraphMenuContr : IAbsMenuContr
{
    public NewGraphMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }
}