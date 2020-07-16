using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal.Internal;

public class GenerateHistogram : MonoBehaviour
{
    [SerializeField]
    GameObject histogramPrefab;
    List<SData> dataList = new List<SData>();
    Dictionary<int, int> valuesDict = new Dictionary<int, int>();

    private void Start()
    {
        SetupData();
        GameObject histoGo = Instantiate(histogramPrefab, Vector3.zero, Quaternion.identity);
        Histogram hObj = histoGo.GetComponentInChildren<Histogram>();
        hObj.InitializeHistogram(dataList, "ccmadRatio");

        foreach(KeyValuePair<int, int> keyVal in valuesDict)
        {
            //Debug.Log(keyVal.Key + ": " + keyVal.Value);
        }
    }

    private void SetupData()
    {
        System.Random random = new System.Random();
        for(int i = 0; i < 1000; i++)
        {
            SData sData = new SData();
            int value = random.Next(1000, 1100);
            sData.ccmadRatio = value;
            if (!valuesDict.ContainsKey(value))
            {
                valuesDict.Add(value, 0);
            }
            valuesDict[value]++;
            dataList.Add(sData);
        }
    }
}

