using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class ToggleSelector : MonoBehaviour
{
    [SerializeField]
    GameObject captureBtnPrefab;
    [SerializeField]
    GameObject selectorShapePrefab;

    protected bool selectorActive = false;
    protected GameObject selectorShapeGo;
    protected GameObject captureBtnGo;

    private void Start()
    {
        selectorShapeGo = Instantiate(selectorShapePrefab);
        captureBtnGo = Instantiate(captureBtnPrefab);

        captureBtnGo.GetComponent<DataCapturer>().selectorShape = selectorShapeGo;

        selectorShapeGo.SetActive(false);
        captureBtnGo.SetActive(false);
    }
    public void HandleSelector()
    {
        if (selectorActive)
        {
            selectorActive = false;
            selectorShapeGo.SetActive(false);
            captureBtnGo.SetActive(false);
        }
        else
        {
            selectorActive = true;
            selectorShapeGo.transform.position = CalculatePosition();
            selectorShapeGo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            captureBtnGo.SetActive(true);
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
