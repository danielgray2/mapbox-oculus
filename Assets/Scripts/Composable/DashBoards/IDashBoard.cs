using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashBoard
{
    int numRows { get; set; }
    int numCols { get; set; }
    void DrawDashBoard();
}
