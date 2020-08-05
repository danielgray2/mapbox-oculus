using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    
    void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.GENERAL_MENU;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        if (mE.HasValue)
        {
            Debug.Log("Value: " + mE.Value);
            IAbstractMenu aM = mH.menuDictionary[mE.Value];
            aM.Initiate(new MenuData());
        }
    }
}
