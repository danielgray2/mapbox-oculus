using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject next;

    [SerializeField]
    GameObject latDDGo;

    [SerializeField]
    GameObject lonDDGo;

    [SerializeField]
    GameObject extrusionDDGo;

    [SerializeField]
    GameObject mapPrefab;

    protected TMP_Dropdown latDDObj;
    protected TMP_Dropdown lonDDObj;
    protected TMP_Dropdown extrusionDDObj;
    protected MapModel mapModel;

    private void Start()
    {
        Setup(MenuEnum.MAP, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(iAbsModel);
        mapModel = new MapModel();
        mapModel.SetValsFromBase(compModel);
        mV.RegisterModel(mapModel.gUID, mapModel);
        mV.UpdateCurrModel(mapModel);

        latDDObj = latDDGo.GetComponent<TMP_Dropdown>();
        lonDDObj = lonDDGo.GetComponent<TMP_Dropdown>();
        extrusionDDObj = extrusionDDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> coordOptions = GetCoordOptions();
        List<TMP_Dropdown.OptionData> extrusionOptions = GetExtrusionOptions();

        latDDObj.options = coordOptions;
        lonDDObj.options = coordOptions;
        extrusionDDObj.options = extrusionOptions;
    }

    public List<TMP_Dropdown.OptionData> GetCoordOptions()
    {
        DataObj dataObj = mapModel.dataObj;
        List<string> nameList = new List<string>();
        foreach (DataFrameColumn col in dataObj.df.Columns)
        {
            nameList.Add(col.Name);
        }
        return CreateOptions(nameList);
    }

    public List<TMP_Dropdown.OptionData> GetExtrusionOptions()
    {
        List<string> optionsList = new List<string>();
        for(int i = 1; i < 10; i++)
        {
            optionsList.Add(i.ToString());
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
        mV.UpdateMapLatName(latDDObj.options[latDDObj.value].text);
        mV.UpdateMapLonName(lonDDObj.options[lonDDObj.value].text);
        mV.UpdateMapExaggeration(extrusionDDObj.options[extrusionDDObj.value].text);

        // Maybe move this
        GameObject mapGo = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        MapWrapper mapWrapper = mapGo.GetComponent<MapWrapper>();
        mV.UpdateTransform(mapWrapper.transform);
        mapWrapper.Initialize(mapModel);
        mapWrapper.Plot();

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }
}
