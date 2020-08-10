using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject mapMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected GameObject next;
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.MAP_BASE;
        mH.Register(mE, this.gameObject);
        controller = new MapBaseMenuContr(this);
    }

    public override void Initialize(IModel iModel)
    {
        model = iModel;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrepForTransition()
    {
        if (!(controller is MapBaseMenuContr baseContr))
        {
            throw new ArgumentException("Controller must be of type MapBaseMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        baseContr.Transition(nextMenu);
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
