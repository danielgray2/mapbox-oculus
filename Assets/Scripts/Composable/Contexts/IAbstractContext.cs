using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractContext : IContext
{
    public DataObj dataObj { get; set; }
    public List<IAbstractTransformation> transformations { get; set; }
    public DataPointOptions dataPointOptions { get; set; }
    public List<IComposable> subComps { get; set; }
    public IComposable superComp { get; set; }
    public IModel model { get; set; }
    public IAbstractView view { get; set; }
    public abstract void Generate();
    public abstract void Initialize(IModel iModel);
    public abstract void Update();
}
