using Mapbox.Unity.Map;
using Mapbox.Unity.Map.Strategies;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapContext : MonoBehaviour, IGraph
{
    [SerializeField]
    public Texture2D loadingTexture;

    [SerializeField]
    public Material tileMaterial;

    [SerializeField]
    public GameObject dataPointPrefab;

    public DataObj currDataObj;
    private List<GameObject> pointList = new List<GameObject>();
    string latColName;
    string lonColName;
    GameObject mapGo;
    float iconSize = 5;
    GameObject pointHolder;
    bool plottedOnce = false;

    public DataObj dataObj { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector3 minDpSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector3 maxDpSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void InitializeMapViz(DataObj dataObj, string latColName, string lonColName)
    {
        this.currDataObj = dataObj;
        this.latColName = latColName;
        this.lonColName = lonColName;

        pointHolder = new GameObject();
        pointHolder.name = "PointHolder";

        Plot();
    }

    public void Plot()
    {
        if (!plottedOnce)
        {
            DrawMap();
            pointHolder.transform.parent = GetComponentInChildren<AbstractMap>().transform;
            pointHolder.transform.localPosition = Vector3.zero;
            PlotPoints();
            plottedOnce = true;
        }
    }

    void DrawMap()
    {
        mapGo = new GameObject();
        mapGo.transform.parent = transform;
        mapGo.transform.localPosition = Vector3.zero;
        mapGo.transform.localRotation = Quaternion.identity;
        mapGo.transform.localScale = Vector3.zero;

        float firstLat = (float)currDataObj.df.Columns[latColName][0];
        float firstLon = (float)currDataObj.df.Columns[lonColName][0];

        AbstractMap map = mapGo.AddComponent<AbstractMap>();
        map.Options = CreateMapOptions(firstLat, firstLon);

        map.ImageLayer.SetLayerSource(ImagerySourceType.MapboxSatellite);
        map.Terrain.SetProperties(ElevationSourceType.MapboxTerrain, ElevationLayerType.TerrainWithElevation, true, 1, 0);

        DataExtentOptions extentOptions = new DataExtentOptions();
        extentOptions.SetOptions(currDataObj, latColName, lonColName);

        GameObject tileProviderGo = new GameObject("TileProvider");
        tileProviderGo.transform.parent = mapGo.transform;
        DataTileProvider provider = tileProviderGo.AddComponent<DataTileProvider>();
        provider.SetOptions(extentOptions);
        map.TileProvider = provider;
        map.SetExtent(MapExtentType.Custom, extentOptions);

        map.Initialize(new Vector2d((double)firstLat, (double)firstLon), 13);
        map.UpdateMap();

        AddObjectManipulator oM = mapGo.AddComponent<AddObjectManipulator>();
        oM.PlaceObjectManipulator(mapGo.transform);
    }

    public MapOptions CreateMapOptions(float startLat, float startLon)
    {
        MapOptions mO = new MapOptions();

        MapExtentOptions extentOptions = new MapExtentOptions(MapExtentType.Custom);
        mO.extentOptions = extentOptions;

        MapLocationOptions locOp = new MapLocationOptions();
        locOp.latitudeLongitude = startLat.ToString() + "," + startLon.ToString();
        locOp.zoom = 13;
        mO.locationOptions = locOp;

        MapPlacementOptions mapPlac = new MapPlacementOptions();
        mapPlac.placementStrategy = new MapPlacementAtLocationCenterStrategy();
        mapPlac.placementType = MapPlacementType.AtLocationCenter;
        mapPlac.snapMapToZero = true;
        mO.placementOptions = mapPlac;

        MapScalingOptions mapScal = new MapScalingOptions();
        mapScal.scalingStrategy = new MapScalingAtUnityScaleStrategy();
        mapScal.scalingType = MapScalingType.Custom;
        mapScal.unityTileSize = 1;
        mO.scalingOptions = mapScal;

        mO.tileMaterial = tileMaterial;
        mO.loadingTexture = loadingTexture;

        return mO;
    }

    public void SetData(DataObj data)
    {
        this.currDataObj = data;
    }

    public DataObj GetData()
    {
        return currDataObj;
    }

    public List<GameObject> GetDataPoints()
    {
        return pointList;
    }

    void PlotPoints()
    {
        DataFrame df = currDataObj.df;
        AbstractMap map = mapGo.GetComponent<AbstractMap>();
        for(int i = 0; i < df.Rows.Count; i++)
        {
            Vector2d latLon = new Vector2d(Convert.ToDouble(df.Columns[latColName][i]), Convert.ToDouble(df.Columns[lonColName][i]));
            Vector3 pos = map.GeoToWorldPosition(latLon);
            Debug.Log("latlon: " + latLon);
            Debug.Log("Pos: " + pos);
            GameObject go = Instantiate(dataPointPrefab, pos, Quaternion.identity);
            float sizeMultiplier = CalculateScaleMultiplier();
            go.transform.localScale = go.transform.localScale * sizeMultiplier;
            go.transform.parent = pointHolder.transform;
            pointList.Add(go);
        }
    }

    float CalculateScaleMultiplier()
    {
        AbstractMap map = mapGo.GetComponent<AbstractMap>();
        int numDecimals = 3;
        float newSizeMultiplier = map.UnityTileSize * map.transform.localScale.x / iconSize;
        return (float)Math.Round(newSizeMultiplier, numDecimals);
    }

    public Vector3 GetMaxDpScale()
    {
        float scaleMultiplier = CalculateScaleMultiplier();
        return new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }

    public Vector3 GetMinDpScale()
    {
        float min = CalculateScaleMultiplier() / 4;
        return new Vector3(min, min, min);
    }
}