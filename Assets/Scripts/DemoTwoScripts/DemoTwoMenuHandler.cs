using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTwoMenuHandler : MonoBehaviour
{

    [SerializeField]
    GameObject selectorSettings;

    [SerializeField]
    GameObject depthAndTimeSettings;

    [SerializeField]
    GameObject timeSettings;

    bool first = true;

    public void Update()
    {
        if (first)
        {
            NavToSelectorSettings();
            first = false;
        }
    }

    public void NavToSelectorSettings()
    {
        selectorSettings.SetActive(true);

        depthAndTimeSettings.SetActive(false);
        timeSettings.SetActive(false);
    }

    public void NavToDepthAndTimeSettings() 
    {
        depthAndTimeSettings.SetActive(true);

        selectorSettings.SetActive(false);
        timeSettings.SetActive(false);
    }

    public void NavToTimeSettings() 
    {
        timeSettings.SetActive(true);

        depthAndTimeSettings.SetActive(false);
        selectorSettings.SetActive(false);
    }
}
