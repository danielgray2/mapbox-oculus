using UnityEngine;
public class ScatterplotBaseMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.SCATTERPLOT_BASE;
        mH.Register(mE, this);
    }
    public override void Transition(MenuEnum? mE)
    {
        IAbstractMenu aM = mH.menuDictionary[mE.Value];
        aM.Initiate(new MenuData());
    }
}