using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

public class MapSetterNoMesh : MonoBehaviour
{
    [SerializeField]
    GameObject map;

    Vector3 originalMapScale = new Vector3(0.17f, 0.17f, 0.17f);
    Vector3 originalMapPosition = new Vector3(2f, 1f, 0f);

    protected GameObject layerParent;
    void Start()
    {
        MapStore.Instance.secondaryMap = map.GetComponent<AbstractMap>();
        map.transform.localScale = originalMapScale;
        map.transform.position = originalMapPosition;
    }
}
