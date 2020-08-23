using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsGraphModel : IAbsCompModel
{
    public IAbsGraphModel()
    {
        compatSuperComps = new List<Type>() { typeof(IAbsContextModel) };
    }
}
