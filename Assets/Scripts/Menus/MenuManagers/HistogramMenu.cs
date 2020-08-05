using UnityEngine;
public class HistogramMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.HISTOGRAM_GRAPH;
        mH.Register(mE, this);
    }

    public override void Transition(MenuEnum? mE)
    {
        Debug.Log("Create a histogram");
    }
}