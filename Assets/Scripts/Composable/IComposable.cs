using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComposable : IController
{
    DataObj dataObj { get; set; }
    List<IAbstractTransformation> transformations { get; set; }
    DataPointOptions dataPointOptions { get; set; }
    List<IComposable> subComps { get; set; }
    IComposable superComp { get; set; }
}
