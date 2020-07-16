using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Data.Analysis;
using System.Linq;
using Unity.XR.Oculus;
using CsvHelper;

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
        float minVal = float.PositiveInfinity;
        if(!this.statsDict[colName].TryGetValue(STATSVALS.MIN, out minVal))
        {
            try
            {
                minVal = (float)this.df.Columns[colName].Min();
            }
            catch
            {
                throw new System.ApplicationException("Attempted to convert an illegal type to a float");
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
        float maxVal = float.PositiveInfinity;
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

        float avgVal = float.PositiveInfinity;
        if (!this.statsDict.ContainsKey(colName) || !this.statsDict[colName].TryGetValue(STATSVALS.AVG, out avgVal))
        {
            this.GetAvg(colName);
            avgVal = this.statsDict[colName][STATSVALS.AVG];
        }

        float lowerQrtVal = float.PositiveInfinity;
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

        float avgVal = float.PositiveInfinity;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.AVG, out avgVal))
        {
            this.GetAvg(colName);
            avgVal = this.statsDict[colName][STATSVALS.AVG];
        }

        float upperQrtVal = float.PositiveInfinity;
        if (!this.statsDict[colName].TryGetValue(STATSVALS.LOWERQRT, out upperQrtVal))
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
}
