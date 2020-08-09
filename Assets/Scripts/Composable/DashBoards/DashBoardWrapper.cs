using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoardWrapper : MonoBehaviour, IDashBoard
{
    IAbstractDashBoard dB;

    public int numRows { get; set; }
    public int numCols { get; set; }

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
}
