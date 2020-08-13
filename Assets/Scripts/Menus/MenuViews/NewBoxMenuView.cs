using System;
using UnityEngine;

public class NewBoxMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject next;

    protected ComposableModel compModel;

    private void Awake()
    {
        Setup(MenuEnum.NEW_BOX, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void PrepForTransition()
    {
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    public void GraphButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.GRAPH);
        mV.RegisterModel(compModel.gUID, compModel);
        model = compModel;
        PrepForTransition();
    }

    public void ContextButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.CONTEXT);
        mV.RegisterModel(compModel.gUID, compModel);
        model = compModel;
        PrepForTransition();
    }

    public void MeshButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.MESH);
        mV.RegisterModel(compModel.gUID, compModel);
        model = compModel;
        PrepForTransition();
    }

    public void DashButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.DASHBOARD);
        mV.RegisterModel(compModel.gUID, compModel);
        model = compModel;
        PrepForTransition();
    }
}
