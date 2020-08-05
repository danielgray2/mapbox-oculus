using UnityEngine;
public class NewGraphMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.NEW_GRAPH;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Get the dropdown and go there");
    }
}