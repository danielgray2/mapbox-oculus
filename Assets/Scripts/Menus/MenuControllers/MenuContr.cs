using System;
using UnityEngine;

public class MenuContr : IAbsMenuContr
{
    public MenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public void RegisterMenu(MenuEnum key, IAbsMenuView value)
    {
        MenuModel menuModel = CastToMenuModel();
        menuModel.menuDictionary.Add(key, value);
    }

    public void RegisterModel(Guid key, IAbsModel value)
    {
        MenuModel menuModel = CastToMenuModel();
        menuModel.modelDictionary.Add(key, value);
    }

    public override void Update() { }

    public void UpdateCurrModelGUID(Guid newMenuGUID)
    {
        MenuModel menuModel = CastToMenuModel();
        menuModel.currModelGUID = newMenuGUID;
    }

    public void UpdateCurrMenu(MenuEnum newMenuEnum)
    {
        MenuModel menuModel = CastToMenuModel();
        menuModel.currMenuEnum = newMenuEnum;
    }

    public void PrepForRoute(RoutingObj routeObj)
    {
        MenuModel menuModel = CastToMenuModel();
        if (menuModel.currMenuEnum != routeObj.viewEnum)
        {
            menuModel.menuDictionary[menuModel.currMenuEnum].gameObject.SetActive(false);
            menuModel.currMenuEnum = routeObj.viewEnum;
            menuModel.menuDictionary[menuModel.currMenuEnum].gameObject.SetActive(true);
        }
        
        if(menuModel.currModelGUID != routeObj.modelGUID)
        {
            menuModel.currModelGUID = routeObj.modelGUID;
        }
    }

    protected MenuModel CastToMenuModel()
    {
        if (!(model is MenuModel mM))
        {
            throw new ArgumentException("Model must be of type MenuModel");
        }
        return mM;
    }
}
