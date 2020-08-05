using System;
using UnityEngine;
public class AddChildMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.ADD_CHILD;
        mH.Register(mE, this);
    }

    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Put in logic to add child");
    }
}