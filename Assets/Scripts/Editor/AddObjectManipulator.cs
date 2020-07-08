using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectManipulator : MonoBehaviour
{


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
