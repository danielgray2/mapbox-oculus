using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashBoard
{
    public IOptions options { get; set; }
    public int numRows { get; set; }
    public int numCols { get; set; }

    void DrawDashBoard();
}
