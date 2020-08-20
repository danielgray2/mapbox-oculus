using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Microsoft.Data.Analysis;

public class DataObjMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject dataObjDDGo;

    [SerializeField]
    IAbsMenuView newBoxMenuGo;

    [SerializeField]
    IAbsMenuView transfMenu;

    protected TMP_Dropdown dataDDObj;
    protected string dataObjName;
    protected IAbsCompModel compModel;
    protected string useSuper = "Super Component";
    protected Dictionary<string, DataObj> availSets;
    protected IAbsMenuView next;

    private void Start()
    {
        Setup(MenuEnum.DATA_OBJ, menuHandlerGo.GetComponent<MenuView>());
        dataDDObj = dataObjDDGo.GetComponent<TMP_Dropdown>();
        availSets = new Dictionary<string, DataObj>();
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        compModel = VizUtils.CastToCompModel(iAbsModel);
        availSets = GetAvailDataSets();
        dataDDObj.options = GetGraphOptions();
    }

    public Dictionary<string, DataObj> GetAvailDataSets()
    {
        Dictionary<string, DataObj> retDict = new Dictionary<string, DataObj>();
        List<string> keys = DataStore.Instance.dataDict.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            retDict.Add(keys[i], DataStore.Instance.dataDict[keys[i]]);
        }

        if(compModel.superComp != null)
        {
            retDict.Add(useSuper, compModel.superComp.dataObj);
        }
        return retDict;
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = availSets.Keys.ToList();
        if(compModel.superComp != null)
        {
            keys.Add(useSuper);
        }
        return CreateOptions(keys);
    }

    public void PrepForTransition()
    {
        dataObjName = dataDDObj.options[dataDDObj.value].text;
        DataObj dataObj;
        if (dataObjName == useSuper)
        {
            dataObj = availSets[dataObjName];
        }
        else
        {
            dataObj = new DataObj(availSets[dataObjName].df.Clone());
        }
        mV.UpdateModelDataObj(dataObj);
        mV.ActivateMenu(next);
    }

    public void ContinueBtnPressed()
    {
        if (mV.mM.creatingSubComp)
        {
            next = transfMenu;
        }
        else
        {
            next = newBoxMenuGo;
        }
        PrepForTransition();
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
}
