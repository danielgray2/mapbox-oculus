using UnityEngine;

public class MapBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject mapMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Start()
    {
        Setup(MenuEnum.MAP_BASE, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
    }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void BoxBtnClicked()
    {
        next = boxMenuGo;
        PrepForTransition();
    }

    public void MapBtnClicked()
    {
        next = mapMenuGo;
        PrepForTransition();
    }
}
