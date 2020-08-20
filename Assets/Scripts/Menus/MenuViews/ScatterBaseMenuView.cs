using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject scatterMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Start()
    {
        Setup(MenuEnum.SCATTERPLOT_BASE, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void BoxBtnClicked()
    {
        next = boxMenuGo;
        PrepForTransition();
    }

    public void ScatterBtnClicked()
    {
        next = scatterMenuGo;
        PrepForTransition();
    }
}