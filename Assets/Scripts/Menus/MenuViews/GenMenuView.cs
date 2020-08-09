using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMenuView : IAbstractView
{
    [SerializeField]
    public GameObject next;

    [SerializeField]
    GameObject menuHandlerGo;

    protected MenuHandler mH;
    protected MenuEnum mE;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.GENERAL_MENU;
        mH.Register(mE, this.gameObject);
        controller = new GenMenuContr(this);
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
        throw new System.NotImplementedException();
    }

    public void PrepForTransition()
    {
        if(!(controller is GenMenuContr genContr))
        {
            throw new ArgumentException("Controller must be of type GenMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }
        genContr.Transition(nextMenu);
    }
}