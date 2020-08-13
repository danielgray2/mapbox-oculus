using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuModel : IAbsModel
{
    public Dictionary<MenuEnum, IAbsMenuView> menuDictionary = new Dictionary<MenuEnum, IAbsMenuView>();
    public Dictionary<Guid, IAbsModel> modelDictionary = new Dictionary<Guid, IAbsModel>();
    public Guid currModelGUID { get; set; }
    public MenuEnum currMenuEnum { get; set; }
}
