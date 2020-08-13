using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChildMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject next;

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
    }

    public void PrepForTransition()
    {

    }
}
