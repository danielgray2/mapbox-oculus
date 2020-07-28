using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterObj
{
    public enum FilterType
    {
        GTE,
        E,
        LTE
    }

    public FilterType filterType { get; set; }
    public IComparable value { get; set; }
    public string colName { get; set; }

    public FilterObj(FilterType filterType, IComparable value, string colName)
    {
        this.filterType = filterType;
        this.value = value;
        this.colName = colName;
    }
}
