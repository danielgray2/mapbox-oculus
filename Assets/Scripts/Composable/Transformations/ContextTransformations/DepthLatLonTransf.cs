using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using UnityEngine;

public class DepthLatLonTransf : IAbsTransf
{
    public string xColName { get; protected set; } = "XVals";
    public string yColName { get; protected set; } = "YVals";
    public string zColName { get; protected set; } = "ZVals";
    public string depthColName { get; set; }
    public string latColName { get; set; }
    public string lonColName { get; set; }
    public AbstractMap absMap { get; set; }

    public static new string friendlyName { get; protected set; } = "DepthLatLonTransf";

    public DepthLatLonTransf(string depthColName, string latColName, string lonColName, AbstractMap absMap)
    {
        this.depthColName = depthColName;
        this.absMap = absMap;
        this.latColName = latColName;
        this.lonColName = lonColName;
    }
    public override DataObj ApplyTransformation(DataObj dO)
    {
        int dfLength = (int)dO.df.Rows.Count;
        PrimitiveDataFrameColumn<float> xVals = new PrimitiveDataFrameColumn<float>(xColName, dfLength);
        PrimitiveDataFrameColumn<float> yVals = new PrimitiveDataFrameColumn<float>(yColName, dfLength);
        PrimitiveDataFrameColumn<float> zVals = new PrimitiveDataFrameColumn<float>(zColName, dfLength);

        DataFrame df = dO.df;

        for(int i = 0; i < df.Rows.Count; i++)
        {
            Vector2d latLon = ConvertLatLon((float)df.Columns[latColName][i], (float)df.Columns[lonColName][i]);
            Vector3 unadjustedPos = absMap.GeoToWorldPosition(latLon, true);

            float currentElevMeter = absMap.QueryElevationInMetersAt(latLon);
            float currentElevUnity = absMap.QueryElevationInUnityUnitsAt(latLon) * absMap.transform.localScale.y;
            currentElevMeter = currentElevMeter == 0 ? 0.001f : currentElevMeter; // Cheat if necessary
            float ratio = currentElevUnity / currentElevMeter;
            // Convert kilometers to meters
            float depthInMeters = -(float)df.Columns[depthColName][i] * 1000;
            float adjUnityUnits = ratio * depthInMeters;
            float adjElevUnity = unadjustedPos.y + adjUnityUnits;
            xVals[i] = unadjustedPos.x;
            yVals[i] = adjElevUnity;
            zVals[i] = unadjustedPos.z;
        }

        DataFrame retDf = new DataFrame(xVals, yVals, zVals);
        return new DataObj(retDf);
    }

    protected Vector2d ConvertLatLon(float lat, float lon)
    {
        string locString = lat + ", " + lon;
        return Conversions.StringToLatLon(locString);
    }

    public override DataObj Update(DataObj dO)
    {
        throw new System.NotImplementedException();
    }
}
