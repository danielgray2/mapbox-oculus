using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistBaseMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject histMenuGo;

    [SerializeField]
    GameObject boxMenuGo;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected GameObject next;
    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.HISTOGRAM_BASE;
        mH.Register(mE, this.gameObject);
        controller = new HistBaseMenuContr(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Initialize(IModel iModel)
    {
        model = iModel;
    }

    public void PrepForTransition()
    {
        if (!(controller is HistBaseMenuContr baseContr))
        {
            throw new ArgumentException("Controller must be of type HistBaseMenuContr");
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

    public void HistBtnClicked()
    {
        next = histMenuGo;
        PrepForTransition();
    }
}