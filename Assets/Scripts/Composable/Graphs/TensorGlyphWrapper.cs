using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Microsoft.Data.Analysis;

public class TensorGlyphWrapper : IAbstractWrapper
{
    [SerializeField]
    GameObject glyphPrefab;

    bool initialized = false;
    bool plottedOnce = false;
    AddObjectManipulator oM;
    TensorGlyph wrapped;
    List<GameObject> currGlyphList = new List<GameObject>();

    void Start()
    {
        initialized = false;
        plottedOnce = false;
    }

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
        for (int i = 0; i < currGlyphList.Count; i++)
        {
            Destroy(currGlyphList[i]);
        }
        currGlyphList = new List<GameObject>();

        DrawGlyphs(wrapped.CreateGlyphs());

        oM.UpdateObjectManipulator();
    }

    public void Initialize(TensorGlyphModel model)
    {
        if (!initialized)
        {
            wrapped = new TensorGlyph(model);
            this.model = model;
            model.modelUpdateEvent.AddListener(HandleModelUpdate);
            initialized = true;
        }
    }

    public void DrawGlyphs(List<List<float>> glyphVals)
    {
        TensorGlyphModel tGModel = CastToTensorGlyphModel();
        for (int i = 0; i < glyphVals.Count; i++)
        {
            List<float> currGlyph = glyphVals[i];

            // Replace this logic with a filter at some point
            IAbsCompModel compModel = VizUtils.CastToCompModel(model);
            List<float?> latList = ((PrimitiveDataFrameColumn<float>)compModel.dataObj.df.Columns["Latitude"]).ToList();
            float latMed = compModel.dataObj.CalculateMedian(latList);
            float latMax = compModel.dataObj.GetMax("Latitude");
            float latMin = compModel.dataObj.GetMin("Latitude");
            List<float?> lonList = ((PrimitiveDataFrameColumn<float>)compModel.dataObj.df.Columns["Longitude"]).ToList();
            float lonMed = compModel.dataObj.CalculateMedian(lonList);
            float lonMax = compModel.dataObj.GetMax("Longitude");
            float lonMin = compModel.dataObj.GetMin("Longitude");

            GameObject glyph = Instantiate(glyphPrefab, transform);
            glyph.transform.localPosition = new Vector3(wrapped.NormalizeValue(currGlyph[0], latMin, latMax) * 1000, currGlyph[1]/10, wrapped.NormalizeValue(currGlyph[2], lonMin, lonMax) * 1000);
            GlyphDP currDP = glyph.GetComponent<GlyphDP>();

            float oneAxisVal = wrapped.NormalizeValue(currGlyph[3], tGModel.axisOneMax, tGModel.axisOneMin) * 10;
            currDP.xComp.transform.localRotation = Quaternion.Euler(currGlyph[4] * 10, currGlyph[5] * 10, currGlyph[6] * 10);
            currDP.xComp.transform.localScale = new Vector3(oneAxisVal, 1, 1);

            float twoAxisVal = wrapped.NormalizeValue(currGlyph[7], tGModel.axisTwoMax, tGModel.axisTwoMin) * 10;
            currDP.yComp.transform.localRotation = Quaternion.Euler(currGlyph[8] * 10, currGlyph[9] * 10, currGlyph[10] * 10);
            currDP.yComp.transform.localScale = new Vector3(1, twoAxisVal, 1);

            float threeAxisVal = wrapped.NormalizeValue(currGlyph[11], tGModel.axisThreeMax, tGModel.axisThreeMin) * 10;
            currDP.zComp.transform.localRotation = Quaternion.Euler(currGlyph[12] * 10, currGlyph[13] * 10, currGlyph[14] * 10);
            currDP.zComp.transform.localScale = new Vector3(1, 1, threeAxisVal);

            currGlyphList.Add(glyph);
        }
    }

    TensorGlyphModel CastToTensorGlyphModel()
    {
        if (!(model is TensorGlyphModel tensorGlyphModel))
        {
            throw new ArgumentException("Model must be of type TensorGlyphModel");
        }
        return tensorGlyphModel;
    }

    public override void Create()
    {
        Debug.Log("Alrighty");
    }
}
