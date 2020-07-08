using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IGraph
{
    void Plot();
    List<SData> GetData();
    void SetData(List<SData> data);
}