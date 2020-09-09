﻿using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComposableType
{
    GRAPH,
    CONTEXT,
    DASHBOARD,
    MESH
}
public abstract class IAbsCompModel : IAbsModel
{
    public DataObj dataObj { get; set; }
    public ComposableType compType { get; set; }
    public List<string> availTransfs { get; set; } = new List<string>();
    public List<IAbsTransf> useTransfs { get; set; } = new List<IAbsTransf>();
    public DataPointOptions dataPointOptions { get; set; }
    public List<IAbsCompModel> subComps { get; set; }
    public IAbsCompModel superComp { get; set; }
    public Transform parent { get; set; }
    public Transform transform { get; set; }

    // Not set from base
    public List<Type> compatSubComps { get; set; }

    // Not set from base
    public List<Type> compatSuperComps { get; set; }

    // Not set from base
    public MarkerType mT { get; set; }

    // Beginning to think that we should remove this and
    // simply set the values as necessary without using
    // a function like this. Perhaps more code, but a lot
    // clearer as to what is going on.
    public void SetValsFromBase(IAbsCompModel compModel)
    {
        dataObj = compModel.dataObj;
        compType = compModel.compType;
        availTransfs.AddRange(compModel.availTransfs);
        useTransfs.AddRange(compModel.useTransfs);
        dataPointOptions = dataPointOptions;
        subComps = compModel.subComps;
        superComp = compModel.superComp;
        parent = compModel.parent;
        transform = compModel.transform;
    }
}
