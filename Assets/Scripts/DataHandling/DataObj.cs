using System.Collections.Generic;
using Microsoft.Data.Analysis;
using System.Linq;
using System;
using UnityEngine;

public class DataObj
{
    public enum STATSVALS
    { 
        MIN,
        MAX,
        AVG,
        MEDIAN,
        LOWERQRT,
        UPPERQRT
    }
    public DataFrame df { get; protected set; }
    protected Dictionary<string, Dictionary<STATSVALS, float>> statsDict { get; set; }
    public DataObj(DataFrame df)
    {
        this.df = df;
        statsDict = new Dictionary<string, Dictionary<STATSVALS, float>>();
    }

    public float GetMin(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, float>());
        }
        float minVal;
        if(!this.statsDict[colName].TryGetValue(STATSVALS.MIN, out minVal))
        {
            for (int i = 0; i < df.Columns[colName].Length; i++)
            {
                try
                {
                    minVal = (float)this.df.Columns[colName].Min();
                }
                catch
                {
                    throw new InvalidCastException("Attempted to find the min of an illegal variable type");
                }
            }
            
            this.statsDict[colName].Add(STATSVALS.MIN, minVal);
        }
        return this.statsDict[colName][STATSVALS.MIN];
    }

    public float GetMax(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, float>());
        }
        float maxVal;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.MAX, out maxVal))
        {
            try
            {
                maxVal = (float)this.df.Columns[colName].Max();
            }
            catch
            {
                throw new System.ApplicationException("Attempted to convert an illegal type to a float");
            }
            this.statsDict[colName].Add(STATSVALS.MAX, maxVal);
        }
        return this.statsDict[colName][STATSVALS.MAX];
    }

    public float GetAvg(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, float>());
        }
        if (!this.statsDict[colName].ContainsKey(STATSVALS.AVG))
        {
            float avgVal;
            try
            {
                avgVal = (float)this.df.Columns[colName].Mean();
            }
            catch
            {
                throw new System.ApplicationException("Attempted to convert an illegal type to a float");
            }
            this.statsDict[colName].Add(STATSVALS.AVG, avgVal);
        }
        return this.statsDict[colName][STATSVALS.AVG];
    }

    public float GetLowerQrt(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, float>());
        }

        float avgVal;
        if (!this.statsDict.ContainsKey(colName) || !this.statsDict[colName].TryGetValue(STATSVALS.AVG, out avgVal))
        {
            this.GetAvg(colName);
            avgVal = this.statsDict[colName][STATSVALS.AVG];
        }

        float lowerQrtVal;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.LOWERQRT, out lowerQrtVal))
        {
            try
            {
                PrimitiveDataFrameColumn<float> col = (PrimitiveDataFrameColumn<float>)this.df.Columns[colName];
                List<float?> valsAsList = col.Where(val => val < avgVal).ToList();
                lowerQrtVal = this.CalculateMedian(valsAsList);
            }
            catch
            {
                throw new System.ApplicationException("Attempted to calculate stats on an illegal type");
            }
            this.statsDict[colName].Add(STATSVALS.LOWERQRT, lowerQrtVal);
        }
        return this.statsDict[colName][STATSVALS.LOWERQRT];
    }

    public float GetUpperQrt(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, float>());
        }

        float avgVal;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.AVG, out avgVal))
        {
            this.GetAvg(colName);
            avgVal = this.statsDict[colName][STATSVALS.AVG];
        }

        float upperQrtVal;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.UPPERQRT, out upperQrtVal))
        {
            try
            {
                PrimitiveDataFrameColumn<float> col = (PrimitiveDataFrameColumn<float>)this.df.Columns[colName];
                List<float?> valsAsList = col.Where(val => val >= avgVal).ToList();
                upperQrtVal = this.CalculateMedian(valsAsList);
            }
            catch
            {
                throw new System.ApplicationException("Attempted to calculate stats on an illegal type");
            }
            this.statsDict[colName].Add(STATSVALS.UPPERQRT, upperQrtVal);
        }
        return this.statsDict[colName][STATSVALS.UPPERQRT];
    }

    public float CalculateMedian(List<float?> values)
    {
        List<float> sortedList = values.Where(x => x.HasValue).Select(x => x.Value).ToList();
        sortedList.Sort();

        if(sortedList.Count == 0)
        {
            throw new System.ArgumentException("Cannot calculate median when no values are given");
        }

        if(sortedList.Count == 1)
        {
            return sortedList[0];
        }

        int length = sortedList.Count;
        int midPoint = (length / 2) - 1;
        return length % 2 == 0 ? ((sortedList.ElementAt(midPoint) + sortedList.ElementAt(midPoint + 1)) / 2) : sortedList.ElementAt(midPoint);
    }
    public DataObj SliceByAttribute<T>(string attrName, T startVal, T endVal) where T : unmanaged, IComparable
    {
        CheckValidAttr(attrName);
        List<bool> lowerBools = new List<bool>();
        List<bool> upperBools = new List<bool>();
        for (int i = 0; i < df.Columns[attrName].Length; i++)
        {
            T currVal;

            try
            {
                currVal = (T)Convert.ChangeType(df.Columns[attrName][i], typeof(T));
            }
            catch
            {
                throw new InvalidCastException("Attempted an illegal cast");
            }

            if (currVal.CompareTo(startVal) >= 0)
            {
                lowerBools.Add(true);
            }
            else
            {
                lowerBools.Add(false);
            }

            if (currVal.CompareTo(endVal) <= 0)
            {
                upperBools.Add(true);
            }
            else
            {
                upperBools.Add(false);
            }
        }

        PrimitiveDataFrameColumn<bool> lowerVals = new PrimitiveDataFrameColumn<bool>("lowerVals", lowerBools);
        PrimitiveDataFrameColumn<bool> upperVals = new PrimitiveDataFrameColumn<bool>("upperVals", upperBools);
        PrimitiveDataFrameColumn<bool> filterVals = new PrimitiveDataFrameColumn<bool>("filterVals", upperVals.Count());

        for (int i = 0; i < lowerVals.Count(); i++)
        {
            if ((lowerVals[i].HasValue && lowerVals[i].Value) && (upperVals[i].HasValue && upperVals[i].Value))
            {
                filterVals[i] = true;
            }
            else
            {
                filterVals[i] = false;
            }
        }

        DataFrame filteredDf = df.Filter(filterVals);
        return new DataObj(filteredDf);
    }
    public DataObj SliceByIndex(int startIndex, int endIndex)
    {
        if (startIndex < 0 || endIndex >= df.Columns[0].Length)
        {
            throw new IndexOutOfRangeException("Attempted to slice by an invalid index");
        }

        if (startIndex > endIndex)
        {
            throw new ArgumentException("Start index must be smaller than end index.");
        }

        List<bool> filterVals = new List<bool>();
        
        for (int i = 0; i < df.Rows.Count(); i++)
        {
            if(i >= startIndex && i <= endIndex)
            {
                filterVals.Add(true);
            }
            else 
            {
                filterVals.Add(false);
            }
        }
        PrimitiveDataFrameColumn<bool> filterCol = new PrimitiveDataFrameColumn<bool>("filterCol", filterVals);
        DataFrame filteredDf = df.Filter(filterCol);
        return new DataObj(filteredDf);
    }

    bool CheckValidAttr(string attrName)
    {
        if (this.df.Columns[attrName] == null)
        {
            throw new ArgumentException("Attempted to slice using an attribute that does not exist");
        }
        return true;
    }
}
