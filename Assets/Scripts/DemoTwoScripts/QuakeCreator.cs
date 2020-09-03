using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeCreator : MonoBehaviour
{
    [SerializeField]
    public GameObject mapPrefab;

    [SerializeField]
    public GameObject dPrefab;

    [SerializeField]
    GameObject menu;

    GameObject mapGo;
    List<GameObject> dataPoints;
    MapModel mapModel;
    DataObj origDo;
    DataObj currDo;
    MapWrapper mapWrapper;
    private string[] dateFormats = { "yyyy-MM-dd;HH:mm:ss.fff", "yyyy-MM-dd;HH:mm" };

    void Start()
    {
        origDo = new DataObj(DataFrame.LoadCsv("Assets\\Resources\\coso_for_demo_two.csv"));
        currDo = origDo;
        menu.GetComponent<MonthMenu>().DateUpdated.AddListener(HandleScrubbing);
        dataPoints = new List<GameObject>();

        CreateMap();
        PlotPoints();
    }

    void CreateMap()
    {
        mapModel = CreateMapModel();
        mapModel.dataObj = origDo;
        mapGo = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        mapWrapper = mapGo.GetComponent<MapWrapper>();
        mapWrapper.Initialize(mapModel);
        mapWrapper.Plot();
    }

    MapModel CreateMapModel()
    {
        MapModel mapModel = new MapModel();


        mapModel.latColName = "lat";
        mapModel.lonColName = "lon";
        mapModel.exaggerationFactor = 1;

        return mapModel;
    }

    void PlotPoints()
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            Destroy(dataPoints[i]);
        }

        dataPoints = new List<GameObject>();
        DataObj pointsDo = GetUnityPositions(currDo);

        for(int i = 0; i < pointsDo.df.Rows.Count; i++)
        {
            GameObject currDp = Instantiate(dPrefab);
            currDp.transform.parent = mapModel.absMap.transform;
            Vector3 currPos = new Vector3((float)pointsDo.df.Columns["XVals"][i], (float)pointsDo.df.Columns["YVals"][i], (float)pointsDo.df.Columns["ZVals"][i]);
            currDp.transform.position = currPos;
            dataPoints.Add(currDp);
        }
    }

    DataObj GetUnityPositions(DataObj dO)
    {
        DataFrame returnDf = new DataFrame();
        LatLonTransf transf = new LatLonTransf("lat", "lon", mapModel.absMap);
        DataObj newDo = transf.ApplyTransformation(dO);
        foreach (DataFrameColumn ndf in newDo.df.Columns)
        {
            returnDf.Columns.Add(ndf);
        }

        foreach (DataFrameColumn odf in dO.df.Columns)
        {
            returnDf.Columns.Add(odf);
        }
        return new DataObj(returnDf);
    }

    public void HandleScrubbing(List<DateTime> startAndEndTimes)
    {
        List<bool> boolList = new List<bool>();

        for(int i = 0; i < origDo.df.Rows.Count; i++)
        {
            DateTime currDateTime = ParseDateTime((string)origDo.df.Columns["dateTime"][i]);
            if(currDateTime >= startAndEndTimes[0] && currDateTime <= startAndEndTimes[1])
            {
                boolList.Add(true);
            }
            else
            {
                boolList.Add(false);
            }
        }

        PrimitiveDataFrameColumn<bool> filterCol = new PrimitiveDataFrameColumn<bool>("filter", boolList);

        currDo = new DataObj(origDo.df.Filter(filterCol));
        PlotPoints();
    }

    protected Vector2d ConvertLatLon(float lat, float lon)
    {
        string locString = lat + ", " + lon;
        return Conversions.StringToLatLon(locString);
    }

    public DateTime ParseDateTime(string newDate)
    {
        DateTime parsedDate;
        if (DateTime.TryParseExact(newDate, dateFormats, null,
                        System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                        System.Globalization.DateTimeStyles.AdjustToUniversal,
                        out parsedDate))
        {
            return parsedDate;
        }
        else
        {
            throw new ArgumentException("Could not parse the given date string to a DateTime variable: " + newDate);
        }
    }
}
