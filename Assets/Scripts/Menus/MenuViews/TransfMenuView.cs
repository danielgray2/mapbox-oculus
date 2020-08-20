using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransfMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject transfsDDGo;

    protected TMP_Dropdown transfsDDObj;
    protected IAbsCompModel compModel;
    protected List<IAbsTransf> availTransfs;
    protected IAbsMenuView next;

    private void Start()
    {
        Setup(MenuEnum.TRANSFS, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        compModel = VizUtils.CastToCompModel(iAbsModel);

        mV = menuHandlerGo.GetComponent<MenuView>();
        transfsDDObj = transfsDDGo.GetComponent<TMP_Dropdown>();

        if(compModel.superComp != null)
        {
            compModel.availTransfs.AddRange(compModel.superComp.availTransfs);
        }

        List<TMP_Dropdown.OptionData> transfOptions = GetTransfOptions();
        transfsDDObj.options = transfOptions;
    }

    public List<TMP_Dropdown.OptionData> GetTransfOptions()
    {
        List<string> optionsList = compModel.availTransfs;
        optionsList.Insert(0, "");
        return CreateOptions(optionsList);
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
        mV.ActivateMenu(next);
    }

    public void AddBtnClicked()
    {
        string selected = transfsDDObj.options[transfsDDObj.value].text;
        mV.AddTransfToProc(selected);

        // Set to blank
        transfsDDObj.value = 0;
    }

    public void ContinueBtnClicked()
    {
        SetNext();
        PrepForTransition();
    }

    public void SetNext()
    {
        if(mV.mM.transfsToProc.Count > 0)
        {
            next = mV.FindFilterMenu(mV.mM.transfsToProc[0]);
        }
        else
        {
            next = mV.FindConfigMenu(compModel);
        }
    }
}