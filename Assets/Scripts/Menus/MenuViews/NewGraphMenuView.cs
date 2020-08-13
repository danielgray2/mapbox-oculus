using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewGraphMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject scatterMenuGo;

    [SerializeField]
    GameObject histogramMenuGo;

    [SerializeField]
    GameObject graphTypeDDGo;

    protected TMP_Dropdown graphTypeDDObj;
    protected GameObject next;

    private void Awake()
    {
        Setup(MenuEnum.NEW_GRAPH, menuHandlerGo.GetComponent<MenuView>());
        graphTypeDDObj = graphTypeDDGo.GetComponent<TMP_Dropdown>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrepForTransition()
    {
        string graphName = graphTypeDDObj.options[graphTypeDDObj.value].text;
        GraphTypeEnum enumVal = GraphDict.stringEnumDict[graphName];
        next = DetermineNext(enumVal);

        mV.Route(new RoutingObj(next.GetComponent<IAbsMenuView>().mE, model.gUID));
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        model = iAbsModel;
        controller = new NewGraphMenuContr(this, model);
        graphTypeDDObj.options = GetGraphOptions();
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = GraphDict.stringEnumDict.Keys.ToList();
        return CreateOptions(keys);
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

    public GameObject DetermineNext(GraphTypeEnum enumVal)
    {
        GameObject retVal;
        switch (enumVal)
        {
            case GraphTypeEnum.SCATTERPLOT:
                retVal = scatterMenuGo;
                break;
            case GraphTypeEnum.HISTOGRAM:
                retVal = histogramMenuGo;
                break;
            default:
                throw new ArgumentException("Graph type not recognized");
        }
        return retVal;
    }
}