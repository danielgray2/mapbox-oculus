using UnityEngine;

public class HistBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject histMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Start()
    {
        Setup(MenuEnum.HISTOGRAM_BASE, menuHandlerGo.GetComponent<MenuView>());
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

    public void HistBtnClicked()
    {
        next = histMenuGo;
        PrepForTransition();
    }
}