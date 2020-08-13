using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutingObj
{
    public MenuEnum viewEnum { get; set; }
    public Guid modelGUID { get; set; }
    public RoutingObj(MenuEnum viewEnum, Guid modelGUID)
    {
        this.viewEnum = viewEnum;
        this.modelGUID = modelGUID;
    }
}
