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
    protected ScatterMenuContr scatterContr;

    private void Start()
    {
        Setup(MenuEnum.SCATTERPLOT_GRAPH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if(!(iAbsModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        scatterModel = new ScatterModel(compModel);
        mV.RegisterModel(scatterModel.gUID, scatterModel);
        model = scatterModel;
        scatterContr = new ScatterMenuContr(this, model);
        controller = scatterContr;

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
        scatterContr.updateYName(yDDObj.options[yDDObj.value].text);
        scatterContr.updateZName(zDDObj.options[zDDObj.value].text);

        if (scatterModel.parent != null)
        {
            transform.parent = scatterModel.parent.transform;
        }

        GameObject scatterBoxGo = Instantiate(scatterBoxPrefab, Vector3.zero, Quaternion.identity);
        ScatterBoxWrapper scatterBoxWrapper = scatterBoxGo.GetComponent<ScatterBoxWrapper>();
        scatterBoxWrapper.Initialize(scatterModel);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }
}