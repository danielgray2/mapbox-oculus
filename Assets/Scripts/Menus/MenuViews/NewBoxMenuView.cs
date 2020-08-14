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

    private void Start()
    {
        Setup(MenuEnum.NEW_BOX, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
    }

    public void PrepForTransition()
    {
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    public void GraphButtonClicked()
    {
        ComposableModel compModel = CastToCompModel();
        compModel.compType = ComposableType.GRAPH;
        next = graphMenuGo;
        PrepForTransition();
    }

    public void ContextButtonClicked()
    {
        ComposableModel compModel = CastToCompModel();
        compModel.compType = ComposableType.CONTEXT;
        next = contextMenuGo;
        PrepForTransition();
    }

    public void MeshButtonClicked()
    {
        ComposableModel compModel = CastToCompModel();
        compModel.compType = ComposableType.MESH;
        next = meshMenuGo;
        PrepForTransition();
    }

    public void DashButtonClicked()
    {
        ComposableModel compModel = CastToCompModel();
        compModel.compType = ComposableType.DASHBOARD;
        next = dashMenuGo;
        PrepForTransition();
    }

    ComposableModel CastToCompModel()
    {
        ComposableModel compModel;
        if (model is ComposableModel)
        {
            compModel = (ComposableModel)model;
        }
        else if (model.compModel != null)
        {
            compModel = model.compModel;
        }
        else
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }
        return compModel;
    }
}
