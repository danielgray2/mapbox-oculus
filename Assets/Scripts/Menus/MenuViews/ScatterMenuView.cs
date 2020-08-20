using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScatterMenuView : IAbsMenuView
{
    [SerializeField]
    public GameObject next;

    [SerializeField]
    GameObject xDDGo;

    [SerializeField]
    GameObject yDDGo;

    [SerializeField]
    GameObject zDDGo;

    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject scatterBoxPrefab;

    protected TMP_Dropdown xDDObj;
    protected TMP_Dropdown yDDObj;
    protected TMP_Dropdown zDDObj;

    protected ScatterModel scatterModel;

    private void Start()
    {
        Setup(MenuEnum.SCATTERPLOT_GRAPH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        scatterModel = new ScatterModel();
        IAbsCompModel compModel = VizUtils.CastToCompModel(iAbsModel);
        scatterModel.SetValsFromBase(compModel);
        mV.RegisterModel(scatterModel.gUID, scatterModel);
        mV.UpdateCurrModel(scatterModel);

        xDDObj = xDDGo.GetComponent<TMP_Dropdown>();
        yDDObj = yDDGo.GetComponent<TMP_Dropdown>();
        zDDObj = zDDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> axisOptions = GetAxisOptions();

        xDDObj.options = axisOptions;
        yDDObj.options = axisOptions;
        zDDObj.options = axisOptions;
    }

    public List<TMP_Dropdown.OptionData> GetAxisOptions()
    {
        DataObj dataObj = scatterModel.dataObj;
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
        mV.UpdateScatterXName(xDDObj.options[xDDObj.value].text);
        mV.UpdateScatterYName(yDDObj.options[yDDObj.value].text);
        mV.UpdateScatterZName(zDDObj.options[zDDObj.value].text);

        // Probably should move this at some point
        if (scatterModel.parent != null)
        {
            transform.parent = scatterModel.parent.transform;
        }

        GameObject scatterBoxGo = Instantiate(scatterBoxPrefab, Vector3.zero, Quaternion.identity);
        ScatterBoxWrapper scatterBoxWrapper = scatterBoxGo.GetComponent<ScatterBoxWrapper>();
        mV.UpdateTransform(scatterBoxWrapper.transform);
        scatterBoxWrapper.Initialize(scatterModel);
        scatterBoxWrapper.Plot();

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }
}