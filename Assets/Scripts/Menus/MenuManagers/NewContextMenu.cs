using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewContextMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.NEW_CONTEXT;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Get dropdown and go there");
    }
}
