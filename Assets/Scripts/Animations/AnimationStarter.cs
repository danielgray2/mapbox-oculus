using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStarter : MonoBehaviour
{
    [SerializeField]
    Gradient sphereColorGradient;
    
    [SerializeField]
    GameObject dataPointPrefab;

    [SerializeField]
    GameObject secondaryDataPointPrefab;

    [SerializeField]
    GameObject scrubbingMenuGo;

    protected bool animationStartedOnce = false;
    protected bool currentlyScrolling = false;
    protected GameObject plotAnimation;
    protected GameObject secondaryPlotAnimation;
    protected TimeSeries ts;
    protected TimeSeriesDelete secondaryTs;

    public void HandleClick()
    {
        if (currentlyScrolling)
        {
            currentlyScrolling = false;
            EndScrolling();
            ActivateAnimation();
        }
        else
        {
            currentlyScrolling = true;
            EndAnimation();
            ActivateScrolling();
        }
    }
    public void ActivateAnimation()
    {
        ts.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
        secondaryTs.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
    }

    public void EndAnimation()
    {
        ts.EndIndexedAnimation();
        secondaryTs.EndIndexedAnimation();
    }

    public void ActivateScrolling()
    {
        ts.BeginScrubbedAnimation(scrubbingMenuGo, sphereColorGradient);
        secondaryTs.BeginScrubbedAnimation(scrubbingMenuGo, sphereColorGradient);
    }

    public void EndScrolling()
    {
        ts.EndScrubbedAnimation();
        secondaryTs.EndScrubbedAnimation();
    }

    public void Update()
    {
        if(!animationStartedOnce && MapStore.Instance.map != null)
        {
            animationStartedOnce = true;
            plotAnimation = new GameObject();
            plotAnimation.transform.position = Vector3.zero;
            ts = plotAnimation.AddComponent<TimeSeries>();
            ts.dataPointPrefab = dataPointPrefab;

            secondaryPlotAnimation = new GameObject();
            secondaryPlotAnimation.transform.position = Vector3.zero;
            secondaryTs = secondaryPlotAnimation.AddComponent<TimeSeriesDelete>();
            secondaryTs.dataPointPrefab = secondaryDataPointPrefab;

            //ts.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
            //secondaryTs.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
            currentlyScrolling = false;
        }
    }
}
