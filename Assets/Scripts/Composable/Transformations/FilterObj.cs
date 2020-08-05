using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterObj : IAbstractTransformation
{
    public enum FilterType
    {
        GTE,
        E,
        LTE
    }

    protected IAbsTransformationAnimator iTA;
    public FilterType filterType { get; set; }
    public IComparable value { get; set; }
    public string colName { get; set; }

    public FilterObj(FilterType filterType, IComparable value, string colName)
    {
        this.filterType = filterType;
        this.value = value;
        this.colName = colName;
    }

    public override DataObj ApplyTransformation(DataObj dO)
    {
        //TODO: Figure this out using the filter parser
        return new DataObj(new DataFrame());
    }

    public override DataObj Update(DataObj dO)
    {
        throw new NotImplementedException();
    }
}
