using Mapbox.Unity.Map;
using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapMenuView : IAbstractView
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

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected MapMenuContr mapMenuContr;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.MAP;
        mH.Register(mE, this.gameObject);
        mapMenuContr = new MapMenuContr(this);
        controller = mapMenuContr;
    }

    public override void Initialize(IModel iModel)
    {
        if (!(iModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        MapModel mapModel = new MapModel(compModel);
        model = mapModel;
        mapMenuContr.UpdateMapModel(mapModel);

        latDDObj = latDDGo.GetComponent<TMP_Dropdown>();
        lonDDObj = lonDDGo.GetComponent<TMP_Dropdown>();
        extrusionDDObj = extrusionDDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> coordOptions = GetCoordOptions();
        List<TMP_Dropdown.OptionData> extrusionOptions = GetExtrusionOptions();

        latDDObj.options = coordOptions;
        lonDDObj.options = coordOptions;
        extrusionDDObj.options = extrusionOptions;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        MapModel mapModel = CastToMapModel();
        DataObj dataObj = mapModel.compModel.dataObj;
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

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        mapMenuContr.Transition(nextMenu);
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
