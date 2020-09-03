using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Microsoft.Data.Analysis;
using System.Collections;

public class TenGlyphWrapper : IAbstractWrapper
{
    [SerializeField]
    GameObject glyphPrefab;

    [SerializeField]
    Gradient gradient;

    //Definitely going to want to remove this later
    public GameObject menu { get; set; }

    bool initialized = false;
    bool plottedOnce = false;
    float currTime = 0;
    float currDepth = 2000;
    float prevDepth = 2000;
    float depthComp = 1000;
    Dictionary<Guid, GameObject> currActiveDict;
    Dictionary<Guid, GameObject> allPoints;
    List<Guid> pointsToDraw;
    List<Guid> pointsToRemove;

    AddObjectManipulator oM;
    TenGlyph wrapped;

    // Start is called before the first frame update
    public void Plot()
    {
        if (initialized && !plottedOnce)
        {
            DrawGlyphs(wrapped.CreateGlyphs());

            oM = this.gameObject.AddComponent<AddObjectManipulator>();
            oM.PlaceObjectManipulator(this.transform);

            plottedOnce = true;
        }
    }

    public override void ReRender()
    {
        // Probably won't even need this
        // code because all data points will
        // be rendered, just not active.
        DrawGlyphs(wrapped.CreateGlyphs());

        oM.UpdateObjectManipulator();
    }

    public void Initialize(TenGlyphModel model)
    {
        if (!initialized)
        {
            wrapped = new TenGlyph(model);
            this.model = model;
            model.modelUpdateEvent.AddListener(HandleModelUpdate);
            initialized = true;
            currActiveDict = new Dictionary<Guid, GameObject>();

            RegisterForDepthUpdates();
            RegisterForTimeUpdates();
        }
    }

    public void DrawGlyphs(List<List<float>> glyphVals)
    {
        // Replace this logic with a filter at some point
        IAbsCompModel compModel = VizUtils.CastToCompModel(model);
        allPoints = new Dictionary<Guid, GameObject>();
        List<float?> latList = ((PrimitiveDataFrameColumn<float>)compModel.dataObj.df.Columns["Latitude"]).ToList();

        float latMed = compModel.dataObj.CalculateMedian(latList);
        float latMax = compModel.dataObj.GetMax("Latitude");
        float latMin = compModel.dataObj.GetMin("Latitude");
        List<float?> lonList = ((PrimitiveDataFrameColumn<float>)compModel.dataObj.df.Columns["Longitude"]).ToList();
        float lonMed = compModel.dataObj.CalculateMedian(lonList);
        float lonMax = compModel.dataObj.GetMax("Longitude");
        float lonMin = compModel.dataObj.GetMin("Longitude");

        TenGlyphModel tGModel = CastToTensorGlyphModel();
        for (int i = 0; i < glyphVals.Count; i++)
        {
            List<float> currGlyph = glyphVals[i];

            GameObject glyph = Instantiate(glyphPrefab, transform);
            glyph.transform.localPosition = new Vector3(wrapped.NormalizeValue(currGlyph[0], latMin, latMax) * 10, -currGlyph[1] / depthComp + 2, wrapped.NormalizeValue(currGlyph[2], lonMin, lonMax) * 10);
            GlyphDP currDP = glyph.GetComponent<GlyphDP>();

            int rotationFactor = 10;
            float scaleFactor = 0.1f;

            float oneAxisVal = wrapped.NormalizeValue(currGlyph[3], tGModel.axisOneMax, tGModel.axisOneMin) * scaleFactor;
            currDP.xComp.transform.localRotation = Quaternion.Euler(currGlyph[4] * rotationFactor, currGlyph[5] * rotationFactor, currGlyph[6] * rotationFactor);
            currDP.xComp.transform.localScale = new Vector3(oneAxisVal, 1, 1);

            float twoAxisVal = wrapped.NormalizeValue(currGlyph[7], tGModel.axisTwoMax, tGModel.axisTwoMin) * scaleFactor;
            currDP.yComp.transform.localRotation = Quaternion.Euler(currGlyph[8] * rotationFactor, currGlyph[9] * rotationFactor, currGlyph[10] * rotationFactor);
            currDP.yComp.transform.localScale = new Vector3(1, twoAxisVal, 1);

            float threeAxisVal = wrapped.NormalizeValue(currGlyph[11], tGModel.axisThreeMax, tGModel.axisThreeMin) * scaleFactor;
            currDP.zComp.transform.localRotation = Quaternion.Euler(currGlyph[12] * rotationFactor, currGlyph[13] * rotationFactor, currGlyph[14] * rotationFactor);
            currDP.zComp.transform.localScale = new Vector3(1, 1, threeAxisVal * 100);

            // This is so that we can do the animation.
            // Look in your notebook for how you were
            // going to refactor this.
            Guid currGuid = Guid.Parse((string)compModel.dataObj.df.Columns[tGModel.guidColName][i]);
            allPoints.Add(currGuid, glyph);
            glyph.gameObject.SetActive(false);
        }
        HandleTimeUpdate(0);
        this.menu.gameObject.SetActive(false);
    }

    TenGlyphModel CastToTensorGlyphModel()
    {
        if (!(model is TenGlyphModel tensorGlyphModel))
        {
            throw new ArgumentException("Model must be of type TensorGlyphModel");
        }
        return tensorGlyphModel;
    }

    public override void Create()
    {
        Debug.Log("Alrighty");
    }

    public void HandleDepthUpdate(float value)
    {
        prevDepth = currDepth;
        currDepth = value;
        UpdateDf();
        CalcDiff();

        float distance = ((currDepth - prevDepth) / depthComp) * transform.localScale.y;
        Vector3 endPoint = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z);
        StartCoroutine(DistanceLerp(transform.position, endPoint));
        StartCoroutine(AlphaLerp(0, 1, pointsToDraw, pointsToRemove));
    }

    public void HandleTimeUpdate(float value)
    {
        currTime = value;
        UpdateDf();
        CalcDiff();
        StartCoroutine(AlphaLerp(0, 1, pointsToDraw, pointsToRemove));
    }

    public void CalcDiff()
    {
        pointsToDraw = new List<Guid>();
        pointsToRemove = new List<Guid>();
        CalcAdd();
        CalcRemove();
    }

    public void CalcAdd()
    {
        TenGlyphModel tGModel = CastToTensorGlyphModel();
        for (int i = 0; i < tGModel.dataObj.df.Rows.Count; i++)
        {
            // Don't update currRenderedDict here. Do that when the point is actually
            // rendered
            Guid currGuid = Guid.Parse((string)tGModel.dataObj.df.Columns[tGModel.guidColName][i]);
            if (!currActiveDict.ContainsKey(currGuid))
            {
                pointsToDraw.Add(currGuid);
            }
        }
    }

    public void CalcRemove()
    {
        TenGlyphModel tGModel = CastToTensorGlyphModel();
        DataFrameColumn dfCol = tGModel.dataObj.df.Columns[tGModel.guidColName];
        List<Guid> dfGuids = ((StringDataFrameColumn)tGModel.dataObj.df.Columns[tGModel.guidColName]).ToList().Select(s => Guid.Parse(s)).ToList();
        List<Guid> renderedGuids = currActiveDict.Keys.ToList();
        for (int i = 0; i < currActiveDict.Count; i++)
        {
            if (!dfGuids.Contains(renderedGuids[i]))
            {
                pointsToRemove.Add(renderedGuids[i]);
            }
        }
    }

    public void UpdateDf()
    {
        IAbsCompModel compModel = VizUtils.CastToCompModel(this.model);
        PrimitiveDataFrameColumn<bool> depthFilter = compModel.origDo.df.Columns["Depth"].ElementwiseEquals(currDepth);
        PrimitiveDataFrameColumn<bool> timeFilter = compModel.origDo.df.Columns["dTime"].ElementwiseEquals(currTime);
        int numTrue = timeFilter.Where(s => s == true).ToList().Count();
        PrimitiveDataFrameColumn<bool> combinedFilter = (PrimitiveDataFrameColumn<bool>)depthFilter.And(timeFilter);
        DataObj newDO = new DataObj(compModel.origDo.df.Filter(combinedFilter));
        compModel.dataObj = newDO;
    }

    public void RegisterForDepthUpdates()
    {
        DepthTimeSelector selector = this.menu.GetComponent<DepthTimeSelector>();
        selector.depthUpdated.AddListener(HandleDepthUpdate);
    }

    public void RegisterForTimeUpdates()
    {
        DepthTimeSelector selector = this.menu.GetComponent<DepthTimeSelector>();
        selector.timeUpdated.AddListener(HandleTimeUpdate);
    }

    IEnumerator DistanceLerp(Vector3 start, Vector3 stop)
    {
        float totalFrames = 45;
        float elapsedFrames = 0;

        while(elapsedFrames < totalFrames)
        {
            float ratio = elapsedFrames / totalFrames;
            transform.localPosition = Vector3.Lerp(start, stop, ratio);
            elapsedFrames ++;
            yield return new WaitForSeconds(0.001f);
        }
        yield return null;
    }

    IEnumerator AlphaLerp(float fadedVal, float fullVal, List<Guid> newDPs, List<Guid> oldDPs)
    {
        float totalFrames = 45;
        float elapsedFrames = 0;

        foreach(Guid newGuid in newDPs)
        {
            allPoints[newGuid].gameObject.SetActive(true);
        }

        while (elapsedFrames < totalFrames)
        {
            float ratio = elapsedFrames / totalFrames;
            ChangeAlpha(fullVal, ratio, newDPs);
            ChangeAlpha(fadedVal, ratio, oldDPs);
            elapsedFrames++;
            yield return new WaitForSeconds(0.001f);
        }
        UpdateActive(newDPs, oldDPs);
        yield return null;
    }

    protected void ChangeAlpha(float alpha, float ratio, List<Guid> dataPoints)
    {
        // Iterate through list to remove and remove all
        // of them.
        for(int i = 0; i < dataPoints.Count; i++)
        {
            Guid currGuid = dataPoints[i];
            GameObject currGlyph = allPoints[currGuid];
            GlyphDP currObj = currGlyph.GetComponent<GlyphDP>();
            GameObject currDP = currObj.dp;
            Color currColor = currDP.GetComponent<Renderer>().material.color;
            Color newColor = Color.Lerp(currColor, new Color(currColor.r, currColor.g, currColor.b, alpha), ratio);
            currDP.GetComponent<Renderer>().material.color = newColor;
        }
        
    }

    protected void UpdateActive(List<Guid> newList, List<Guid> oldList)
    {
        foreach(Guid oldGuid in oldList)
        {
            allPoints[oldGuid].gameObject.SetActive(false);
            currActiveDict.Remove(oldGuid);
        }

        foreach(Guid newGuid in newList)
        {
            currActiveDict.Add(newGuid, allPoints[newGuid]);
        }
    }
}
