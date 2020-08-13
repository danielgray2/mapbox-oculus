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

    private void Awake()
    {
        Setup(MenuEnum.GENERAL_MENU, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void PrepForTransition()
    {
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, Guid.NewGuid()));
    }
}