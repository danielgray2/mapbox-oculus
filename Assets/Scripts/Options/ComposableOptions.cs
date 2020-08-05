using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposableOptions : IOptions
{
    public DataObj dataObj { get; set; }
    public List<IAbstractTransformation> transformations { get; set; }
    public DataPointOptions dataPointOptions { get; set; }
    public List<IComposable> subComps { get; set; }
    public IComposable superComp { get; set; }
}
