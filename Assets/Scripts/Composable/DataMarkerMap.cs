using System.Collections.Generic;
using UnityEngine;

public class DataMarkerMap : MonoBehaviour
{
    [SerializeField]
    GameObject pointPrefab;

    public Dictionary<MarkerType, GameObject> dict { get; protected set; }

    void Awake()
    {
        dict = new Dictionary<MarkerType, GameObject>();
        dict.Add(MarkerType.NONE, new GameObject());
        dict.Add(MarkerType.POINT, pointPrefab);
    }
}
