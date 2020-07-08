using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GraphStore
{
    private static GraphStore instance = null;
    private static readonly object padlock = new object();
    public List<IGraph> graphList = new List<IGraph>();

    GraphStore() { }

    public static GraphStore Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new GraphStore();
                }
                return instance;
            }
        }
    }
}
