using UnityEngine;
public class ParentMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.PARENT;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Create the new parent");
    }
}