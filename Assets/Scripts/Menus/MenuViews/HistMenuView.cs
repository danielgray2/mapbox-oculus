using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistMenuView : IAbsMenuView
{
    [SerializeField]
    public GameObject next;

    [SerializeField]
    GameObject dDGo;

    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject histPrefab;

    protected TMP_Dropdown dDObj;

    protected HistModel histModel;
    protected HistMenuContr histContr;

    private void Start()
    {
        Setup(MenuEnum.HISTOGRAM_GRAPH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if (!(iAbsModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        histModel = new HistModel(compModel);
        mV.RegisterModel(histModel.gUID, histModel);
        histContr = new HistMenuContr(this, histModel);
        controller = histContr;
        model = histModel;

        dDObj = dDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> axisOptions = GetAxisOptions();

        dDObj.options = axisOptions;
    }

    public List<TMP_Dropdown.OptionData> GetAxisOptions()
    {
        DataObj dataObj = histModel.compModel.dataObj;
        List<string> nameList = new List<string>();
        foreach (DataFrameColumn col in dataObj.df.Columns)
        {
            nameList.Add(col.Name);
        }
        return CreateOptions(nameList);
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
        histContr.UpdateColName(dDObj.options[dDObj.value].text);

        GameObject histGo = Instantiate(histPrefab, Vector3.zero, Quaternion.identity);
        
        if (histModel.parent != null)
        {
            histGo.transform.parent = histModel.parent.transform;
        }
        HistWrapper histWrapper = histGo.GetComponent<HistWrapper>();
        histWrapper.Initialize(histModel);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));

    }
}