using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsMeshModel : IAbsCompModel
{
    public IAbsMeshModel()
    {
        compatSuperComps = new List<Type>() { typeof(IAbsContextModel) };
    }
}
