using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractGraph : IGraph
{
    public IModel model { get; set; }
    public Vector3 minDpSize { get; set; }
    public Vector3 maxDpSize { get; set; }
    public DataObj dataObj { get; set; }
    public List<IAbstractTransformation> transformations { get ; set; }
    public DataPointOptions dataPointOptions { get; set; }
    public List<IComposable> subComps { get; set; }
    public IComposable superComp { get; set; }
    public IAbstractView view { get; set; }

    public void Initialize(IModel iModel)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
