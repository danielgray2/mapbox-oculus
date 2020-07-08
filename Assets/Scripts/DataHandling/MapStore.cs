using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Unity.Map;
using Mapbox.Map;
using Mapbox.Utils;
using System.Linq;

public sealed class MapStore
{
    public AbstractMap map { get; set; }
    public AbstractMap secondaryMap { get; set; }
    public List<GameObject> iconGOs = new List<GameObject>();
    public List<GameObject> secondaryIconGOs = new List<GameObject>();
    // Contains only the iconGOs that are also within the bounds of the
    // selector region.
    public List<GameObject> selectedGOs = new List<GameObject>();
    public float iconSize = 10;

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
