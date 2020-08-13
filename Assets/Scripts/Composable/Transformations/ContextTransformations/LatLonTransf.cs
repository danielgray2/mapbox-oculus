using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using UnityEngine;

public class LatLonTransf : IAbsTransf
{
    public string xColName { get; protected set; } = "XVals";
    public string yColName { get; protected set; } = "YVals";
    public string zColName { get; protected set; } = "ZVals";
    public string latColName { get; set; }
    public string lonColName { get; set; }
    public AbstractMap absMap { get; set; }

    public LatLonTransf(string latColName, string lonColName, AbstractMap absMap)
    {
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

        for (int i = 0; i < df.Rows.Count; i++)
        {
            Vector2d latLon = ConvertLatLon((float)df.Columns[latColName][i], (float)df.Columns[lonColName][i]);
            Vector3 unadjustedPos = absMap.GeoToWorldPosition(latLon, true);

            xVals[i] = unadjustedPos.x;
            yVals[i] = unadjustedPos.y;
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
