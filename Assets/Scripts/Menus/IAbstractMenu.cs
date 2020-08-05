using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IAbstractMenu : MonoBehaviour
{
    [SerializeField]
    protected GameObject menuHandlerGo;

    protected MenuEnum mE;
    protected MenuHandler mH;

    public MenuEnum? GetMenuEnum()
    {
        return mE;
    }

    public virtual void Initiate(IMenuData mD)
    {
        mH.ActivateMenu(this.gameObject);
    }

    public abstract void Transition(MenuEnum? mE);
}
