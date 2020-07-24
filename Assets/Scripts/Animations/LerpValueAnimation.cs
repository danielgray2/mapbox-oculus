using UnityEngine;
using System;
using System.Collections.Generic;

// So basically, the idea here is that what we need to do
// to setup an animation for a characteristic is to add
// that characteristic to the IAnimation enum and dictionary,
// add it to the case statement, and call a lerp function.
public class LerpValueAnimation : IAnimation
{

    public float timeMultiplier { get; set; } = 1;
    float delta = 0.001f;
    float? startTime;
    Vector3 minVal = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    Vector3 maxVal = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    AnimateAttrs attrToAnimate;

    public LerpValueAnimation(AnimateAttrs attrToAnimate, EndBehav endBehav, Vector3 minVal, Vector3 maxVal) : base(endBehav)
    {
        this.attrToAnimate = attrToAnimate;
        this.endBehav = endBehav;
        this.minVal = minVal;
        this.maxVal = maxVal;
    }

    public override void Animate()
    {
        switch (attrToAnimate)
        {
            case AnimateAttrs.POS:
                HandlePosLerp(gO.transform.localPosition, this.minVal, this.maxVal);
                break;
            case AnimateAttrs.SCALE:
                HandleScaleLerp(gO.transform.localScale, this.minVal, this.maxVal);
                break;
            default:
                throw new ArgumentException("The attribute you want to animate is not valid");
        }
    }

    public void HandlePosLerp(Vector3 currVal, Vector3 minVal, Vector3 maxVal)
    {
        if (startTime == null)
        {
            gO.transform.localPosition = minVal;
            currVal = minVal;
        }

        if (!CheckArrival(currVal, maxVal))
        {
            gO.transform.localPosition = HandleLerp(currVal, minVal, maxVal);
        }
        else
        {
            HandleEnd();
        }
    }

    public void HandleScaleLerp(Vector3 currVal, Vector3 minVal, Vector3 maxVal)
    {
        if (startTime == null)
        {
            gO.transform.localScale = minVal;
            currVal = minVal;
        }

        if (!CheckArrival(currVal, maxVal))
        {
            gO.transform.localScale = HandleLerp(currVal, minVal, maxVal);
        }
        else
        {
            HandleEnd();
        }
    }

    Vector3 HandleLerp(Vector3 currVal, Vector3 startVal, Vector3 endVal)
    {
        if (startTime == null)
        {
            startTime = Time.time;
        }

        float totalChange = Vector3.Distance(startVal, endVal);
        float changeDone = (Time.time - (float)startTime) * timeMultiplier;
        float fractionDone = changeDone / totalChange;
        Vector3 val = Vector3.Lerp(currVal, endVal, fractionDone);
        return val;
    }

    public bool CheckArrival(Vector3 currVal, Vector3 endVal)
    {
        return Vector3.Distance(currVal, endVal) < delta;
    }

    public override void HandleDestroy()
    {
        UnityEngine.Object.Destroy(gO);
    }

    public override void HandleStop()
    {
        gO.GetComponent<DataPoint>().RemoveAnimation(this);
    }

    public override void HandleRepeatBeginning() 
    {
        startTime = null;
        switch (attrToAnimate)
        {
            case AnimateAttrs.SCALE:
                gO.transform.localScale = minVal;
                break;
            case AnimateAttrs.POS:
                gO.transform.position = minVal;
                break;
            default:
                throw new ArgumentException("The attribute you want to animate is not valid");
        }
    }

    public override void HandleReverse()
    {
        startTime = null;
        Vector3 curr = minVal;
        minVal = maxVal;
        maxVal = curr;
    }

    public override void HandleCallAnother()
    { 
        if(nextAnim != null)
        {
            nextAnim.Activate(gO);
            gO.GetComponent<DataPoint>().RemoveAnimation(this);
        }
        else
        {
            throw new ArgumentException("The next animation must be set before you can call it");
        }
    }
}