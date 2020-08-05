using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.MAP;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Create a map logic");
    }
}
