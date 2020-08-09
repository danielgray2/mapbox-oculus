using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScatterMenuView : IAbstractView
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

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected ScatterplotModel scatterModel;
    protected ScatterMenuContr scatterContr;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.SCATTERPLOT_GRAPH;
        mH.Register(mE, this.gameObject);
        scatterContr = new ScatterMenuContr(this);
        controller = scatterContr;
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
        if(!(iModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        scatterModel = new ScatterplotModel(compModel);
        scatterContr.UpdateScatterModel(scatterModel);

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
        DataObj dataObj = scatterModel.compModel.dataObj;
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
        scatterContr.updateXName(xDDObj.options[xDDObj.value].text);
        scatterContr.updateYName(yDDObj.options[xDDObj.value].text);
        scatterContr.updateZName(zDDObj.options[xDDObj.value].text);

        if (scatterModel.parent != null)
        {
            transform.parent = scatterModel.parent.transform;
        }

        GameObject scatterBoxGo = Instantiate(scatterBoxPrefab, Vector3.zero, Quaternion.identity);
        ScatterBoxWrapper scatterBoxWrapper = scatterBoxGo.GetComponent<ScatterBoxWrapper>();
        scatterBoxWrapper.Initialize(scatterModel);

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        scatterContr.Transition(nextMenu);
    }
}