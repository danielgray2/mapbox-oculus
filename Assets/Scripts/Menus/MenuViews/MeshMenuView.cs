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

    protected MeshModel meshModel;

    private void Start()
    {
        Setup(MenuEnum.MESH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(iAbsModel);

        meshModel = new MeshModel();
        meshModel.SetValsFromBase(compModel);
        mV.RegisterModel(meshModel.gUID, meshModel);
        mV.UpdateCurrModel(meshModel);

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
        DataObj dataObj = meshModel.dataObj;
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
        mV.UpdateMeshXColName(xDDObj.options[xDDObj.value].text);
        mV.UpdateMeshYColName(yDDObj.options[yDDObj.value].text);
        mV.UpdateMeshZColName(zDDObj.options[zDDObj.value].text);
        mV.UpdateMeshValColName(valueDDObj.options[valueDDObj.value].text);

        // Again, maybe move this
        GameObject meshGo = Instantiate(meshVizPrefab, Vector3.zero, Quaternion.identity);
        MeshVizWrapper meshWrapper = meshGo.GetComponent<MeshVizWrapper>();
        mV.UpdateTransform(meshWrapper.transform);
        meshWrapper.Initialize(meshModel);
        meshWrapper.Plot();

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }
}
