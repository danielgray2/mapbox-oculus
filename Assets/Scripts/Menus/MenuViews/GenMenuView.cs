using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMenuView : IAbsMenuView
{
    [SerializeField]
    public GameObject next;

    [SerializeField]
    GameObject menuHandlerGo;

    private void Start()
    {
        Setup(MenuEnum.GENERAL_MENU, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void PrepForTransition()
    {
        model = new ComposableModel();
        mV.RegisterModel(model.gUID, model);
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }
}