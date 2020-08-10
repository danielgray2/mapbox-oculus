﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractDashBoard : IDashBoard
{
    public int numRows { get; set; }
    public int numCols { get; set; }

    public void DrawDashBoard()
    {
        throw new System.NotImplementedException();
    }
}