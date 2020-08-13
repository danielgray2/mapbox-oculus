using Mapbox.Unity.Map;
using Microsoft.Data.Analysis;
using System;
using System.Collections;
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

    protected MapMenuContr mapMenuContr;

    private void Awake()
    {
        Setup(MenuEnum.MAP, menuHandlerGo.GetComponent<MenuView>());
        controller = mapMenuContr;
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if (!(iAbsModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        MapModel mapModel = new MapModel(compModel);
        mV.RegisterModel(mapModel.gUID, mapModel);
        model = mapModel;
        mapMenuContr = new MapMenuContr(this, model);

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
        MapModel mapModel = CastToMapModel();
        DataObj dataObj = mapModel.compModel.dataObj;
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
        MapModel mapModel = CastToMapModel();
        mapMenuContr.UpdateLatName(latDDObj.options[latDDObj.value].text);
        mapMenuContr.UpdateLonName(lonDDObj.options[lonDDObj.value].text);
        mapMenuContr.UpdateExaggeration(extrusionDDObj.options[extrusionDDObj.value].text);

        GameObject mapGo = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        MapWrapper mapWrapper = mapGo.GetComponent<MapWrapper>();
        mapWrapper.Initialize(mapModel);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    MapModel CastToMapModel()
    {
        if (!(model is MapModel mapModel))
        {
            throw new ArgumentException("Model must be of type MapModel");
        }
        return mapModel;
    }
}
