using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsMenuView : IAbstractView
{
    protected MenuView mV;
    public MenuEnum mE { get; set; }

    public void Setup(MenuEnum mE, MenuView mV)
    {
        this.mV = mV;
        this.mE = mE;
        mV.RegisterMenu(mE, this);
    }

    public abstract void Initialize(IAbsModel iAbsModel);
}
