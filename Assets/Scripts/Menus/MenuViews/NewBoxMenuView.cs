using System;
using UnityEngine;

public class NewBoxMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject next;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected ComposableModel compModel;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.NEW_BOX;
        mH.Register(mE, this.gameObject);
        controller = new NewBoxMenuContr(this);
    }

    public override void Initialize(IModel iModel) { }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void PrepForTransition()
    {
        if(!(controller is NewBoxMenuContr boxContr))
        {
            throw new ArgumentException("Controller must be of type NewBoxMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        boxContr.Transition(nextMenu);
    }

    public void GraphButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.GRAPH);
        controller.model = compModel;
        PrepForTransition();
    }

    public void ContextButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.CONTEXT);
        controller.model = compModel;
        PrepForTransition();
    }

    public void DashButtonClicked()
    {
        compModel = new ComposableModel(ComposableType.DASHBOARD);
        controller.model = compModel;
        PrepForTransition();
    }
}
