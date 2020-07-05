using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureData : MonoBehaviour
{
    public GameObject selectorShape { get; set; }

    public void HandleDataSelection()
    {
        // Be aware that bounds.Contains() uses an exis aligned
        // bounding box, so any rotations on the selector region
        // will be ignored.
        if(selectorShape == null) { return; }
        MapStore.Instance.selectedGOs = new List<GameObject>();
        foreach(GameObject go in MapStore.Instance.iconGOs)
        {
            if (selectorShape.GetComponent<BoxCollider>().bounds.Contains(go.transform.position))
            {
                MapStore.Instance.selectedGOs.Add(go);
            }
        }
        Debug.Log("Here is the count: " + MapStore.Instance.selectedGOs.Count);
    }
}
