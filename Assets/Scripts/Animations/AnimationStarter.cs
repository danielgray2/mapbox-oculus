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
    GameObject scrubbingMenuGo;

    protected bool animationStartedOnce = false;
    protected bool currentlyScrolling = false;
    protected GameObject plotAnimation;
    protected TimeSeries ts;

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
    }

    public void EndAnimation()
    {
        ts.EndIndexedAnimation();
    }

    public void ActivateScrolling()
    {
        ts.BeginScrubbedAnimation(scrubbingMenuGo, sphereColorGradient);
    }

    public void EndScrolling()
    {
        ts.EndScrubbedAnimation();
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
            //ts.BeginIndexedAnimation(0, 70, 1f, sphereColorGradient);
            currentlyScrolling = false;
        }
    }
}
