using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using UnityEngine;

public class VeloCreator : MonoBehaviour
{
    [SerializeField]
    public GameObject mapPrefab;

    [SerializeField]
    public GameObject meshPrefab;

    GameObject mapGo;
    GameObject meshGo;
    MapWrapper mapWrapper;
    MapModel mapModel;
    MeshModel meshModel;
    MeshVizWrapper meshWrapper;
    DataObj origDo;

    void Start()
    {
        origDo = new DataObj(DataFrame.LoadCsv("Assets\\Resources\\coso_velo_for_demo_two.csv"));

        CreateMap();
        CreateMesh();
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

    void CreateMesh()
    {
        DataObj meshDo = CreateMeshDo(origDo);
        meshModel = CreateMeshModel();
        meshModel.dataObj = meshDo;
        meshGo = Instantiate(meshPrefab, Vector3.zero, Quaternion.identity);
        meshWrapper = meshGo.GetComponent<MeshVizWrapper>();
        meshWrapper.transform.parent = mapModel.absMap.transform;
        meshWrapper.transform.localPosition = Vector3.zero;
        
        meshWrapper.Initialize(meshModel);
        meshWrapper.Plot();
        //mapWrapper.ReRender();
    }

    MapModel CreateMapModel()
    {
        MapModel mapModel = new MapModel();


        mapModel.latColName = "lat";
        mapModel.lonColName = "lon";
        mapModel.exaggerationFactor = 1;

        return mapModel;
    }

    MeshModel CreateMeshModel()
    {
        MeshModel meshModel = new MeshModel();
        meshModel.yCol = "depth(km)";
        meshModel.xCol = "lon";
        meshModel.zCol = "lat";
        meshModel.valueCol = "Vp/Vs";

        return meshModel;
    }

    DataObj CreateMeshDo(DataObj dO)
    {
        DataFrame returnDf = new DataFrame();
        DepthLatLonTransf transf = new DepthLatLonTransf("depth(km)", "lat", "lon", mapModel.absMap, mapModel.exaggerationFactor);
        DataObj newDo = transf.ApplyTransformation(dO);
        foreach(DataFrameColumn ndf in newDo.df.Columns)
        {
            returnDf.Columns.Add(ndf);
        }

        foreach(DataFrameColumn odf in dO.df.Columns)
        {
            returnDf.Columns.Add(odf);
        }
        return new DataObj(returnDf);
    }

    protected Vector2d ConvertLatLon(float lat, float lon)
    {
        string locString = lat + ", " + lon;
        return Conversions.StringToLatLon(locString);
    }
}