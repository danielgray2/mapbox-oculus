using System;
using UnityEngine;

public class ChildBaseMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject dataObjMenuGo;

    [SerializeField]
    GameObject removeChildMenuGo;

    protected GameObject next;

    private void Start()
    {
        Setup(MenuEnum.CHILD, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
    }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void CreateNewClicked()
    {
        next = dataObjMenuGo;
        PrepForTransition();
    }

    public void ManagerClicked()
    {
        next = removeChildMenuGo;
        PrepForTransition();
    }
}