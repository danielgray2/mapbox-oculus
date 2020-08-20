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
        IAbsCompModel compModel = new BaseCompModel();
        mV.RegisterModel(compModel.gUID, compModel);
        mV.UpdateCurrModel(compModel);
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }
}