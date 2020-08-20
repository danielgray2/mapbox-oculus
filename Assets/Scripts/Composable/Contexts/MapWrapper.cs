using Mapbox.Unity.Map;
using Mapbox.Utils;
using System;
using UnityEngine;

public class MapWrapper : IAbstractWrapper
{
    [SerializeField]
    public Texture2D loadingTexture;

    [SerializeField]
    public Material tileMaterial;

    GameObject mapGo;
    AddObjectManipulator oM;
    Map wrapped;
    bool plottedOnce = false;
    bool initialized = false;

    public override void Create()
    {
        throw new NotImplementedException();
    }

    // Make sure to add the AbstractMap
    // to the map model
    public void Initialize(MapModel model)
    {
        if (!initialized)
        {
            wrapped = new Map(model);
            this.model = model;
            model.modelUpdateEvent.AddListener(HandleModelUpdate);
            // Register for updates here
            initialized = true;
        }
    }

    public void Plot()
    {
        if (initialized && !plottedOnce)
        {
            DrawMap();
            oM = this.gameObject.AddComponent<AddObjectManipulator>();
            oM.PlaceObjectManipulator(this.transform);

            plottedOnce = true;
        }
    }

    public override void ReRender()
    {
        Destroy(mapGo);
        DrawMap();
        oM.UpdateObjectManipulator();
    }

    void DrawMap()
    {
        MapModel mapModel = CastToMapModel();
        DataObj dataObj = mapModel.dataObj;
        string latColName = mapModel.latColName;
        string lonColName = mapModel.lonColName;

        mapGo = new GameObject();
        mapGo.transform.parent = transform;
        mapGo.transform.localPosition = Vector3.zero;
        mapGo.transform.localRotation = Quaternion.identity;
        mapGo.transform.localScale = Vector3.zero;

        float firstLat = (float)dataObj.df.Columns[latColName][0];
        float firstLon = (float)dataObj.df.Columns[lonColName][0];

        AbstractMap absMap = mapGo.AddComponent<AbstractMap>();
        CreateMapOptions(firstLat, firstLon);
        absMap.Options = mapModel.mO;

        absMap.ImageLayer.SetLayerSource(mapModel.imagerySourceType);
        absMap.Terrain.SetProperties(mapModel.mapboxTerrain, mapModel.elevationLayerType, true, 1, 0);

        DataExtentOptions extentOptions = new DataExtentOptions();
        extentOptions.SetOptions(dataObj, latColName, lonColName);

        GameObject tileProviderGo = new GameObject("TileProvider");
        tileProviderGo.transform.parent = mapGo.transform;
        DataTileProvider provider = tileProviderGo.AddComponent<DataTileProvider>();
        provider.SetOptions(extentOptions);
        absMap.TileProvider = provider;
        absMap.SetExtent(mapModel.mapExtentType, extentOptions);
        absMap.Terrain.ExaggerationFactor = mapModel.exaggerationFactor;

        absMap.Initialize(new Vector2d((double)firstLat, (double)firstLon), mapModel.zoom);
        absMap.UpdateMap();
        wrapped.UpdateAbsMap(absMap);

        wrapped.UpdateMaxDpScale(wrapped.CalcMaxDpScale(absMap));
        wrapped.UpdateMinDpScale(wrapped.CalcMinDpScale(absMap));

        AddObjectManipulator oM = mapGo.AddComponent<AddObjectManipulator>();
        oM.PlaceObjectManipulator(mapGo.transform);
    }

    void CreateMapOptions(float firstLat, float firstLon)
    {
        MapModel mapModel = CastToMapModel();
        wrapped.CreateMapOptions(firstLat, firstLon);
        mapModel.mO.tileMaterial = tileMaterial;
        mapModel.mO.loadingTexture = loadingTexture;
    }

    MapModel CastToMapModel()
    {
        if (!(model is MapModel mapModel))
        {
            throw new ArgumentException("Model must be of type MapModel");
        }
        return mapModel;
    }
}