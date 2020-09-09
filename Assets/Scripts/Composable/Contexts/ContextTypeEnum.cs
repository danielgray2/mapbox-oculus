using System.Collections.Generic;

public enum ContextTypeEnum
{
    MAP,
    THREEDAXIS
}

public static class ContextDict
{
    public static Dictionary<string, ContextTypeEnum> stringEnumDict = new Dictionary<string, ContextTypeEnum>
    {
        { "map", ContextTypeEnum.MAP },
        { "threeDAxis", ContextTypeEnum.THREEDAXIS }
    };

    public static Dictionary<ContextTypeEnum, string> enumStringDict = new Dictionary<ContextTypeEnum, string>
    {
        { ContextTypeEnum.MAP, "map" },
        { ContextTypeEnum.THREEDAXIS, "threeDAxis" }
    };
}
