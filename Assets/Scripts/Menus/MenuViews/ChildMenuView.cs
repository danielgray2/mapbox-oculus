using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject newBoxGo;

    [SerializeField]
    GameObject removeChildMenuGo;

    protected GameObject next;

    private void Start()
    {
        Setup(MenuEnum.CHILD, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
    }

    public void PrepForTransition()
    {
        if(model != null)
        {
            mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
        }
        else
        {
            mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, Guid.NewGuid()));
        }
    }

    public void CreateNewClicked()
    {
        next = newBoxGo;
        ComposableModel compModel = new ComposableModel();
        //compModel.superComp = model;
        mV.RegisterModel(model.gUID, model);
        PrepForTransition();
    }

    public void ManagerClicked()
    {
        next = removeChildMenuGo;
        PrepForTransition();
    }
}