using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoxMenuView : IAbsMenuView
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
    GameObject meshMenuGo;

    [SerializeField]
    GameObject dataObjDDGo;

    protected GameObject next;
    protected TMP_Dropdown dataDDObj;
    protected string dataObjName;

    private void Awake()
    {
        Setup(MenuEnum.BOX, menuHandlerGo.GetComponent<MenuView>());
        dataDDObj = dataObjDDGo.GetComponent<TMP_Dropdown>();
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        controller = new BoxMenuContr(this, model);
        dataDDObj.options = GetGraphOptions();
    }

    public void PrepForTransition()
    {   
        if (!(controller is BoxMenuContr boxContr))
        {
            throw new ArgumentException("Controller must be of type BoxMenuContr");
        }

        dataObjName = dataDDObj.options[dataDDObj.value].text;
        DataObj dataObj = DataStore.Instance.dataDict[dataObjName];

        boxContr.UpdateModelDataObj(dataObj);
        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
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
        ComposableModel compModel = CastToCompModel();
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
            case ComposableType.MESH:
                next = meshMenuGo;
                break;
            default:
                throw new ArgumentException("Unable to recognize the button clicked");
        }
        PrepForTransition();
    }

    public ComposableModel CastToCompModel()
    {
        if (!(this.model is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }
        return compModel;
    }
}
