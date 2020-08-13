using UnityEngine;
public class ScatterBaseMenuContr : IAbsMenuContr
{
    public ScatterBaseMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update(){}
}