using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeUpdated : UnityEvent<float> { }
public class DepthUpdated : UnityEvent<float> { }

public class DepthTimeSelector : MonoBehaviour
{
    [SerializeField]
    GameObject depthSliderGo;

    [SerializeField]
    GameObject timeSliderGo;

    [SerializeField]
    GameObject timeDisplayGo;

    [SerializeField]
    GameObject depthDisplayGo;

    DataObj origdO;
    DataObj currdO;
    bool initialized = false;
    bool firstRun = true;

    Slider depthSliderObj;
    Slider timeSliderObj;
    TextMeshProUGUI timeDisplayObj;
    TextMeshProUGUI depthDisplayObj;

    public TimeUpdated timeUpdated;
    public DepthUpdated depthUpdated;

    // Start is called before the first frame update
    private void Start()
    {
        // If there is an error, it is probably because we are importing the
        // wrong slider.
        if(timeUpdated == null)
        {
            timeUpdated = new TimeUpdated();
        }

        if (depthUpdated == null)
        {
            depthUpdated = new DepthUpdated();
        }

        depthSliderObj = depthSliderGo.GetComponent<Slider>();
        timeSliderObj = timeSliderGo.GetComponent<Slider>();
        depthDisplayObj = depthDisplayGo.GetComponent<TextMeshProUGUI>();
        timeDisplayObj = timeDisplayGo.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (initialized && firstRun)
        {
            HandleDepthUpdate(depthSliderObj.value);
            HandleTimeUpdate(timeSliderObj.value);

            firstRun = false;
        }
    }

    public void Initialize(DataObj dO)
    {
        this.origdO = dO;
        this.currdO = new DataObj(dO.df.Clone());
        initialized = true;
    }

    public void HandleDepthUpdate(float value)
    {
        float newVal = (value + 1) * 1000;
        depthDisplayObj.text = newVal.ToString();
        depthUpdated.Invoke(newVal);
    }

    public void HandleTimeUpdate(float value)
    {
        Debug.Log("Here is the new time: " + timeDisplayObj);
        timeDisplayObj.text = value.ToString();
        timeUpdated.Invoke(value);
    }
}
