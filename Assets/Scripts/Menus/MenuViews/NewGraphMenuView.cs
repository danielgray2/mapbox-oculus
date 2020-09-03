using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewGraphMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject scatterMenuGo;

    [SerializeField]
    GameObject histogramMenuGo;

    [SerializeField]
    GameObject tensorGlyphMenuGo;

    [SerializeField]
    GameObject graphTypeDDGo;

    protected TMP_Dropdown graphTypeDDObj;
    protected GameObject next;

    private void Start()
    {
        Setup(MenuEnum.NEW_GRAPH, menuHandlerGo.GetComponent<MenuView>());
        graphTypeDDObj = graphTypeDDGo.GetComponent<TMP_Dropdown>();
    }

    public void PrepForTransition()
    {
        string graphName = graphTypeDDObj.options[graphTypeDDObj.value].text;
        GraphTypeEnum enumVal = GraphDict.stringEnumDict[graphName];
        next = DetermineNext(enumVal);

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        graphTypeDDObj.options = GetGraphOptions();
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = GraphDict.stringEnumDict.Keys.ToList();
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

    public GameObject DetermineNext(GraphTypeEnum enumVal)
    {
        GameObject retVal;
        switch (enumVal)
        {
            case GraphTypeEnum.SCATTERPLOT:
                retVal = scatterMenuGo;
                break;
            case GraphTypeEnum.HISTOGRAM:
                retVal = histogramMenuGo;
                break;
            case GraphTypeEnum.TENSOR_GLYPH:
                retVal = tensorGlyphMenuGo;
                break;
            default:
                throw new ArgumentException("Graph type not recognized");
        }
        return retVal;
    }
}