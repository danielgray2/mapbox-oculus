using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAbsCompContr : IAbsMenuContr
{
    public void UpdateDataObj(IAbsCompModel model, DataObj dO)
    {
        model.dataObj = dO;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateCompType(IAbsCompModel model, ComposableType compType) 
    {
        model.compType = compType;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateAvailTransfs(IAbsCompModel model, List<string> availTransfs)
    {
        model.availTransfs = availTransfs;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateUseTransfs(IAbsCompModel model, List<IAbsTransf> useTransfs) 
    {
        model.useTransfs = useTransfs;
        model.modelUpdateEvent.Invoke();
    }

    public void AddTransfToUse(IAbsCompModel model, IAbsTransf useTransf)
    {
        model.useTransfs.Add(useTransf);
    }

    public void UpdateDataPointOptions(IAbsCompModel model, DataPointOptions dataPointOptions) 
    {
        model.dataPointOptions = dataPointOptions;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateSubComps(IAbsCompModel model, List<IAbsCompModel> subComps) 
    {
        model.subComps = subComps;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateSuperComp(IAbsCompModel model, IAbsCompModel superComp) 
    {
        model.superComp = superComp;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateParent(IAbsCompModel model, Transform parent) 
    {
        model.parent = parent;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateTransform(IAbsCompModel model, Transform transform) 
    {
        model.transform = transform;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateCompatSubComps(IAbsCompModel model, List<Type> compatSubComps) 
    {
        model.compatSubComps = compatSubComps;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateCompatSuperComps(IAbsCompModel model, List<Type> compatSuperComps) 
    {
        model.compatSuperComps = compatSuperComps;
        model.modelUpdateEvent.Invoke();
    }

    public void UpdateMarkerType(IAbsCompModel model, MarkerType mT)
    {
        model.mT = mT;
        model.modelUpdateEvent.Invoke();
    }
}
