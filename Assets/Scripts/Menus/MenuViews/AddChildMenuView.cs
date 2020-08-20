using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AddChildMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject compDDGo;

    [SerializeField]
    GameObject dataObjMenuGo;

    [SerializeField]
    GameObject newBoxMenuGo;

    protected IAbsCompModel compModel;
    protected TMP_Dropdown compDDObj;
    protected Dictionary<Guid, IAbsModel> modelDict;
    protected GameObject next;

    private void Start()
    {
        Setup(MenuEnum.ADD_CHILD, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        compModel = VizUtils.CastToCompModel(iAbsModel);

        mV = menuHandlerGo.GetComponent<MenuView>();
        compDDObj = compDDGo.GetComponent<TMP_Dropdown>();

        if (compModel.superComp != null)
        {
            compModel.availTransfs.AddRange(compModel.superComp.availTransfs);
        }

        List<TMP_Dropdown.OptionData> transfOptions = GetCompOptions();
        compDDObj.options = transfOptions;
    }

    public List<TMP_Dropdown.OptionData> GetCompOptions()
    {
        modelDict = mV.mM.modelDictionary;
        List<Type> compatSubComps = compModel.superComp.compatSubComps;
        List<Guid> keys = mV.mM.modelDictionary.Keys.ToList();
        List<string> optionsList = new List<string>();

        for(int i = 0; i < keys.Count; i++)
        {
            IAbsModel currModel = modelDict[keys[i]];
            List<Type> absModelList = compatSubComps.Where(m => m.IsAssignableFrom(currModel.GetType())).ToList();
            if(absModelList.Count > 0)
            {
                optionsList.Add(currModel.gUID.ToString());
            }
        }
        return CreateOptions(optionsList);
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

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void AddExistingChildBtnClicked()
    {
        Guid selected = Guid.Parse(compDDObj.options[compDDObj.value].text);
        IAbsCompModel selectedModel = VizUtils.CastToCompModel(modelDict[selected]);
        selectedModel.superComp = VizUtils.CastToCompModel(mV.GetCurrModel()).superComp;
        mV.UpdateCurrModel(selectedModel);
        next = dataObjMenuGo;
        PrepForTransition();
    }

    public void AddNewChildBtnClicked()
    {
        next = newBoxMenuGo;
        PrepForTransition();
    }
}
