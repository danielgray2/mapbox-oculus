using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEngine.Rendering.Universal.Internal;

public class GraphMenu : MonoBehaviour
{
    [SerializeField]
    GameObject graphTypeDD;

    [SerializeField]
    GameObject xAxisDD;

    [SerializeField]
    GameObject yAxisDD;

    [SerializeField]
    GameObject histogramPrefab;

    [SerializeField]
    GameObject zAxisDD;

    [SerializeField]
    GameObject scatterplotPrefab;

    TMP_Dropdown graphTypeDDObj;
    TMP_Dropdown xAxisDDObj;
    TMP_Dropdown yAxisDDObj;
    TMP_Dropdown zAxisDDObj;

    private static string SCATTERPLOT = "Scatterplot";
    private static string HISTOGRAM = "Histogram";

    private void Start()
    {
        graphTypeDDObj = graphTypeDD.GetComponent<TMP_Dropdown>();
        xAxisDDObj = xAxisDD.GetComponent<TMP_Dropdown>();
        yAxisDDObj = yAxisDD.GetComponent<TMP_Dropdown>();
        zAxisDDObj = zAxisDD.GetComponent<TMP_Dropdown>();

        SetGraphOptions();
        SetXAxisOptions();
        SetYAxisOptions();
        SetZAxisOptions();
    }

    public void HandleGeneration()
    {
        string graphType = graphTypeDDObj.options.ElementAt(graphTypeDDObj.value).text;
        string xAxis = xAxisDDObj.options.ElementAt(xAxisDDObj.value).text;
        string yAxis = yAxisDDObj.options.ElementAt(yAxisDDObj.value).text;
        string zAxis = zAxisDDObj.options.ElementAt(zAxisDDObj.value).text;

        if(graphType == SCATTERPLOT)
        {
            // TODO: Change this to follow a
            // design pattern and classes instead
            // of methods so that we can grow
            GenerateScatterplot(xAxis, yAxis, zAxis);
        }
        else if(graphType == HISTOGRAM)
        {
            GenerateHistogram(xAxis);
        }
    }

    public void GenerateScatterplot(string xName, string yName, string zName)
    {
        List<SData> sDataList = new List<SData>();

        foreach(GameObject pointGo in MapStore.Instance.selectedGOs)
        {
            SData sData = pointGo.GetComponent<LerpAnimation>().sData;
            sDataList.Add(sData);
        }

        GameObject scatterplotGo = Instantiate(scatterplotPrefab, Vector3.zero, Quaternion.identity);
        ScatterBox scatterplotObj = scatterplotGo.GetComponentInChildren<ScatterBox>();
        scatterplotObj.InitializeScatterplot(sDataList, xName, yName, zName);
    }

    public void GenerateHistogram(string xName)
    {
        List<SData> sDataList = new List<SData>();

        foreach (GameObject pointGo in MapStore.Instance.selectedGOs)
        {
            SData sData = pointGo.GetComponent<LerpAnimation>().sData;
            sDataList.Add(sData);
        }

        GameObject histogramGo = Instantiate(histogramPrefab, Vector3.zero, Quaternion.identity);
        Histogram scatterplotObj = histogramGo.GetComponentInChildren<Histogram>();
        scatterplotObj.InitializeHistogram(sDataList, xName);
    }

    // Take a look at the GraphTypeEnum
    // class to get all of the currently
    // implemented graphs. Probably should
    // figure out a better way to do this
    private void SetGraphOptions()
    {
        List<string> graphTypes = new List<string> { SCATTERPLOT, HISTOGRAM };
        List<TMP_Dropdown.OptionData> optionsList = CreateOptions(graphTypes);
        graphTypeDDObj.options = optionsList;
    }

    private void SetXAxisOptions() 
    {
        List<TMP_Dropdown.OptionData> options = GetOptionsSData();
        xAxisDDObj.options = options;
    }

    private void SetYAxisOptions() 
    {
        List<TMP_Dropdown.OptionData> options = GetOptionsSData();
        yAxisDDObj.options = options;
    }

    private void SetZAxisOptions()
    {
        List<TMP_Dropdown.OptionData> options = GetOptionsSData();
        zAxisDDObj.options = options;
    }

    private List<TMP_Dropdown.OptionData> GetOptionsSData() 
    {
        List<PropertyInfo> propInf = typeof(SData).GetProperties().ToList();
        List<string> nameList = new List<string>();

        foreach(PropertyInfo inf in propInf)
        {
            nameList.Add(inf.Name);
        }

        return CreateOptions(nameList);
    }

    protected List<TMP_Dropdown.OptionData> CreateOptions(List<string> stringList)
    {
        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();
        foreach (string item in stringList)
        {
            optionData.Add(new TMP_Dropdown.OptionData(item));
        }

        return optionData;
    }
}
