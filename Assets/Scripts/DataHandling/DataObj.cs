using System.Collections.Generic;
using Microsoft.Data.Analysis;
using System.Linq;
using System;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using System.Text.RegularExpressions;
using System.Globalization;
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
        UPPERQRT,
        IQR
    }
    protected string[] dateFormats = { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd" };
    public DataFrame df { get; protected set; }
    protected Dictionary<string, Dictionary<STATSVALS, dynamic>> statsDict { get; set; }
    public DataObj(DataFrame df)
    {
        UpdateDf(df);
    }

    public void UpdateDf(DataFrame df)
    {
        this.df = df;
        statsDict = new Dictionary<string, Dictionary<STATSVALS, dynamic>>();
    }

    public dynamic GetMin(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
        }

        dynamic minVal;
        try
        {
            minVal = df.Columns[colName][0];
        }
        catch(IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException("Unable to assign first value in column to variable. Column may be empty.");
        }

        if (!this.statsDict[colName].ContainsKey(STATSVALS.MIN))
        {
            for (int i = 0; i < df.Columns[colName].Length; i++)
            {
                try
                {
                    if (minVal.CompareTo(df.Columns[colName][i]) > 0)
                    {
                        minVal = df.Columns[colName][i];
                    }
                }
                catch
                {
                    throw new ArgumentException("Error thrown when comparing elements");
                }
            }
            this.statsDict[colName].Add(STATSVALS.MIN, minVal);
        }
        return this.statsDict[colName][STATSVALS.MIN];
    }

    public dynamic GetMax(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
        }

        dynamic maxVal;
        try
        {
            maxVal = df.Columns[colName][0];
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException("Unable to assign first value in column to variable. Column may be empty.");
        }

        if (!this.statsDict[colName].ContainsKey(STATSVALS.MAX))
        {
            for (int i = 0; i < df.Columns[colName].Length; i++)
            {
                try
                {
                    if (maxVal.CompareTo(df.Columns[colName][i]) < 0)
                    {
                        maxVal = df.Columns[colName][i];
                    }
                }
                catch
                {
                    throw new ArgumentException("Error thrown when comparing elements");
                }
            }
            this.statsDict[colName].Add(STATSVALS.MAX, maxVal);
        }
        return this.statsDict[colName][STATSVALS.MAX];
    }

    public float GetMedian(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
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
                throw new ArgumentException("Attempted to calculate mean on an invalid datatype");
            }
            this.statsDict[colName].Add(STATSVALS.AVG, avgVal);
        }
        return this.statsDict[colName][STATSVALS.AVG];
    }

    public float GetLowerQrt(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
        }

        float avgVal;
        if (!this.statsDict.ContainsKey(colName) || !this.statsDict[colName].ContainsKey(STATSVALS.AVG))
        {
            this.GetMedian(colName);
        }
        avgVal = this.statsDict[colName][STATSVALS.AVG];

        if (!this.statsDict[colName].ContainsKey(STATSVALS.LOWERQRT))
        {
            float lowerQrtVal;
            try
            {
                PrimitiveDataFrameColumn<float> col = new PrimitiveDataFrameColumn<float>("floatCol", this.df.Columns[colName].Length);
                for (int i = 0; i < this.df.Columns[colName].Length; i++)
                {
                    object currVal = this.df.Columns[colName][i];
                    string currString = currVal.ToString();
                    float currFloat = float.Parse(currString, CultureInfo.InvariantCulture.NumberFormat);
                    col[i] = currFloat;
                }

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
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
        }

        float avgVal;
        if (!this.statsDict[colName].ContainsKey(STATSVALS.AVG))
        {
            this.GetMedian(colName);
        }
        avgVal = this.statsDict[colName][STATSVALS.AVG];

        if (!this.statsDict[colName].ContainsKey(STATSVALS.UPPERQRT))
        {
            float upperQrtVal;
            try
            {
                PrimitiveDataFrameColumn<float> col = new PrimitiveDataFrameColumn<float>("floatCol", this.df.Columns[colName].Length);
                for(int i = 0; i < this.df.Columns[colName].Length; i++)
                {
                    object currVal = this.df.Columns[colName][i];
                    string currString = currVal.ToString();
                    float currFloat = float.Parse(currString, CultureInfo.InvariantCulture.NumberFormat);
                    col[i] = currFloat;
                }

                List<float?> valsAsList = col.Where(val => val >= avgVal).ToList();
                upperQrtVal = this.CalculateMedian(valsAsList);
            }
            catch
            {
                throw new ApplicationException("Attempted to calculate stats on an illegal type");
            }

            this.statsDict[colName].Add(STATSVALS.UPPERQRT, upperQrtVal);
        }
        return this.statsDict[colName][STATSVALS.UPPERQRT];
    }

    public float GetIQR(string colName)
    {
        if (!this.statsDict.ContainsKey(colName))
        {
            this.statsDict.Add(colName, new Dictionary<STATSVALS, dynamic>());
        }

        float upperQrtVal;
        if (!this.statsDict[colName].ContainsKey(STATSVALS.UPPERQRT))
        {
            this.GetUpperQrt(colName);
        }
        upperQrtVal = this.statsDict[colName][STATSVALS.UPPERQRT];

        float lowerQrtVal;
        if (!this.statsDict[colName].ContainsKey(STATSVALS.LOWERQRT))
        {
            this.GetLowerQrt(colName);
        }
        lowerQrtVal = this.statsDict[colName][STATSVALS.LOWERQRT];

        if (!this.statsDict[colName].ContainsKey(STATSVALS.IQR))
        {
            float iqrVal = upperQrtVal - lowerQrtVal;
            this.statsDict[colName].Add(STATSVALS.IQR, iqrVal);
        }
        return this.statsDict[colName][STATSVALS.IQR];
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

    public DataObj SliceByAttribute(string attrName, IComparable startVal, IComparable endVal)
    {
        CheckValidAttr(attrName);
        List<bool> lowerBools = new List<bool>();
        List<bool> upperBools = new List<bool>();
        for (int i = 0; i < df.Columns[attrName].Length; i++)
        {
            IComparable currVal;
            try
            {
                currVal = (IComparable)df.Columns[attrName][i];
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

    public void ParseDateColumns()
    {
        for(int i = 0; i < df.Columns.Count; i++)
        {
            StringDataFrameColumn col;
            try
            {
                col = (StringDataFrameColumn)df.Columns[i];
            }
            catch
            {
                continue;
            }

            PrimitiveDataFrameColumn<DateTime> newCol = AttemptParseDateCol((StringDataFrameColumn)col);
            if(newCol != null)
            {
                df.Columns[i] = newCol;
            }
        }
    }

    public DateTime CreateDateFromString(string dateString)
    {
        if (DateTime.TryParseExact(dateString, dateFormats, null,
                        System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                        System.Globalization.DateTimeStyles.AdjustToUniversal,
                        out DateTime parsedDate))
        {
            return parsedDate;
        }
        throw new ArgumentException("Could not parse the given date string to a DateTime variable: " + dateString);
    }

    public PrimitiveDataFrameColumn<DateTime> AttemptParseDateCol(DataFrameColumn col)
    {
        PrimitiveDataFrameColumn<DateTime> newCol = new PrimitiveDataFrameColumn<DateTime>(col.Name, col.Length);
        DateTimeModel dTM = new DateTimeRecognizer(Culture.English).GetDateTimeModel();
        
        for (int i = 0; i < col.Length; i++)
        {
            List<ModelResult> results = dTM.Parse((string)col[i]);
            if (results.Count > 0)
            {
                List<Dictionary<string, string>> valueArray = (List<Dictionary<string, string>>)results[0].Resolution["values"];
                if (CheckIfTimeOnly(valueArray[0]["value"]))
                {
                    return null;
                }
                DateTime currDate = CreateDateFromString(valueArray[0]["value"]);
                // Date Parser strips milliseconds, so add them back
                int numMillis = CheckForMillis((String)col[i]);
                currDate = currDate.AddMilliseconds(numMillis);
                newCol[i] = currDate;
            }
            else
            {
                return null;
            }
        }
        return newCol;
    }

    public bool CheckIfTimeOnly(string dateString)
    {
        string pattern = @"^\d\d:\d\d:\d\d\.?\d{0,3}$";
        Regex rg = new Regex(pattern);
        MatchCollection matches = rg.Matches(dateString);

        return matches.Count > 0 ;
    }

    public int CheckForMillis(String val)
    {
        int numMillis = 0;
        string patternOne = @"\.\d{1,3}\s";
        string patternTwo = @"\.\d{1,3}$";
        Regex rgOne = new Regex(patternOne);
        Regex rgTwo = new Regex(patternTwo);
        MatchCollection matchesOne = rgOne.Matches(val);
        MatchCollection matchesTwo = rgTwo.Matches(val);

        string result;
        if (matchesOne.Count <= 0 && matchesTwo.Count <= 0)
        {
            return numMillis;
        }

        MatchCollection matches = matchesOne.Count > 0 ? matchesOne : matchesTwo;

        result = matches[0].Value.Trim();
        result = result.Substring(1, result.Length - 1);
        if(result.Length == 1)
        {
            result += "00";
        }

        if(result.Length == 2)
        {
            result += "0";
        }

        try
        {
            numMillis = Int32.Parse(result);
        }
        catch (FormatException)
        {
            throw new FormatException("Unable to parse milliseconds");
        }

        return numMillis;
    }

    //TODO: Doesn't work for floats
    public int CountNumVals<T>(string colName, T value) where T : IComparable
    {
        int counter = 0;
        for(int i = 0; i < df.Columns[colName].Length; i++)
        {
            T currVal;

            try
            {
                currVal = (T)Convert.ChangeType(df.Columns[colName][i], typeof(T));
            }
            catch
            {
                throw new InvalidCastException("Attempted an illegal cast");
            }
            if(value.CompareTo(currVal) == 0)
            {
                counter++;
            }
        }
        return counter;
    }

    public List<IComparable> GetUniqueVals(string colName)
    {
        List<IComparable> retList = new List<IComparable>();
        DataFrameColumn col = df.Columns[colName];
        for(int i = 0; i < col.Length; i++)
        {
            if(retList.FindIndex(elem => elem.CompareTo(col[i]) == 0) < 0)
            {
                retList.Add((IComparable)col[i]);
            }
        }
        return retList;
    }

    public DataFrame MergeDf(DataFrame dfToMerge)
    {
        DataFrame clonedDf = df.Clone();
        for(int i = 0; i < dfToMerge.Columns.Count; i++)
        {
            clonedDf.Columns.Append(dfToMerge.Columns[i]);
        }
        return clonedDf;
    }
}
