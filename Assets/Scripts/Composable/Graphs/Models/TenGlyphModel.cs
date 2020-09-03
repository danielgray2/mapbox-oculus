using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenGlyphModel : IAbsGraphModel
{
    public string xPosColName { get; set; }
    public string yPosColName { get; set; }
    public string zPosColName { get; set; }
    public string axisOneName { get; set; }
    public string axisTwoName { get; set; }
    public string axisThreeName { get; set; }
    public string oneCompAxisOneName { get; set; }
    public string oneCompAxisTwoName { get; set; }
    public string oneCompAxisThreeName { get; set; }
    public string twoCompAxisOneName { get; set; }
    public string twoCompAxisTwoName { get; set; }
    public string twoCompAxisThreeName { get; set; }
    public string threeCompAxisOneName { get; set; }
    public string threeCompAxisTwoName { get; set; }
    public string threeCompAxisThreeName { get; set; }
    public float axisOneMax { get; set; }
    public float axisTwoMax { get; set; }
    public float axisThreeMax { get; set; }
    public float axisOneMin { get; set; }
    public float axisTwoMin { get; set; }
    public float axisThreeMin { get; set; }
    public string guidColName { get; set; }
}
