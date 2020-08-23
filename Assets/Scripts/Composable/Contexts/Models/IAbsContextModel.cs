using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsContextModel : IAbsCompModel
{
    public IAbsContextModel()
    {
        compatSubComps = new List<Type>() { typeof(IAbsGraphModel), typeof(IAbsMeshModel) };
    }
}
