using Mapbox.Unity.Map;
using Microsoft.Data.Analysis;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DepLatLonMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject depthDDGo;

    [SerializeField]
    GameObject latDDGo;

    [SerializeField]
    GameObject lonDDGo;

    [SerializeField]
    GameObject mapDDGo;

    [SerializeField]
    GameObject menuHandlerGo;

    protected TMP_Dropdown depthDDObj;
    protected TMP_Dropdown latDDObj;
    protected TMP_Dropdown lonDDObj;
    protected TMP_Dropdown mapDDObj;

    protected LatLonTransf transf;
    protected IAbsCompModel compModel;
    protected string thisMapOpt = "This map";
    protected string superMapOpt = "Parent map";

    protected Dictionary<string, AbstractMap> mapDict;
    protected IAbsMenuView next;

    private void Start()
    {
        Setup(MenuEnum.LAT_LON, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        mapDict = new Dictionary<string, AbstractMap>();

        compModel = VizUtils.CastToCompModel(iAbsModel);
        depthDDObj = depthDDGo.GetComponent<TMP_Dropdown>();
        latDDObj = latDDGo.GetComponent<TMP_Dropdown>();
        lonDDObj = lonDDGo.GetComponent<TMP_Dropdown>();
        mapDDObj = mapDDGo.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> axisOptions = GetAxisOptions();

        depthDDObj.options = axisOptions;
        latDDObj.options = axisOptions;
        lonDDObj.options = axisOptions;

        SetMapOptions();
        mapDDObj.options = CreateOptions(mapDict.Keys.ToList());
    }

    public void SetMapOptions()
    {
        if (compModel is MapModel thisMapModel)
        {
            mapDict.Add(thisMapOpt, thisMapModel.absMap);
        }

        if (compModel.superComp != null)
        {
            if (compModel.superComp != null)
            {
                if (compModel.superComp is MapModel superMapModel)
                {
                    mapDict.Add(superMapOpt, superMapModel.absMap);
                }
            }
        }
    }

    public List<TMP_Dropdown.OptionData> GetAxisOptions()
    {
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
        string depthName = depthDDObj.options[depthDDObj.value].text;
        string latName = latDDObj.options[latDDObj.value].text;
        string lonName = lonDDObj.options[lonDDObj.value].text;
        string mapName = mapDDObj.options[mapDDObj.value].text;
        AbstractMap selectedMap = mapDict[mapName];

        mV.AddTransfToUse(new DepthLatLonTransf(depthName, latName, lonName, selectedMap));

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void ContinueBtnClicked()
    {
        SetNext();
        PrepForTransition();
    }

    public void SetNext()
    {
        if (mV.mM.transfsToProc.Count > 0)
        {
            next = mV.FindFilterMenu(mV.mM.transfsToProc[0]);
            mV.RemoveTransfToProc(mV.mM.transfsToProc[0]);
        }
        else
        {
            next = mV.FindConfigMenu(compModel);
        }
    }
}
