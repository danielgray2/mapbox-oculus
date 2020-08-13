using System.Collections.Generic;

public interface IComposable : IController
{
    DataObj dataObj { get; set; }
    List<IAbsTransf> transformations { get; set; }
    DataPointOptions dataPointOptions { get; set; }
    List<IComposable> subComps { get; set; }
    IComposable superComp { get; set; }
}
