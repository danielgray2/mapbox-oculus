using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AddObjectManipulator : MonoBehaviour
{
    public void PlaceObjectManipulator(Transform root)
    {
        List<Renderer> allRends = GetAllChildrenRenderers(root);
        Bounds tempBounds = new Bounds();
        foreach(Renderer rend in allRends)
        {
            tempBounds.Encapsulate(rend.bounds);
        }
        BoxCollider bc = gameObject.AddComponent<BoxCollider>();
        bc.center = tempBounds.center;
        bc.size = tempBounds.extents;

        root.gameObject.AddComponent<ObjectManipulator>();
        BoundingBox box = gameObject.AddComponent<BoundingBox>();
        box.ShowWireFrame = false;
        box.ShowScaleHandles = false;
        box.ShowRotationHandleForX = false;
        box.ShowRotationHandleForY = false;
        box.ShowRotationHandleForZ = false;
    }

    protected List<Renderer> GetAllChildrenRenderers(Transform root)
    {
        List<Renderer> rendererList = new List<Renderer>();
        foreach (Renderer rend in root.GetComponentsInChildren<Renderer>())
        {
            if (rend != root.GetComponent<Renderer>())
            {
                rendererList.Add(rend);
            }
        }
        return rendererList;
    }

    protected List<Transform> GetAllChildrenTransforms(Transform parent)
    {
        List<Transform> returnList = new List<Transform>();
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child != parent)
            {
                returnList.Add(child);
            }
        }
        return returnList;
    }
}
