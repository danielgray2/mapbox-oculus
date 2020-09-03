using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GraphTypeEnum
{
    SCATTERPLOT,
    HISTOGRAM,
    TENSOR_GLYPH
}

public static class GraphDict
{
    public static Dictionary<string, GraphTypeEnum> stringEnumDict = new Dictionary<string, GraphTypeEnum>
    {
        { "scatterplot", GraphTypeEnum.SCATTERPLOT },
        { "histogram", GraphTypeEnum.HISTOGRAM },
        { "tensorGlyph",  GraphTypeEnum.TENSOR_GLYPH},
    };

    public static Dictionary<GraphTypeEnum, string> enumStringDict = new Dictionary<GraphTypeEnum, string>
    {
        { GraphTypeEnum.SCATTERPLOT, "scatterplot" },
        { GraphTypeEnum.HISTOGRAM, "histogram" },
        { GraphTypeEnum.TENSOR_GLYPH, "tensorGlyph" },
    };
}