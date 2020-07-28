using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Rework map so that it follows the pattern
// used by scatterplot
public class MapSetter : MonoBehaviour
{
    [SerializeField]
    GameObject map;

    [SerializeField]
    Gradient meshColorGradient;

    [SerializeField]
    Material meshMaterial;
    Vector3 originalMapScale = new Vector3(0.17f, 0.17f, 0.17f);
    Vector3 originalMapPosition = new Vector3(-2f, 1f, 0f);

    protected GameObject dataMesh;
    void Start()
    {
        MapStore.Instance.map = map.GetComponent<AbstractMap>();
        map.transform.localScale = originalMapScale;
        map.transform.position = originalMapPosition;
    }

    /*
    private void Update()
    {
        dataMesh.transform.position = MapStore.Instance.map.transform.position;
        dataMesh.transform.localScale = MapStore.Instance.map.transform.localScale;
    }
    */
}
