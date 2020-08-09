using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GraphTypeEnum
{
    SCATTERPLOT,
    HISTOGRAM
}

public static class GraphDict
{
    public static Dictionary<string, GraphTypeEnum> stringEnumDict = new Dictionary<string, GraphTypeEnum>
    {
        { "scatterplot", GraphTypeEnum.SCATTERPLOT },
        { "histogram", GraphTypeEnum.HISTOGRAM }
    };

    public static Dictionary<GraphTypeEnum, string> enumStringDict = new Dictionary<GraphTypeEnum, string>
    {
        { GraphTypeEnum.SCATTERPLOT, "scatterplot" },
        { GraphTypeEnum.HISTOGRAM, "histogram" }
    };
}