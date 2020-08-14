using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class DataObjMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject dataObjDDGo;

    [SerializeField]
    GameObject next;

    protected TMP_Dropdown dataDDObj;
    protected string dataObjName;

    private void Start()
    {
        Setup(MenuEnum.DATA_OBJ, menuHandlerGo.GetComponent<MenuView>());
        dataDDObj = dataObjDDGo.GetComponent<TMP_Dropdown>();
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        controller = new DataObjMenuContr(this, model);
        dataDDObj.options = GetGraphOptions();
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = DataStore.Instance.dataDict.Keys.ToList();
        return CreateOptions(keys);
    }

    public void PrepForTransition()
    {
        if (!(controller is DataObjMenuContr dataContr))
        {
            throw new ArgumentException("Controller must be of type BoxMenuContr");
        }

        dataObjName = dataDDObj.options[dataDDObj.value].text;
        DataObj dataObj = DataStore.Instance.dataDict[dataObjName];
        dataContr.UpdateModelDataObj(dataObj);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
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

    public ComposableModel CastToCompModel()
    {
        ComposableModel compModel;
        if (model is ComposableModel)
        {
            compModel = (ComposableModel)model;
        }
        else if (model.compModel != null)
        {
            compModel = model.compModel;
        }
        else
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }
        return compModel;
    }
}
