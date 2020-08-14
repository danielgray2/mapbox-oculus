using Microsoft.Data.Analysis;
using OVR;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeshMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject next;

    [SerializeField]
    GameObject xDDGo;

    [SerializeField]
    GameObject yDDGo;

    [SerializeField]
    GameObject valueDDGo;

    [SerializeField]
    GameObject zDDGo;

    [SerializeField]
    GameObject meshVizPrefab;

    protected TMP_Dropdown xDDObj;
    protected TMP_Dropdown yDDObj;
    protected TMP_Dropdown zDDObj;
    protected TMP_Dropdown valueDDObj;

    protected MeshMenuContr meshMenuContr;

    private void Start()
    {
        Setup(MenuEnum.MESH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if (!(iAbsModel is ComposableModel compModel))
        {
            throw new ArgumentException("Model must be of type ComposableModel");
        }

        MeshModel meshModel = new MeshModel(compModel);
        mV.RegisterModel(meshModel.gUID, meshModel);
        model = meshModel;
        meshMenuContr = new MeshMenuContr(this, model);
        controller = meshMenuContr;

        xDDObj = xDDGo.GetComponent<TMP_Dropdown>();
        yDDObj = yDDGo.GetComponent<TMP_Dropdown>();
        zDDObj = zDDGo.GetComponent<TMP_Dropdown>();
        valueDDObj = valueDDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> options = GetOptions();

        xDDObj.options = options;
        yDDObj.options = options;
        zDDObj.options = options;
        valueDDObj.options = options;
    }

    public List<TMP_Dropdown.OptionData> GetOptions()
    {
        MeshModel meshModel = CastToMeshModel();
        DataObj dataObj = meshModel.compModel.dataObj;
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
        MeshModel meshModel = CastToMeshModel();
        meshMenuContr.UpdateXColName(xDDObj.options[xDDObj.value].text);
        meshMenuContr.UpdateYColName(yDDObj.options[yDDObj.value].text);
        meshMenuContr.UpdateZColName(zDDObj.options[zDDObj.value].text);
        meshMenuContr.UpdateValueColName(valueDDObj.options[valueDDObj.value].text);

        GameObject meshGo = Instantiate(meshVizPrefab, Vector3.zero, Quaternion.identity);
        MeshVizWrapper meshWrapper = meshGo.GetComponent<MeshVizWrapper>();
        meshWrapper.Initialize(meshModel);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    MeshModel CastToMeshModel()
    {
        if (!(model is MeshModel meshModel))
        {
            throw new ArgumentException("Model must be of type MeshModel");
        }
        return meshModel;
    }
}
