using System;
using UnityEngine;

public class NewBoxMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject graphMenuGo;

    [SerializeField]
    GameObject contextMenuGo;

    [SerializeField]
    GameObject dashMenuGo;

    [SerializeField]
    GameObject meshMenuGo;

    protected GameObject next;
    protected IAbsCompModel compModel;

    private void Start()
    {
        Setup(MenuEnum.NEW_BOX, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        compModel = VizUtils.CastToCompModel(iAbsModel);
    }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void GraphButtonClicked()
    {
        compModel.compType = ComposableType.GRAPH;
        next = graphMenuGo;
        PrepForTransition();
    }

    public void ContextButtonClicked()
    {
        compModel.compType = ComposableType.CONTEXT;
        next = contextMenuGo;
        PrepForTransition();
    }

    public void MeshButtonClicked()
    {
        compModel.compType = ComposableType.MESH;
        next = meshMenuGo;
        PrepForTransition();
    }

    public void DashButtonClicked()
    {
        compModel.compType = ComposableType.DASHBOARD;
        next = dashMenuGo;
        PrepForTransition();
    }
}
