using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSlice
{
    public List<SData> dataList { get; set; }
    public int lastIndex { get; set; }

    public DataSlice(List<SData> dataList, int lastIndex)
    {
        this.dataList = dataList;
        this.lastIndex = lastIndex;
    }
}
