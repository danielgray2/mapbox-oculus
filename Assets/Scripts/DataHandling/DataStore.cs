using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mapbox.Unity.MeshGeneration.Data;
using Microsoft.Data.Analysis;

public sealed class DataStore
{
    public Dictionary<string, DataObj> dataDict = new Dictionary<string, DataObj>();
    public List<SData> sDataRecords { get; private set;}
    public List<VData> vDataRecords { get; private set; }
    ///*
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
    //*/
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

    public void FakeAddDataSet(DataFrame df, string name)
    {
        DataObj dO = new DataObj(df);
        dataDict.Add(name, dO);
    }

    public void AddDataSet(DataFrame df, string name)
    {
        DataObj dO = new DataObj(df);
        dataDict.Add(name, dO);
        ///*
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
        //*/
    }

    public void SetVDataRecords(List<VData> vDataRecords)
    {
        this.vDataRecords = vDataRecords;
        //minVpVs = this.vDataRecords.Min(record => record.vPvS);
        //maxVpVs = this.vDataRecords.Max(record => record.vPvS);
    }
    
    // Returns the index of the first element
    // with value >= val
    /*
    public int GetIndexAttr<T>(string dfName, string attrName, T val)
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
    */

    // There is a faster way to do this (Median Selection). This will
    // work for now though.
    /*
    public float CalculateMedian(List<float> values)
    {
        List<float> sortedList = new List<float>(values);
        sortedList.Sort();

        int length = sortedList.Count;
        int midPoint = length / 2;
        return length % 2 == 0 ? sortedList.ElementAt(midPoint) : ((sortedList.ElementAt(midPoint) + sortedList.ElementAt(midPoint + 1)) / 2);
    }
    */

    public float CalculateMedian(List<float> dataList) { return 0f; }

    public DataSlice SliceByIndex() { return new DataSlice(new List<SData>(), 0); }
    public int GetIndexTime(DateTime startTime) { return 0; }
    public DataSlice SliceIndexTime(float currIndex, float hours) { return new DataSlice(new List<SData>(), 0); }

    public DataSlice SliceTimes(DateTime startTime, DateTime endTime) { return new DataSlice(new List<SData>(), 0); }
}
