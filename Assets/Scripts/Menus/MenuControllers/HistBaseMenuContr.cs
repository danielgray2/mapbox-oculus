using UnityEngine;

public class HistBaseMenuContr : IAbsMenuContr
{
    public HistBaseMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }
}