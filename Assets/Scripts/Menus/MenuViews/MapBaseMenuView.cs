using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject mapMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Awake()
    {
        Setup(MenuEnum.MAP_BASE, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        controller = new MapBaseMenuContr(this, model);
    }

    public void PrepForTransition()
    {
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    public void BoxBtnClicked()
    {
        next = boxMenuGo;
        PrepForTransition();
    }

    public void MapBtnClicked()
    {
        next = mapMenuGo;
        PrepForTransition();
    }
}
