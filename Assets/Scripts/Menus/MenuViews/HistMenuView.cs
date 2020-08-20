using Microsoft.Data.Analysis;
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

    private void Start()
    {
        Setup(MenuEnum.HISTOGRAM_GRAPH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if(iAbsModel is HistModel)
        {
            histModel = (HistModel)iAbsModel;
        }
        else
        {
            IAbsCompModel compModel = VizUtils.CastToCompModel(iAbsModel);
            histModel = new HistModel();
            histModel.SetValsFromBase(compModel);
            mV.RegisterModel(histModel.gUID, histModel);
            mV.UpdateCurrModel(histModel);
        }

        dDObj = dDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> axisOptions = GetAxisOptions();

        dDObj.options = axisOptions;
    }

    public List<TMP_Dropdown.OptionData> GetAxisOptions()
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(mV.GetCurrModel());
        DataObj dataObj = compModel.dataObj;
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
        mV.UpdateHistColName(dDObj.options[dDObj.value].text);

        // Maybe move this to some sort of composable creator
        GameObject histGo = Instantiate(histPrefab, Vector3.zero, Quaternion.identity);
        
        if (histModel.superComp != null)
        {
            histGo.transform.parent = histModel.parent.transform;
        }

        HistWrapper histWrapper = histGo.GetComponent<HistWrapper>();
        mV.UpdateTransform(histWrapper.transform);
        histWrapper.Initialize(histModel);
        histWrapper.Plot();

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());

    }
}