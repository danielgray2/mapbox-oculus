using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject meshMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Start()
    {
        Setup(MenuEnum.MESH_BASE, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        controller = new MeshBaseMenuContr(this, model);
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

    public void MeshBtnClicked()
    {
        next = meshMenuGo;
        PrepForTransition();
    }
}