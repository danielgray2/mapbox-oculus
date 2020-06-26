using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Unity.Map;

public sealed class MapStore
{
    public AbstractMap map { get; set; }
    public List<GameObject> iconGOs = new List<GameObject>();

    private static MapStore instance = null;
    private static readonly object padlock = new object();
    MapStore(){ }

    public static MapStore Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MapStore();
                }
                return instance;
            }
        }
    }
}
