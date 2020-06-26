using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetter : MonoBehaviour
{
    [SerializeField]
    GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        MapStore.Instance.map = map.GetComponent<AbstractMap>();
    }
}
