using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class DataStore
{
    public List<SData> sDataRecords { get; private set;}
    public List<VData> vDataRecords { get; private set; }
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

    public void SetSDataRecords(List<SData> sDataRecords)
    {
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
        lowerQCCMad = CalculateMedian(this.sDataRecords.Select(record => record.ccmadRatio).Where(record => record <= medianCCMad).ToList());
        upperQCCMad = CalculateMedian(this.sDataRecords.Select(record => record.ccmadRatio).Where(record => record >= medianCCMad).ToList());
    }

    public void SetVDataRecords(List<VData> vDataRecords)
    {
        this.vDataRecords = vDataRecords;
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
}
