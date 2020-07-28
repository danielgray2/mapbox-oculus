using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;

public static class FilterParser
{
    public static DataObj ParseAnd(DataObj dO, List<FilterObj> filterObjs)
    {
        DataFrame currFiltered = dO.df.Clone();
        for (int i = 0; i < filterObjs.Count; i++)
        {
            FilterObj currFilter = filterObjs[i];
            switch (currFilter.filterType)
            {
                case FilterObj.FilterType.E:
                    currFiltered = HandleEquals(currFiltered, currFilter);
                    break;
                case FilterObj.FilterType.GTE:
                    currFiltered = HandleGreaterThanEquals(currFiltered, currFilter);
                    break;
                case FilterObj.FilterType.LTE:
                    currFiltered = HandleLessThanEquals(currFiltered, currFilter);
                    break;
                default:
                    break;
            }
        }
        return new DataObj(currFiltered);
    }

    private static DataFrame HandleLessThanEquals(DataFrame df, FilterObj currFilter)
    {
        DataObj dO = new DataObj(df);
        IComparable lowestValue = dO.GetMin(currFilter.colName);
        return dO.SliceByAttribute(currFilter.colName, lowestValue, currFilter.value).df;
    }

    private static DataFrame HandleGreaterThanEquals(DataFrame df, FilterObj currFilter)
    {
        DataObj dO = new DataObj(df);
        IComparable highestValue = dO.GetMax(currFilter.colName);
        return dO.SliceByAttribute(currFilter.colName, currFilter.value, highestValue).df;
    }

    private static DataFrame HandleEquals(DataFrame df, FilterObj currFilter)
    {
        DataObj dO = new DataObj(df);
        return dO.SliceByAttribute(currFilter.colName, currFilter.value, currFilter.value).df;
    }
}
