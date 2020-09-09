using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuView : IAbstractView
{
    public MenuModel mM;
    protected MenuEnum startMenu = MenuEnum.GENERAL_MENU;
    protected MenuContr menuContr;

    // Delegate Controllers
    protected HistMenuContr histMenuContr = new HistMenuContr();
    protected MapMenuContr mapMenuContr = new MapMenuContr();
    protected MeshMenuContr meshMenuContr = new MeshMenuContr();
    protected ScatterMenuContr scatterMenuContr = new ScatterMenuContr();
    protected BaseCompContr baseCompContr = new BaseCompContr();

    private void Awake()
    {
        mM = new MenuModel();
        menuContr = new MenuContr(mM);
        controller = menuContr;
        menuContr.UpdateCurrMenu(startMenu);
    }

    public void ActivateMenu(IAbsMenuView view)
    {
        List<MenuEnum> menuList = mM.menuDictionary.Keys.ToList();
        foreach(MenuEnum curr in menuList)
        {
            if(mM.menuDictionary[curr] == view)
            {
                mM.menuDictionary[curr].gameObject.SetActive(true);
                if (mM.modelDictionary.ContainsKey(mM.currModelGUID))
                {
                    mM.menuDictionary[curr].Initialize(mM.modelDictionary[mM.currModelGUID]);
                }
                else
                {
                    mM.menuDictionary[curr].Initialize(null);
                }
            }
            else
            {
                mM.menuDictionary[curr].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateModelDataObj(DataObj dataObj)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(mM.modelDictionary[mM.currModelGUID]);
        baseCompContr.UpdateDataObj(compModel, dataObj);
    }

    public void RegisterMenu(MenuEnum key, IAbsMenuView value, bool startInit = false)
    {
        menuContr.RegisterMenu(key, value);
        if (startInit || key == startMenu)
        {
            ActivateMenu(value);
        }
        else
        {
            value.gameObject.SetActive(false);
        }
    }

    public void RegisterModel(Guid key, IAbsModel value)
    {
        menuContr.RegisterModel(key, value);
    }

    public void UpdateCurrMenu(MenuEnum newMenuEnum)
    {
        mM.currMenuEnum = newMenuEnum;
    }

    public void UpdateCurrModel(IAbsModel newModel)
    {
        menuContr.UpdateCurrModelGUID(newModel.gUID);
    }

    public void UpdateHistColName(string newName)
    {
        IAbsModel currModel = mM.modelDictionary[mM.currModelGUID];
        histMenuContr.UpdateColName(currModel, newName);
    }

    public void UpdateMapLatName(string newName)
    {
        mapMenuContr.UpdateLatName(GetCurrModel(), newName);
    }

    public void UpdateMapLonName(string newName)
    {
        mapMenuContr.UpdateLonName(GetCurrModel(), newName);
    }

    public void SetCreatingSubComp(bool val)
    {
        mM.creatingSubComp = val;
    }

    public void UpdateMapExaggeration(string newVal)
    {
        mapMenuContr.UpdateExaggeration(GetCurrModel(), newVal);
    }

    public void UpdateMeshXColName(string newName)
    {
        meshMenuContr.UpdateXColName(GetCurrModel(), newName);
    }

    public void UpdateMeshYColName(string newName)
    {
        meshMenuContr.UpdateYColName(GetCurrModel(), newName);
    }

    public void UpdateMeshZColName(string newName)
    {
        meshMenuContr.UpdateZColName(GetCurrModel(), newName);
    }

    public void UpdateMeshValColName(string newName)
    {
        meshMenuContr.UpdateValColName(GetCurrModel(), newName);
    }

    public void UpdateScatterXName(string newName)
    {
        scatterMenuContr.UpdateXName(GetCurrModel(), newName);
    }

    public void UpdateScatterYName(string newName)
    {
        scatterMenuContr.UpdateYName(GetCurrModel(), newName);
    }

    public void UpdateScatterZName(string newName)
    {
        scatterMenuContr.UpdateZName(GetCurrModel(), newName);
    }

    public void UpdateScatterExtraMargin(float newVal)
    {
        scatterMenuContr.UpdateExtraMargin(GetCurrModel(), newVal);
    }
    
    public void UpdateScatterPlotScale(float newVal)
    {
        scatterMenuContr.UpdatePlotScale(GetCurrModel(), newVal);
    }

    public void UpdateScatterDataPointScale(Vector3 newVal)
    {
        scatterMenuContr.UpdateDataPointScale(GetCurrModel(), newVal);
    }

    public void UpdateScatterNumMarkersPerAxis(int newVal)
    {
        scatterMenuContr.UpdateNumMarkersPerAxis(GetCurrModel(), newVal);
    }

    public void UpdateScatterParent(GameObject newParent)
    {
        scatterMenuContr.UpdateParent(GetCurrModel(), newParent);
    }

    public void UpdateTransform(Transform transform)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.UpdateTransform(compModel, transform);
    }

    public void UpdateParentTransform(Transform transform)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.UpdateParent(compModel, transform);
    }

    public void AddTransfToUse(IAbsTransf transf)
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.AddTransfToUse(compModel, transf);
    }

    public IAbsModel GetCurrModel()
    {
        return mM.modelDictionary[mM.currModelGUID];
    }

    public void AddTransfToProc(string name)
    {
        mM.transfsToProc.Add(name);
    }

    public void RemoveTransfToProc(string name)
    {
        mM.transfsToProc.Remove(name);
    }

    public void UpdateTransfsToUse(List<IAbsTransf> transfsToUse)
    {
        IAbsCompModel model = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.UpdateUseTransfs(model, transfsToUse);
    }

    // TODO: Consider adding a parameter here instead of setting it
    // automatically from the curr
    public void UpdateSuperComp(IAbsCompModel superComp)
    {
        IAbsCompModel curr = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.UpdateSuperComp(curr, superComp);
    }

    public void UpdateMarkerType(MarkerType mT)
    {
        IAbsCompModel curr = VizUtils.CastToCompModel(GetCurrModel());
        baseCompContr.UpdateMarkerType(curr, mT);
    }

    public IAbsMenuView FindConfigMenu(IAbsCompModel compModel)
    {
        switch (compModel)
        {
            case ScatterModel scatterModel:
                return mM.menuDictionary[MenuEnum.SCATTERPLOT_GRAPH];
            case HistModel histModel:
                return mM.menuDictionary[MenuEnum.HISTOGRAM_GRAPH];
            case MapModel mapModel:
                return mM.menuDictionary[MenuEnum.MAP];
            case MeshModel meshModel:
                return mM.menuDictionary[MenuEnum.MESH];
            default:
                throw new ArgumentException("Could not find a config menu for : " + compModel.ToString());
        }
    }

    public IAbsMenuView FindFilterMenu(string filterName)
    {
        if(filterName == DepthLatLonTransf.friendlyName)
        {
            return mM.menuDictionary[MenuEnum.DEP_LAT_LON];
        }
        else if(filterName == LatLonTransf.friendlyName)
        {
            return mM.menuDictionary[MenuEnum.LAT_LON];
        }
        else
        {
            throw new ArgumentException("Filter menu not found");
        }
    }
}
