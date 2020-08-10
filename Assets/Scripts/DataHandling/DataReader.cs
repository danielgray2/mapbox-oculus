
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;
using UnityEngine.Assertions;
using Microsoft.Data.Analysis;
using System.Security.Permissions;

public class DataReader : MonoBehaviour
{
    [SerializeField]
    GameObject dataPointPrefab;

    [SerializeField]
    GameObject scrubbingMenu;

    [SerializeField]
    Gradient sphereColorGradient;

    void Start()
    {
        ReadCSV("Assets\\Resources\\coso.csv", "testdata");
        //ReadSeismicData();
        //ReadVeloData();
        
        //GameObject plotAnimation = new GameObject();
        //plotAnimation.transform.position = Vector3.zero;
        //TimeSeries ts = plotAnimation.AddComponent<TimeSeries>();
        //ts.dataPointPrefab = dataPointPrefab;
        //ts.BeginTimeAnimation(DataStore.Instance.sDataRecords.ElementAt(0).dateTime, 1f, gradient);
        //ts.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
        //ts.BeginScrubbedAnimation(scrubbingMenu, gradient);
    }

    private void ReadCSV(string pathToCsv, string name)
    {
        DataFrame csvDataFrame = DataFrame.LoadCsv(pathToCsv);
        DataStore.Instance.FakeAddDataSet(csvDataFrame, name);
        //GroupBy groupBy = csvDataFrame.GroupBy("colOne");
        //DataFrame counts = groupBy.Count();
        //Debug.Log(counts);
        /*
        using (var reader = new StreamReader("Assets\\Resources\\coso.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            List<SData> sDataRecords = new List<SData>();
            csv.Configuration.RegisterClassMap<CSVToObjectMap>();
            var sDataEnumberable = csv.GetRecords<SData>();
            foreach (SData sData in sDataEnumberable)
            {
                sData.SetDateTime();
                sData.SetLatLon();
                sDataRecords.Add(sData);
            }
            DataStore.Instance.SetSDataRecords(sDataRecords);
        }
        */
    }

    private void ReadVeloData()
    {
        using (var reader = new StreamReader("Assets\\Resources\\coso_velo.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            List<VData> vDataRecords = new List<VData>();
            csv.Configuration.RegisterClassMap<CSVToVData>();
            var vDataEnumerable = csv.GetRecords<VData>();
            foreach (VData vData in vDataEnumerable)
            {
                if (CheckBounds(vData.lat, vData.lon) && vData.depth > -2f)
                {
                    vData.SetLatLon();
                    vDataRecords.Add(vData);
                }
                
            }
            DataStore.Instance.SetVDataRecords(vDataRecords);
        }
    }

    private bool CheckBounds(float lat, float lon)
    {
        if (lat < DataStore.Instance.minLat ||
           lat > DataStore.Instance.maxLat ||
           lon < DataStore.Instance.minLon ||
           lon > DataStore.Instance.maxLon)
            return false;
        return true;
    }
}