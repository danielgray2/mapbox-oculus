using UnityEngine;
using System;

// So basically, the idea here is that what we need to do
// to setup an animation for a characteristic is to add
// that characteristic to the IAnimation enum and dictionary,
// add it to the case statement, and call a lerp function.
public class LerpColorAnimation : IAnimation
{
    /*
    //[SerializeField]
    public float? dataValue { get; private set; }
    //[SerializeField]
    
    float colorSpeed = 0.1f;
    Vector3 smallestSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    Vector3 largestSize;
    float delta = 0.001f;
    public bool experiencedChangeOnce = false;
    DataStore ds;
    */
    public float timeMultiplier { get; set; } = 1f;
    float delta = 0.001f;
    float? startTime;
    Color minColor = new Vector4(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    Color maxColor = new Vector4(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

    public LerpColorAnimation(EndBehav endBehav, Color minColor, Color maxColor) : base(endBehav)
    {
        this.endBehav = endBehav;
        this.minColor = minColor;
        this.maxColor = maxColor;
    }

    public override void Animate()
    {
        HandleColorLerp(gO.GetComponent<Renderer>().material.color, this.minColor, this.maxColor);
    }

    public void HandleColorLerp(Color currColor, Color minColor, Color maxColor)
    {
        if(startTime == null)
        {
            gO.GetComponent<Renderer>().material.color = minColor;
        }

        if (!CheckArrival(currColor, maxColor))
        {
            gO.GetComponent<Renderer>().material.color = HandleLerp(currColor, minColor, maxColor);
        }
        else
        {
            HandleEnd();
        }
    }

    Color HandleLerp(Color currVal, Color startVal, Color endVal)
    {
        if (startTime == null)
        {
            startTime = Time.time;
        }

        float totalChange = Vector4.Distance(endVal, startVal);
        float changeDone = (Time.time - (float)startTime) * timeMultiplier;
        float fractionDone = changeDone / totalChange;
        Color returnColor = Color.Lerp(currVal, endVal, fractionDone);
        return returnColor;
    }

    public bool CheckArrival(Color currVal, Color endVal)
    {
        return Vector4.Distance(currVal, endVal) < delta;
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
        gO.GetComponent<Renderer>().material.color = minColor;
    }

    public override void HandleReverse()
    {
        startTime = null;
        Color curr = minColor;
        minColor = maxColor;
        maxColor = curr;
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

    /*
    public void UpdateLargestSize(float timeMultiplier)
    {
        this.timeMultiplier = timeMultiplier;
        float propMax = (float)dataValue / ds.avgCCMad;
        largestSize = new Vector3(propMax, propMax, propMax) * sizeMultiplier;
        totalChange = Vector3.Distance(smallestSize, largestSize);
    }
    */
}