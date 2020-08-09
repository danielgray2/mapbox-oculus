using UnityEngine;

public class HistBaseMenuContr : IAbstractMenu
{
    public HistBaseMenuContr(IAbstractView view) : base(view) { }
    public override void Initialize(IModel iModel)
    {
        model = iModel;
        view.gameObject.SetActive(true);
        view.Initialize(iModel);
    }

    public override void Transition(IAbstractMenu next)
    {
        view.gameObject.SetActive(false);
        next.Initialize(model);
    }

    public override void Update() { }
}