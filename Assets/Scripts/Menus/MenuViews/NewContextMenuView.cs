using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewContextMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject contextTypeDDGo;

    [SerializeField]
    GameObject mapMenuGo;

    protected TMP_Dropdown contextTypeDDObj;
    protected GameObject next;

    private void Start()
    {
        Setup(MenuEnum.NEW_CONTEXT, menuHandlerGo.GetComponent<MenuView>());
        contextTypeDDObj = contextTypeDDGo.GetComponent<TMP_Dropdown>();
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        contextTypeDDObj.options = GetContextOptions();
    }

    public void PrepForTransition()
    {
        string contextName = contextTypeDDObj.options[contextTypeDDObj.value].text;
        ContextTypeEnum enumVal = ContextDict.stringEnumDict[contextName];
        next = DetermineNext(enumVal);

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public List<TMP_Dropdown.OptionData> GetContextOptions()
    {
        List<string> keys = ContextDict.stringEnumDict.Keys.ToList();
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

    public GameObject DetermineNext(ContextTypeEnum enumVal)
    {
        GameObject retVal;
        switch (enumVal)
        {
            case ContextTypeEnum.MAP:
                retVal = mapMenuGo;
                break;
            default:
                throw new ArgumentException("Graph type not recognized");
        }
        return retVal;
    }
}
