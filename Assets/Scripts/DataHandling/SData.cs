using CsvHelper.Configuration.Attributes;
using Mapbox.Json;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SData
{
    public string rawDate { get; set; }
    public string rawTime { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public float depth { get; set; }
    public float unkwn { get; set; }
    public int eventId { get; set; }
    public int templateId { get; set; }
    public float ccmadRatio { get; set; }
    public int relocationFlag { get; set; }
    private string [] dateFormats = { "yyyy-MM-dd;HH:mm:ss.fff", "yyyy-MM-dd;HH:mm" };
    public DateTime dateTime { get; private set; }
    public Vector2d latLon { get; private set; }

    public void SetDateTime()
    {
        if (this.rawDate != null && this.rawTime != null)
        {
            DateTime parsedDate;
            string dateString = rawDate + ";" + rawTime;
            if (DateTime.TryParseExact(dateString, dateFormats, null,
                            System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                            System.Globalization.DateTimeStyles.AdjustToUniversal,
                            out parsedDate))
            {
                dateTime = parsedDate;
            }
            else
            {
                throw new System.ArgumentException("Could not parse the given date string to a DateTime variable: " + dateString);
            }
        }
    }

    public void SetLatLon()
    {
        string stringLat = lat.ToString();
        string stringLon = lon.ToString();

        string locString = lat + ", " + lon;
        latLon = Conversions.StringToLatLon(locString);
    }
}
