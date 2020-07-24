using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraph
{
    void Plot();
    DataObj GetData();
    void SetData(DataObj data);
    List<GameObject> GetDataPoints();
    Vector3 GetMaxDpScale();
    Vector3 GetMinDpScale();
}