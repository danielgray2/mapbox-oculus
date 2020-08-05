using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComposable
{
    DataObj GetDataObj();
    void SetDataObj();
    List<Transformation> GetTransformations();
    void SetTransformations(List<Transformation> transformations);
    DataPointOptions GetDataPointOptions();
    void SetDataPointOptions(DataPointOptions dPO);
    IComposableOptions GetIComposableOptions();
    void SetIComposableOptions(IComposableOptions iCO);
    List<IComposable> GetSubComps();
    void SetSubComps(List<IComposable> subComps);
    void GetSuperComp();
    void SetSuperComp(IComposable superComp);
}
