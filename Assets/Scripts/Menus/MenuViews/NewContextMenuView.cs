using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewContextMenuView : IAbstractView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject contextTypeDDGo;

    [SerializeField]
    GameObject mapMenuGo;

    protected MenuHandler mH;
    protected MenuEnum mE;
    protected TMP_Dropdown contextTypeDDObj;
    protected GameObject next;

    private void Awake()
    {
        mH = menuHandlerGo.GetComponent<MenuHandler>();
        mE = MenuEnum.NEW_CONTEXT;
        mH.Register(mE, this.gameObject);
        contextTypeDDObj = contextTypeDDGo.GetComponent<TMP_Dropdown>();
        controller = new NewContextMenuContr(this);
    }

    public override void Initialize(IModel iModel)
    {
        model = iModel;
        contextTypeDDObj.options = GetContextOptions();
    }

    public void PrepForTransition()
    {
        string contextName = contextTypeDDObj.options[contextTypeDDObj.value].text;
        ContextTypeEnum enumVal = ContextDict.stringEnumDict[contextName];
        next = DetermineNext(enumVal);

        if (!(controller is NewContextMenuContr contextContr))
        {
            throw new ArgumentException("Controller must be of type NewContextMenuContr");
        }

        IController nextIController = next.GetComponent<IAbstractView>().controller;
        if (!(nextIController is IAbstractMenu nextMenu))
        {
            throw new ArgumentException("Controller must be of type IAbstractMenu");
        }

        contextContr.Transition(nextMenu);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<TMP_Dropdown.OptionData> GetContextOptions()
    {
        List<string> keys = ContextDict.stringEnumDict.Keys.ToList();
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

    public GameObject DetermineNext(ContextTypeEnum enumVal)
    {
        GameObject retVal;
        switch (enumVal)
        {
            case ContextTypeEnum.MAP:
                retVal = mapMenuGo;
                break;
            default:
                throw new ArgumentException("Graph type not recognized");
        }
        return retVal;
    }
}
