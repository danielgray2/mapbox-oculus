using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewGraphMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject scatterMenuGo;

    [SerializeField]
    GameObject histogramMenuGo;

    [SerializeField]
    GameObject graphTypeDDGo;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected TMP_Dropdown graphTypeDDObj;
    protected GameObject next;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.NEW_GRAPH;
        mH.Register(mE, this.gameObject);
        graphTypeDDObj = graphTypeDDGo.GetComponent<TMP_Dropdown>();
        controller = new NewGraphMenuContr(this);
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
        
        if(!(controller is NewGraphMenuContr graphContr))
        {
            throw new ArgumentException("Controller must be of type NewGraphMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        graphContr.Transition(nextMenu);
    }

    public override void Initialize(IModel iModel)
    {
        model = iModel;
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