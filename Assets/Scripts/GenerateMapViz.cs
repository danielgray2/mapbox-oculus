using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapViz : MonoBehaviour
{
    [SerializeField]
    GameObject mapPrefab;

    // Start is called before the first frame update
    void Start()
    {
        DataObj dO = GenerateData();
        GameObject mapGO = Instantiate(mapPrefab);
        MapViz mV = mapGO.GetComponent<MapViz>();
        mV.InitializeMapViz(dO, "lat", "lon");
        mV.Plot();
    }

    DataObj GenerateData()
    {
        PrimitiveDataFrameColumn<float> lat = new PrimitiveDataFrameColumn<float>("lat", 10);
        PrimitiveDataFrameColumn<float> lon = new PrimitiveDataFrameColumn<float>("lon", 10);

        lat[0] = 36.10420f;
        lat[1] = 36.01070f;
        lat[2] = 36.01070f;
        lat[3] = 36.08100f;
        lat[4] = 36.08700f;
        lat[5] = 36.10270f;
        lat[6] = 36.01070f;
        lat[7] = 36.08100f;
        lat[8] = 36.01070f;
        lat[9] = 36.09780f;

        lon[0] = -117.86530f;
        lon[1] = -117.88680f;
        lon[2] = -117.88680f;
        lon[3] = -117.86730f;
        lon[4] = -117.87130f;
        lon[5] = -117.91050f;
        lon[6] = -117.88680f;
        lon[7] = -117.86730f;
        lon[8] = -117.88680f;
        lon[9] = -117.89900f;

        DataFrame df = new DataFrame(lat, lon);
        DataObj dO = new DataObj(df);

        return dO;
    }
}
