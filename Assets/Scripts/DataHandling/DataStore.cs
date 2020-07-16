using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Data.Analysis;
using JetBrains.Annotations;

using UnityEngine;
using Mapbox.Map;

public sealed class DataStore
{
    public Dictionary<string, DataObj> dataMap = new Dictionary<string, DataObj>();
    public List<SData> sDataRecords { get; private set;}
    public List<VData> vDataRecords { get; private set; }
    public List<int> triangles { get; private set; }
    public float minLat { get; private set; }
    public float maxLat { get; private set; }
    public float avgLat { get; private set; }
    public float minLon { get; private set; }
    public float maxLon { get; private set; }
    public float avgLon { get; private set; }
    public float minCCMad { get; private set; }
    public float maxCCMad { get; private set; }
    public float avgCCMad { get; private set; }
    public float medianCCMad { get; private set; }
    public float lowerQCCMad { get; private set; }
    public float upperQCCMad { get; private set; }
    public List<List<List<VData>>> depthList { get; private set; }
    public float maxVpVs { get; private set; }
    public float minVpVs { get; private set; }

    private static DataStore instance = null;
    private static readonly object padlock = new object();

    DataStore() { }

    public static DataStore Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new DataStore();
                }
                return instance;
            }
        }
    }

    public void AddDataSet(DataFrame df, string name)
    {
        DataObj dO = new DataObj(df);
        dataMap.Add(name, dO);
        /*
        this.sDataRecords = sDataRecords;

        minLat = this.sDataRecords.Min(record => record.lat);
        maxLat = this.sDataRecords.Max(record => record.lat);
        avgLat = this.sDataRecords.Average(record => record.lat);

        minLon = this.sDataRecords.Min(record => record.lon);
        maxLon = this.sDataRecords.Max(record => record.lon);
        avgLon = this.sDataRecords.Average(record => record.lon);

        minCCMad = this.sDataRecords.Min(record => record.ccmadRatio);
        maxCCMad = this.sDataRecords.Max(record => record.ccmadRatio);
        avgCCMad = this.sDataRecords.Average(record => record.ccmadRatio);

        medianCCMad = CalculateMedian(this.sDataRecords.Select(record => record.ccmadRatio).ToList());
        lowerQCCMad = CalculateMedian(this.sDataRecords.Select(record => record.ccmadRatio).Where(record => record < medianCCMad).ToList());
        upperQCCMad = CalculateMedian(this.sDataRecords.Select(record => record.ccmadRatio).Where(record => record >= medianCCMad).ToList());
        */
    }

    public void SetVDataRecords(List<VData> vDataRecords)
    {
        this.vDataRecords = vDataRecords;
        minVpVs = this.vDataRecords.Min(record => record.vPvS);
        maxVpVs = this.vDataRecords.Max(record => record.vPvS);
    }

    // Returns a dictionary where int is the index of the last element
    // returned by the array in the original array. The dictionary will
    // only have 1 element.
    public DataSlice SliceIndexTime(int indexInArray, float hours)
    {
        List<SData> dataList = new List<SData>();
        DateTime startTime = sDataRecords.ElementAt(indexInArray).dateTime;
        DateTime endTime = startTime.AddHours(hours);

        dataList.Add(sDataRecords.ElementAt(indexInArray));
        DateTime currTime = startTime;
        int currIndex = indexInArray;

        while(currTime <= endTime)
        {
            currIndex++;
            currTime = sDataRecords.ElementAt(currIndex).dateTime;
            dataList.Add(sDataRecords.ElementAt(currIndex));
        }

        return new DataSlice(dataList, currIndex);
    }

    
    public DataSlice SliceTimes(DateTime startTime, DateTime endTime)
    {
        List<SData> dataList = new List<SData>();

        DateTime currTime = sDataRecords.ElementAt(0).dateTime;
        int currIndex = 0;

        while (currTime < startTime)
        {
            currIndex++;
            currTime = sDataRecords.ElementAt(currIndex).dateTime;
        }

        while(currTime < endTime)
        {
            dataList.Add(sDataRecords.ElementAt(currIndex));
            currIndex++;
            currTime = sDataRecords.ElementAt(currIndex).dateTime;
        }

        return new DataSlice(dataList, currIndex);
    }
    

    // Returns the index of the first element
    // with time >= dateTime
    public int GetIndexTime(DateTime dateTime)
    {
        int currIndex = 0;
        DateTime currTime = sDataRecords[currIndex].dateTime;
        while(currTime < dateTime)
        {
            currIndex++;
            currTime = sDataRecords[currIndex].dateTime;
        }
        return currIndex;
    }

    // There is a faster way to do this (Median Selection). This will
    // work for now though.
    public float CalculateMedian(List<float> values)
    {
        List<float> sortedList = new List<float>(values);
        sortedList.Sort();

        int length = sortedList.Count;
        int midPoint = length / 2;
        return length % 2 == 0 ? sortedList.ElementAt(midPoint) : ((sortedList.ElementAt(midPoint) + sortedList.ElementAt(midPoint + 1)) / 2);
    }

    // This can be optimized by sorting each depth, then each
    // lon grouped by depth, then each lat grouped by lon
    public List<List<List<VData>>> CreateMeshStructure()
    {
        string strForm = "0.000";
        Dictionary<string, Dictionary<string, Dictionary<string, List<VData>>>> dataDict = new Dictionary<string, Dictionary<string, Dictionary<string, List<VData>>>>();
        
        // Get the depths
        HashSet<float> depthHashSet = new HashSet<float>();
        foreach(VData vData in this.vDataRecords)
        {
            depthHashSet.Add(vData.depth);
        }

        foreach(float depth in depthHashSet)
        {
            Dictionary<string, Dictionary<string, List<VData>>> subDict = new Dictionary<string, Dictionary<string, List<VData>>>();
            dataDict.Add(depth.ToString(strForm), subDict);
        }

        // Add the longitudes
        foreach(VData vData in this.vDataRecords)
        {
            Dictionary<string, List<VData>> subDict = new Dictionary<string, List<VData>>();
            if (!dataDict[vData.depth.ToString(strForm)].ContainsKey(vData.lon.ToString(strForm)))
                dataDict[vData.depth.ToString(strForm)].Add(vData.lon.ToString(strForm), subDict);
        }

        // Add the latitudes
        foreach(VData vData in this.vDataRecords)
        {
            List<VData> subList = new List<VData>();
            if(!dataDict[vData.depth.ToString(strForm)][vData.lon.ToString(strForm)].ContainsKey(vData.lat.ToString(strForm)))
                dataDict[vData.depth.ToString(strForm)][vData.lon.ToString(strForm)].Add(vData.lat.ToString(strForm), subList);
        }

        // Add the data vDatas
        foreach(VData vData in this.vDataRecords)
        {
            dataDict[vData.depth.ToString(strForm)][vData.lon.ToString(strForm)][vData.lat.ToString(strForm)].Add(vData);
        }

        // Convert dataDict to 3D List
        List<List<List<VData>>> depthList = new List<List<List<VData>>>();
        List<string> depthKeys = sortStringListAsFloats(dataDict.Keys.ToList(), strForm);
        foreach (string depth in depthKeys)
        {
            List<List<VData>> lonList = new List<List<VData>>();
            List<string> lonKeys = sortStringListAsFloats(dataDict[depth].Keys.ToList(), strForm);
            foreach (string lon in lonKeys)
            {
                List<VData> latList = new List<VData>();
                List<string> latKeys = sortStringListAsFloats(dataDict[depth][lon].Keys.ToList(), strForm);
                foreach (string lat in latKeys)
                {
                    foreach(VData vData in dataDict[depth][lon][lat])
                    {
                        latList.Add(vData);
                    }
                }
                lonList.Add(latList);
            }
            depthList.Add(lonList);
        }
        this.depthList = depthList;
        return depthList;
    }

    public List<string> sortStringListAsFloats(List<string> stringList, string strFormat)
    {
        List<string> returnList = new List<string>();
        List<float> floatList = new List<float>();
        foreach(string currString in stringList)
        {
            float asFloat = float.Parse(currString, CultureInfo.InvariantCulture.NumberFormat);
            floatList.Add(asFloat);
        }
        floatList.Sort();
        foreach(float currFloat in floatList)
        {
            returnList.Add(currFloat.ToString(strFormat));
        }
        return returnList;
    }
}
