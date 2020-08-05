using UnityEngine;
public class DataPointMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.DATA_POINT;
        mH.Register(mE, this);
    }

    public override void Transition(MenuEnum? mE)
    {
        throw new System.NotImplementedException();
    }
}