using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;

public class CSVToObjectMap : ClassMap<SData>
{
    public CSVToObjectMap()
    {
        Map(m => m.rawDate).Name("date");
        Map(m => m.rawTime).Name("time");
        Map(m => m.lat).Name("lat");
        Map(m => m.lon).Name("lon");
        Map(m => m.depth).Name("depth");
        Map(m => m.unkwn).Name("UNKWN");
        Map(m => m.eventId).Name("event_id");
        Map(m => m.templateId).Name("template_id");
        Map(m => m.ccmadRatio).Name("CCMAD_ratio");
        Map(m => m.relocationFlag).Name("relocationFlag");
    }
}
