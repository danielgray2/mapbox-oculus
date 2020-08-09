using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoxMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject configDPMenuGo;

    [SerializeField]
    GameObject transfsMenuGo;

    [SerializeField]
    GameObject parentMenuGo;

    [SerializeField]
    GameObject childrenMenuGo;

    [SerializeField]
    GameObject graphMenuGo;

    [SerializeField]
    GameObject contextMenuGo;

    [SerializeField]
    GameObject dashMenuGo;

    [SerializeField]
    GameObject dataObjDDGo;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected ComposableModel compModel;
    protected GameObject next;
    protected TMP_Dropdown dataDDObj;
    protected string dataObjName;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.BOX;
        mH.Register(mE, this.gameObject);
        controller = new BoxMenuContr(this);
        dataDDObj = dataObjDDGo.GetComponent<TMP_Dropdown>();
    }

    public override void Initialize(IModel iModel)
    {
        this.model = iModel;
        if(!(this.model is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }
        this.compModel = compModel;
        dataDDObj.options = GetGraphOptions();
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void PrepForTransition()
    {   
        if (!(controller is BoxMenuContr boxContr))
        {
            throw new ArgumentException("Controller must be of type BoxMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        dataObjName = dataDDObj.options[dataDDObj.value].text;
        DataObj dataObj = DataStore.Instance.dataDict[dataObjName];

        boxContr.UpdateModelDataObj(dataObj);
        boxContr.Transition(nextMenu);
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = DataStore.Instance.dataDict.Keys.ToList();
        return CreateOptions(keys);
    }

    public List<TMP_Dropdown.OptionData> CreateOptions(List<string> stringList)
    {
        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();
        foreach (string item in stringList)
        {
            optionData.Add(new TMP_Dropdown.OptionData(item));
        }

        return optionData;
    }

    public void ConfigDPBtnClicked()
    {
        next = configDPMenuGo;
        PrepForTransition();
    }

    public void TransfsBtnClicked()
    {
        next = transfsMenuGo;
        PrepForTransition();
    }

    public void ParentBtnClicked()
    {
        next = parentMenuGo;
        PrepForTransition();
    }

    public void ChildrenBtnClicked()
    {
        next = childrenMenuGo;
        PrepForTransition();
    }

    public void ContinueBtnClicked()
    {
        switch (compModel.compType)
        {
            case ComposableType.CONTEXT:
                next = contextMenuGo;
                break;
            case ComposableType.DASHBOARD:
                next = dashMenuGo;
                break;
            case ComposableType.GRAPH:
                next = graphMenuGo;
                break;
            default:
                throw new ArgumentException("Unable to recognize the button clicked");
        }
        PrepForTransition();
    }
}
