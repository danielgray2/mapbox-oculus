using UnityEngine;

public class LerpAnimationDelete : MonoBehaviour
{
    //[SerializeField]
    public float? dataValue { get; private set; }
    //[SerializeField]
    public float sizeMultiplier { get; set; }
    float colorSpeed = 0.1f;
    float? startTime;
    Vector3 smallestSize = new Vector3(0.0001f, 0.0001f, 0.0001f);
    Vector3 largestSize;
    float totalChange;
    float totalColorChange;
    float zeroApproximate = 0.001f;
    bool animateDestruct;
    bool largestSizeReached = false;
    bool smallestSizeReached = false;
    public bool experiencedChangeOnce = false;
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
            else if (!smallestSizeReached && animateDestruct)
            {
                HandleInvisibilityLerp();
            }
            else if (animateDestruct)
            {
                int index = MapStore.Instance.secondaryIconGOs.FindIndex(g => g == gameObject);
                if (index != -1)
                {
                    MapStore.Instance.secondaryIconGOs.RemoveAt(index);
                }
                Destroy(gameObject);
            }
        }
    }

    public void Setup(SData sData, bool animateDestruct, Gradient gradient)
    {
        this.sData = sData;
        this.sizeMultiplier = MapStore.Instance.secondaryMap.UnityTileSize * MapStore.Instance.secondaryMap.transform.localScale.x * MapStore.Instance.iconSize;
        this.dataValue = sData.ccmadRatio;
        this.ds = DataStore.Instance;
        this.animateDestruct = animateDestruct;
        transform.localScale = smallestSize;
        transform.position = Vector3.zero;
        float propMax = Mathf.InverseLerp(DataStore.Instance.minCCMad, DataStore.Instance.maxCCMad, (float)dataValue);
        largestSize = new Vector3(propMax, propMax, propMax) * sizeMultiplier;
        Color color = gradient.Evaluate(propMax);
        GetComponent<Renderer>().material.color = color;
        totalChange = Vector3.Distance(smallestSize, largestSize);
        totalColorChange = color.a;
    }

    void HandleGrowingLerp()
    {
        if (startTime == null)
        {
            startTime = Time.time;
        }

        if (transform.localScale.x < largestSize.x - zeroApproximate)
        {
            float growthDone = (Time.time - (float)startTime) * sizeMultiplier;
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
            float shrinkingDone = (Time.time - (float)startTime) * sizeMultiplier;
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

        if (color.a > zeroApproximate)
        {
            float changeDone = (Time.time - (float)startTime) * colorSpeed;
            float fractionOfChange = changeDone / (totalColorChange * 10);
            float newAlpha = Mathf.Lerp(color.a, 0, fractionOfChange);
            GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, newAlpha);
        }
        else
        {
            smallestSizeReached = true;
            startTime = null;
        }
    }

    public void UpdateLargestSize(float sizeMultiplier)
    {
        this.sizeMultiplier = sizeMultiplier;
        float propMax = (float)dataValue / ds.avgCCMad;
        largestSize = new Vector3(propMax, propMax, propMax) * sizeMultiplier;
        totalChange = Vector3.Distance(smallestSize, largestSize);
    }
}

