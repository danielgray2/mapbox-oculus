using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractContext : IContext
{
    public DataObj dataObj { get; set; }
    public List<IAbsTransf> transformations { get; set; }
    public DataPointOptions dataPointOptions { get; set; }
    public List<IComposable> subComps { get; set; }
    public IComposable superComp { get; set; }
    public IAbsModel model { get; set; }
    public IAbstractView view { get; set; }
    public abstract void Generate();
    public abstract void Initialize(IAbsModel iAbsModel);
    public abstract void Update();
}
