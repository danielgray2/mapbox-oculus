using Microsoft.Data.Analysis;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TenGlyphMenuView : IAbsMenuView
{
    [SerializeField]
    public GameObject next;

    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject glyphPrefab;

    protected TenGlyphModel glyphModel;
    /*
    [SerializeField]
    GameObject xPosDDGo;

    [SerializeField]
    GameObject yPosDDGo;

    [SerializeField]
    GameObject zPosDDGo;

    [SerializeField]
    GameObject axisOneDDGo;

    [SerializeField]
    GameObject axisTwoDDGo;

    [SerializeField]
    GameObject axisThreeDDGo;

    [SerializeField]
    GameObject oneCompAxisOneDDGo;

    [SerializeField]
    GameObject oneCompAxisTwoDDGo;

    [SerializeField]
    GameObject oneCompAxisThreeDDGo;

    [SerializeField]
    GameObject twoCompAxisOneDDGo;

    [SerializeField]
    GameObject twoCompAxisTwoDDGo;

    [SerializeField]
    GameObject twoCompAxisThreeDDGo;

    [SerializeField]
    GameObject threeCompAxisOneDDGo;

    [SerializeField]
    GameObject threeCompAxisTwoDDGo;

    [SerializeField]
    GameObject threeCompAxisThreeDDGo;

    protected TMP_Dropdown xPosDDObj;
    protected TMP_Dropdown yPosDDObj;
    protected TMP_Dropdown zPosDDObj;
    protected TMP_Dropdown axisOneDDObj;
    protected TMP_Dropdown axisTwoDDObj;
    protected TMP_Dropdown axisThreeDDObj;
    protected TMP_Dropdown oneCompAxisOneDDObj;
    protected TMP_Dropdown oneCompAxisTwoDDObj;
    protected TMP_Dropdown oneCompAxisThreeDDObj;
    protected TMP_Dropdown twoCompAxisOneDDObj;
    protected TMP_Dropdown twoCompAxisTwoDDObj;
    protected TMP_Dropdown twoCompAxisThreeDDObj;
    protected TMP_Dropdown threeCompAxisOneDDObj;
    protected TMP_Dropdown threeCompAxisTwoDDObj;
    protected TMP_Dropdown threeCompAxisThreeDDObj;
    */

    private void Start()
    {
        Setup(MenuEnum.NEW_TEN_GLYPH_GRAPH, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        if (iAbsModel is TenGlyphModel)
        {
            Debug.Log("Came inside the if");
            glyphModel = (TenGlyphModel)iAbsModel;
        }
        else
        {
            IAbsCompModel compModel = VizUtils.CastToCompModel(iAbsModel);
            glyphModel = new TenGlyphModel();
            glyphModel.SetValsFromBase(compModel);
            mV.RegisterModel(glyphModel.gUID, glyphModel);
            mV.UpdateCurrModel(glyphModel);
        }

        /*
        xPosDDObj = xPosDDGo.GetComponent<TMP_Dropdown>();
        yPosDDObj = yPosDDGo.GetComponent<TMP_Dropdown>();
        zPosDDObj = zPosDDGo.GetComponent<TMP_Dropdown>();
        axisOneDDObj = axisOneDDGo.GetComponent<TMP_Dropdown>();
        axisTwoDDObj = axisTwoDDGo.GetComponent<TMP_Dropdown>();
        axisThreeDDObj = axisThreeDDGo.GetComponent<TMP_Dropdown>();
        oneCompAxisOneDDObj = oneCompAxisOneDDGo.GetComponent<TMP_Dropdown>();
        oneCompAxisTwoDDObj = oneCompAxisTwoDDGo.GetComponent<TMP_Dropdown>();
        oneCompAxisThreeDDObj = oneCompAxisThreeDDGo.GetComponent<TMP_Dropdown>();
        twoCompAxisOneDDObj = twoCompAxisOneDDGo.GetComponent<TMP_Dropdown>();
        twoCompAxisTwoDDObj = twoCompAxisTwoDDGo.GetComponent<TMP_Dropdown>();
        twoCompAxisThreeDDObj = twoCompAxisThreeDDGo.GetComponent<TMP_Dropdown>();
        threeCompAxisOneDDObj = threeCompAxisOneDDGo.GetComponent<TMP_Dropdown>();
        threeCompAxisTwoDDObj = threeCompAxisTwoDDGo.GetComponent<TMP_Dropdown>();
        threeCompAxisThreeDDObj = threeCompAxisThreeDDGo.GetComponent<TMP_Dropdown>();
        */

        List<TMP_Dropdown.OptionData> axisOptions = GetAxisOptions();

        /*
        xPosDDObj.options = axisOptions;
        yPosDDObj.options = axisOptions;
        zPosDDObj.options = axisOptions;
        axisOneDDObj.options = axisOptions;
        axisTwoDDObj.options = axisOptions;
        axisThreeDDObj.options = axisOptions;
        oneCompAxisOneDDObj.options = axisOptions;
        oneCompAxisTwoDDObj.options = axisOptions;
        oneCompAxisThreeDDObj.options = axisOptions;
        twoCompAxisOneDDObj.options = axisOptions;
        twoCompAxisTwoDDObj.options = axisOptions;
        twoCompAxisThreeDDObj.options = axisOptions;
        threeCompAxisOneDDObj.options = axisOptions;
        threeCompAxisTwoDDObj.options = axisOptions;
        threeCompAxisThreeDDObj.options = axisOptions;
        */
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
        /*
        mV.UpdateGlyphXPosName(xPosDDObj.options[xPosDDObj.value].text);
        mV.UpdateGlyphYPosName(yPosDDObj.options[yPosDDObj.value].text);
        mV.UpdateGlyphZPosName(zPosDDObj.options[zPosDDObj.value].text);

        mV.UpdateGlyphAxisOne(axisOneDDObj.options[axisOneDDObj.value].text);
        mV.UpdateGlyphAxisTwo(axisTwoDDObj.options[axisTwoDDObj.value].text);
        mV.UpdateGlyphAxisThree(axisThreeDDObj.options[axisThreeDDObj.value].text);

        mV.UpdateGlyphOneCompAxisOne(oneCompAxisOneDDObj.options[oneCompAxisOneDDObj.value].text);
        mV.UpdateGlyphOneCompAxisTwo(oneCompAxisTwoDDObj.options[oneCompAxisTwoDDObj.value].text);
        mV.UpdateGlyphOneCompAxisThree(oneCompAxisThreeDDObj.options[oneCompAxisThreeDDObj.value].text);

        mV.UpdateGlyphTwoCompAxisOne(twoCompAxisOneDDObj.options[twoCompAxisOneDDObj.value].text);
        mV.UpdateGlyphTwoCompAxisTwo(twoCompAxisTwoDDObj.options[twoCompAxisTwoDDObj.value].text);
        mV.UpdateGlyphTwoCompAxisThree(twoCompAxisThreeDDObj.options[twoCompAxisThreeDDObj.value].text);

        mV.UpdateGlyphThreeCompAxisOne(threeCompAxisOneDDObj.options[threeCompAxisOneDDObj.value].text);
        mV.UpdateGlyphThreeCompAxisTwo(threeCompAxisTwoDDObj.options[threeCompAxisTwoDDObj.value].text);
        mV.UpdateGlyphThreeCompAxisThree(threeCompAxisThreeDDObj.options[threeCompAxisThreeDDObj.value].text);
        */
        mV.UpdateGlyphXPosName("Latitude");
        mV.UpdateGlyphYPosName("Depth");
        mV.UpdateGlyphZPosName("Longitude");
        mV.UpdateGlyphAxisOne("S1");
        mV.UpdateGlyphAxisTwo("S2");
        mV.UpdateGlyphAxisThree("S3");

        mV.UpdateGlyphOneCompAxisOne("S1-v-x");
        mV.UpdateGlyphOneCompAxisTwo("S1-v-y");
        mV.UpdateGlyphOneCompAxisThree("S1-v-z");

        mV.UpdateGlyphTwoCompAxisOne("S2-v-x");
        mV.UpdateGlyphTwoCompAxisTwo("S2-v-y");
        mV.UpdateGlyphTwoCompAxisThree("S2-v-z");

        mV.UpdateGlyphThreeCompAxisOne("S3-v-x");
        mV.UpdateGlyphThreeCompAxisTwo("S3-v-y");
        mV.UpdateGlyphThreeCompAxisThree("S3-v-z");

        // Maybe move this to some sort of composable creator
        GameObject glyphGo = Instantiate(glyphPrefab, Vector3.zero, Quaternion.identity);

        if (glyphModel.superComp != null)
        {
            glyphGo.transform.parent = glyphModel.parent.transform;
        }

        TenGlyphWrapper glyphWrapper = glyphGo.GetComponent<TenGlyphWrapper>();
        mV.UpdateTransform(glyphWrapper.transform);
        glyphWrapper.Initialize(glyphModel);
        glyphWrapper.Plot();

        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }
}
