using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistMenuView : IAbstractView
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

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected HistModel histModel;
    protected HistMenuContr histContr;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.HISTOGRAM_GRAPH;
        mH.Register(mE, this.gameObject);
        histContr = new HistMenuContr(this);
        controller = histContr;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Initialize(IModel iModel)
    {
        if (!(iModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        histModel = new HistModel(compModel);
        histContr.UpdateHistModel(histModel);

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

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        histContr.Transition(nextMenu);
    }
}