using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper.Configuration;

public class CSVToVData : ClassMap<VData>
{
    public CSVToVData()
    {
        Map(m => m.easting).Name("x(m)");
        Map(m => m.northing).Name("y(m)");
        Map(m => m.depth).Name("depth(km)");
        Map(m => m.vPvS).Name("Vp/Vs");
        Map(m => m.lat).Name("lat");
        Map(m => m.lon).Name("lon");
    }
}
