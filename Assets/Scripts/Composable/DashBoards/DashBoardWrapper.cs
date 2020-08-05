using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoardWrapper : MonoBehaviour, IDashBoard
{
    IAbstractDashBoard dB;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DrawDashBoard()
    {
        throw new System.NotImplementedException();
    }

    public int GetCols()
    {
        throw new System.NotImplementedException();
    }

    public int GetRows()
    {
        throw new System.NotImplementedException();
    }

    public void SetCols(int numCols)
    {
        throw new System.NotImplementedException();
    }

    public void SetRows(int numRows)
    {
        throw new System.NotImplementedException();
    }
}
