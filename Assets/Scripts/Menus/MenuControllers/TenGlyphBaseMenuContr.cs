using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenGlyphBaseMenuContr : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject glyphMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected GameObject next;
    private void Start()
    {
        Setup(MenuEnum.TEN_GLYPH_BASE, menuHandlerGo.GetComponent<MenuView>());
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
        next = glyphMenuGo;
        PrepForTransition();
    }
}
