
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
        ReadCSV("Assets\\path\\to\\csv", "data name");
    }

    private void ReadCSV(string pathToCsv, string name)
    {
        DataFrame csvDataFrame = DataFrame.LoadCsv(pathToCsv);
        DataStore.Instance.AddDataSet(csvDataFrame, name);
    }
}