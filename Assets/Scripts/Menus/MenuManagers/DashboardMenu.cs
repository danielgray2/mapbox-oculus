
public class DashboardMenu : IAbstractMenu
{
    protected MenuData data;
    public class MenuData : IMenuData { }

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.DASHBOARD;
        mH.Register(mE, this);
    }

    public override void Transition(MenuEnum? mE)
    {
        throw new System.NotImplementedException();
    }
}