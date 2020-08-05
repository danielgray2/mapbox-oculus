using UnityEngine;
public class ScatterplotMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.SCATTERPLOT_GRAPH;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Create scatterplot");
    }
}