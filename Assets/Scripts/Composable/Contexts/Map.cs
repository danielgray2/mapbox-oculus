using Mapbox.Unity.Map;
using Mapbox.Unity.Map.Strategies;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : IAbstractContext
{
    public Map(MapModel model)
    {
        this.model = model;
    }

    public void CreateMapOptions(float startLat, float startLon)
    {
        MapModel mapModel = CastToMapModel();
        MapOptions mO = new MapOptions();

        MapExtentOptions extentOptions = new MapExtentOptions(mapModel.mapExtentType);
        mO.extentOptions = extentOptions;

        MapLocationOptions locOp = new MapLocationOptions();
        locOp.latitudeLongitude = startLat.ToString() + "," + startLon.ToString();
        locOp.zoom = mapModel.zoom;
        mO.locationOptions = locOp;

        MapPlacementOptions mapPlac = new MapPlacementOptions();
        mapPlac.placementStrategy = new MapPlacementAtLocationCenterStrategy();
        mapPlac.placementType = mapModel.mapPlacementType;
        mapPlac.snapMapToZero = mapModel.snapMapToZero;
        mO.placementOptions = mapPlac;

        MapScalingOptions mapScal = new MapScalingOptions();
        mapScal.scalingStrategy = new MapScalingAtUnityScaleStrategy();
        mapScal.scalingType = MapScalingType.Custom;
        mapScal.unityTileSize = mapModel.unityTileSize;
        mO.scalingOptions = mapScal;

        mapModel.mO = mO;
    }

    /*
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
    */

    float CalculateScaleMultiplier(AbstractMap map)
    {
        //AbstractMap map = mapGo.GetComponent<AbstractMap>();
        MapModel mapModel = CastToMapModel();
        int numDecimals = 3;
        float newSizeMultiplier = map.UnityTileSize * map.transform.localScale.x / mapModel.iconSize;
        return (float)Math.Round(newSizeMultiplier, numDecimals);
    }

    public Vector3 CalcMaxDpScale(AbstractMap map)
    {
        float scaleMultiplier = CalculateScaleMultiplier(map);
        return new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }

    public Vector3 CalcMinDpScale(AbstractMap map)
    {
        float min = CalculateScaleMultiplier(map) / 4;
        return new Vector3(min, min, min);
    }

    public void UpdateAbsMap(AbstractMap map)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.absMap = map;
    }

    MapModel CastToMapModel()
    {
        if (!(model is MapModel mapModel))
        {
            throw new ArgumentException("Model must be of type HistModel");
        }
        return mapModel;
    }

    public override void Generate()
    {
        throw new NotImplementedException();
    }

    public override void Initialize(IModel iModel)
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public void UpdateMaxDpScale(Vector3 newMax)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.maxDpSize = newMax;
    }

    public void UpdateMinDpScale(Vector3 newMin)
    {
        MapModel mapModel = CastToMapModel();
        mapModel.minDpSize = newMin;
    }
}