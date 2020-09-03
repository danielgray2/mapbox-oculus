using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorSettings : MonoBehaviour
{
    [SerializeField]
    GameObject selectorShapePrefab;
    [SerializeField]
    GameObject captureBtn;

    protected bool selectorActive = false;
    protected GameObject selectorShapeGo;

    private void Start()
    {
        selectorShapeGo = Instantiate(selectorShapePrefab);
        captureBtn.GetComponent<DataCapturer>().selectorShape = selectorShapeGo;

        selectorShapeGo.SetActive(false);
        captureBtn.SetActive(false);
    }
    public void HandleSelector()
    {
        if (selectorActive)
        {
            selectorActive = false;
            selectorShapeGo.SetActive(false);
            captureBtn.SetActive(false);
        }
        else
        {
            selectorActive = true;
            selectorShapeGo.transform.position = CalculatePosition();
            selectorShapeGo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            captureBtn.SetActive(true);
            selectorShapeGo.SetActive(true);
        }
    }

    protected Vector3 CalculatePosition()
    {
        Vector3 pos = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;
        float distance = 1.5f;

        return pos + forward * distance;
    }
}
