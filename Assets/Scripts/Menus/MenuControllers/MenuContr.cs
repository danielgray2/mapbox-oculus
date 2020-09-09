using System;
using UnityEngine;

public class MenuContr : IAbsMenuContr
{
    MenuModel menuModel;

    public MenuContr(MenuModel menuModel)
    {
        this.menuModel = menuModel;
    }

    public void RegisterMenu(MenuEnum key, IAbsMenuView value)
    {
        menuModel.menuDictionary.Add(key, value);
    }

    public void RegisterModel(Guid key, IAbsModel value)
    {
        menuModel.modelDictionary.Add(key, value);
    }

    public void UpdateCurrModelGUID(Guid newModelGUID)
    {
        menuModel.currModelGUID = newModelGUID;
    }

    public void UpdateCurrMenu(MenuEnum newMenuEnum)
    {
        menuModel.currMenuEnum = newMenuEnum;
    }

    public void PrepForRoute(RoutingObj routeObj)
    {
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
