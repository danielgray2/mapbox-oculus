using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LerpAnimation : MonoBehaviour
{
    [SerializeField]
    public float? dataValue { get; private set; }
    [SerializeField]
    public float speed = 1f;
    public float colorSpeed = 10f;
    float? startTime;
    Vector3 smallestSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    Vector3 largestSize;
    float totalChange;
    bool largestSizeReached = false;
    bool smallestSizeReached = false;
    public SData sData { get; set; }
    DataStore ds;

    void Update()
    {
        if (dataValue != null)
        {
            if (!largestSizeReached)
            {
                HandleGrowingLerp();
            }
            else if (!smallestSizeReached)
            {
                HandleInvisibilityLerp();
            }
            else
            {
                int index = MapStore.Instance.iconGOs.FindIndex(g => g == gameObject);
                if (index != -1)
                {
                    MapStore.Instance.iconGOs.RemoveAt(index);
                }
                Destroy(gameObject);
            }
        }
    }

    public void Setup(SData sData, Vector3 position)
    {
        this.sData = sData;
        this.dataValue = sData.ccmadRatio;
        this.ds = DataStore.Instance;
        transform.localScale = smallestSize;
        transform.position = position;
        float propMax = (float)dataValue / ds.avgCCMad;
        largestSize = new Vector3(propMax, propMax, propMax);
        Debug.Log("PropMax: " + propMax + " sizeMultiplier: " + " largestSize: " + largestSize.x);
        Color color = new Color(propMax, 1 - propMax, 1 - propMax);
        GetComponent<Renderer>().material.color = color;
        totalChange = Vector3.Distance(smallestSize, largestSize);
    }
    void HandleGrowingLerp()
    {
        if (startTime == null)
        {
            startTime = Time.time;
        }

        if (transform.localScale.x < largestSize.x)
        {
            float growthDone = (Time.time - (float)startTime) * speed;
            float fractionOfGrowth = growthDone / totalChange;
            transform.localScale = Vector3.Lerp(transform.localScale, largestSize, fractionOfGrowth);
        }
        else
        {
            largestSizeReached = true;
            startTime = null;
        }
    }

    void HandleShrinkingLerp()
    {
        if (startTime == null)
            startTime = Time.time;

        if (transform.localScale.x > smallestSize.x)
        {
            float shrinkingDone = (Time.time - (float)startTime) * speed;
            float fractionOfShrinking = shrinkingDone / totalChange;
            transform.localScale = Vector3.Lerp(transform.localScale, smallestSize, fractionOfShrinking);
        }
        else
        {
            smallestSizeReached = true;
            startTime = null;
        }
    }

    void HandleInvisibilityLerp()
    {
        if (startTime == null)
            startTime = Time.time;

        Color color = GetComponent<Renderer>().material.color;

        if (color.a > 0.001)
        {
            float changeDone = (Time.time - (float)startTime) * colorSpeed;
            float fractionOfChange = changeDone / (totalChange * 10);
            float newAlpha = Mathf.Lerp(color.a, 0, fractionOfChange);
            GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, newAlpha);
        }
        else
        {
            smallestSizeReached = true;
            startTime = null;
        }
    }
}
