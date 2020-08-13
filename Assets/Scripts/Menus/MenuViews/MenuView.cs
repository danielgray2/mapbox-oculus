﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuView : IAbstractView
{
    protected MenuEnum startMenu = MenuEnum.GENERAL_MENU;
    protected MenuContr menuContr;

    private void Awake()
    {
        model = new MenuModel();
        menuContr = new MenuContr(this, model);
        controller = menuContr;
        menuContr.UpdateCurrMenu(startMenu);
    }
    
    private void Start()
    {
        MenuModel mM = CastToMenuModel();
        List<MenuEnum> menuList = mM.menuDictionary.Keys.ToList();
        foreach (MenuEnum curr in menuList)
        {
            if(curr != startMenu)
            {
                mM.menuDictionary[curr].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateMenu(GameObject gO)
    {
        MenuModel mM = CastToMenuModel();
        List<MenuEnum> menuList = mM.menuDictionary.Keys.ToList();
        foreach(MenuEnum curr in menuList)
        {
            if(mM.menuDictionary[curr] == gO)
            {
                mM.menuDictionary[curr].gameObject.SetActive(true);
            }
            else
            {
                mM.menuDictionary[curr].gameObject.SetActive(false);
            }
        }
    }

    public void RegisterMenu(MenuEnum key, IAbsMenuView value)
    {
        menuContr.RegisterMenu(key, value);
    }

    public void RegisterModel(Guid key, IAbsModel value)
    {
        menuContr.RegisterModel(key, value);
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void UpdateCurrModel(IAbsModel newModel)
    {
        menuContr.UpdateCurrModelGUID(newModel.gUID);
    }

    public void Route(RoutingObj routeObj)
    {
        menuContr.PrepForRoute(routeObj);
        MenuModel menuModel = CastToMenuModel();
        IAbsMenuView currMenu = menuModel.menuDictionary[menuModel.currMenuEnum];
        Guid currModelKey = menuModel.currModelGUID;

        if (menuModel.modelDictionary.Keys.Contains(currModelKey))
        {
            currMenu.Initialize(menuModel.modelDictionary[currModelKey]);
        }
        else
        {
            currMenu.Initialize(null);
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
