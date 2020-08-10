using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ContextTypeEnum
{
    MAP
}

public static class ContextDict
{
    public static Dictionary<string, ContextTypeEnum> stringEnumDict = new Dictionary<string, ContextTypeEnum>
    {
        { "map", ContextTypeEnum.MAP }
    };

    public static Dictionary<ContextTypeEnum, string> enumStringDict = new Dictionary<ContextTypeEnum, string>
    {
        { ContextTypeEnum.MAP, "map" }
    };
}
