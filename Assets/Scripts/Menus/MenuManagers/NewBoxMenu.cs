using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoxMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        Debug.Log("It is getting set");
        mE = MenuEnum.NEW_BOX;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        IAbstractMenu aM = mH.menuDictionary[mE.Value];
        aM.Initiate(new MenuData());
    }
}
