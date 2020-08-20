using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractContext : IContext
{
    public IAbsModel model { get; set; }
    public abstract void Generate();
}
