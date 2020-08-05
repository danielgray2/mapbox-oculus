using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.BOX;
        mH.Register(mE, this);
    }

    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Logic to create box");
    }
}
