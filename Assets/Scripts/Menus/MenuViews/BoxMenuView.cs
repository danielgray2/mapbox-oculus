using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoxMenuView : IAbsMenuView
{
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject configDPMenuGo;

    [SerializeField]
    GameObject dataObjMenuGo;

    [SerializeField]
    GameObject transfsMenuGo;

    [SerializeField]
    GameObject parentMenuGo;

    [SerializeField]
    GameObject childrenMenuGo;

    [SerializeField]
    GameObject genMenuGo;

    protected GameObject next;
    protected string dataObjName;
    protected IAbsCompModel compModel;

    private void Start()
    {
        Setup(MenuEnum.BOX, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel)
    {
        compModel = VizUtils.CastToCompModel(iAbsModel);
    }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public List<TMP_Dropdown.OptionData> GetGraphOptions()
    {
        List<string> keys = DataStore.Instance.dataDict.Keys.ToList();
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

    public void ConfigDPBtnClicked()
    {
        next = configDPMenuGo;
        PrepForTransition();
    }

    public void TransfsBtnClicked()
    {
        next = transfsMenuGo;
        PrepForTransition();
    }

    public void ParentBtnClicked()
    {
        next = parentMenuGo;
        PrepForTransition();
    }

    public void ChildrenBtnClicked()
    {
        IAbsCompModel prevModel = VizUtils.CastToCompModel(mV.GetCurrModel());
        Transform prevTransform = prevModel.transform;
        mV.UpdateCurrModel(CreateChild(prevModel.dataObj));
        mV.UpdateSuperComp(prevModel);
        mV.UpdateParentTransform(prevTransform);
        next = childrenMenuGo;
        mV.SetCreatingSubComp(true);
        PrepForTransition();
    }

    public void ChangeDataBtnClicked()
    {
        next = dataObjMenuGo;
        PrepForTransition();
    }

    public void ContinueBtnClicked()
    {
        if (mV.mM.creatingSubComp)
        {
            mV.SetCreatingSubComp(false);
            mV.UpdateCurrModel(compModel.superComp);
        }
        else
        {
            next = genMenuGo;
            PrepForTransition();
        }
    }

    public IAbsCompModel CreateChild(DataObj dO)
    {
        IAbsCompModel baseModel = new BaseCompModel
        {
            dataObj = dO
        };
        mV.RegisterModel(baseModel.gUID, baseModel);
        return baseModel;
    }
}
