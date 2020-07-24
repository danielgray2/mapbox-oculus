using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
//using UnityEngine.Rendering.Universal.Internal;

public class GenerateHistogram : MonoBehaviour
{
    [SerializeField]
    GameObject histogramPrefab;
    DataFrame df;
    DataObj dO;
    Dictionary<int, int> valuesDict = new Dictionary<int, int>();

    private void Start()
    {
        SetupData();
        GameObject histoGo = Instantiate(histogramPrefab, Vector3.zero, Quaternion.identity);
        Histogram hObj = histoGo.GetComponentInChildren<Histogram>();
        hObj.InitializeHistogram(dO, "ccmadRatio");
        AddObjectManipulator oM = histoGo.AddComponent<AddObjectManipulator>();
        oM.PlaceObjectManipulator(histoGo.transform);

        foreach(KeyValuePair<int, int> keyVal in valuesDict)
        {
            //Debug.Log(keyVal.Key + ": " + keyVal.Value);
        }
    }

    private void SetupData()
    {
        PrimitiveDataFrameColumn<int> colOne = new PrimitiveDataFrameColumn<int>("ccmadRatio", 1000);
        System.Random random = new System.Random();
        for(int i = 0; i < 1000; i++)
        {
            int value = random.Next(1000, 1100);
            if (!valuesDict.ContainsKey(value))
            {
                valuesDict.Add(value, 0);
            }
            valuesDict[value]++;
            colOne[i] = value;
        }
        df = new DataFrame(colOne);
        dO = new DataObj(df);
    }
}

