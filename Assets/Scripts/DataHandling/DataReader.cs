
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;

public class DataReader : MonoBehaviour
{
    [SerializeField]
    GameObject dataPointPrefab;

    [SerializeField]
    GameObject scrubbingMenu;
    void Start()
    {
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
        GameObject plotAnimation = new GameObject();
        plotAnimation.transform.position = Vector3.zero;
        TimeSeries ts = plotAnimation.AddComponent<TimeSeries>();
        ts.dataPointPrefab = dataPointPrefab;
        //ts.BeginTimeAnimation(DataStore.Instance.sDataRecords.ElementAt(0).dateTime, 1f);
        //ts.BeginIndexedAnimation(0, 70, 1f);
        ts.BeginScrubbedAnimation(scrubbingMenu);
    }
}