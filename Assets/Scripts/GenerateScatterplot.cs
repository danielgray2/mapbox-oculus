using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal.Internal;

public class GenerateScatterplot : MonoBehaviour
{
    [SerializeField]
    GameObject scatterplotPrefab;
    /*
    DataFrame df;
    DataObj dO;

    private void Start()
    {
        SetupData();
        Vector3 plotPosition = Camera.main.transform.right * 2;
        plotPosition.y = 1;
        GameObject scatterplotGo = Instantiate(scatterplotPrefab, plotPosition, Quaternion.identity);
        ScatterBox sPObj = scatterplotGo.GetComponentInChildren<ScatterBox>();
        sPObj.InitializeScatterplot(dO, "lat", "lon", "ccmadRatio");
        foreach(GameObject gO in sPObj.GetDataPoints())
        {
            DataPoint dp = gO.GetComponent<DataPoint>();
            IAnimation anim = new LerpValueAnimation(IAnimation.AnimateAttrs.SCALE, IAnimation.EndBehav.REVERSE, dp.origLocalScale * 0.1f, dp.origLocalScale);
            anim.Activate(gO);
        }
        AddObjectManipulator oM = scatterplotGo.AddComponent<AddObjectManipulator>();
        oM.PlaceObjectManipulator(scatterplotGo.transform);
    }

    private void SetupData()
    {
        PrimitiveDataFrameColumn<float> ccmadCol = new PrimitiveDataFrameColumn<float>("ccmadRatio", 6);
        PrimitiveDataFrameColumn<float> latCol = new PrimitiveDataFrameColumn<float>("lat", 6);
        PrimitiveDataFrameColumn <float> lonCol = new PrimitiveDataFrameColumn<float>("lon", 6);

        ccmadCol[0] = 0.5f;
        latCol[0] = 45f;
        lonCol[0] = 110f;

        ccmadCol[1] = 0.5f;
        latCol[1] = 50f;
        lonCol[1] = 100f;

        ccmadCol[2] = 0.25f;
        latCol[2] = 48f;
        lonCol[2] = 105f;

        ccmadCol[3] = 0.8f;
        latCol[3] = 60f;
        lonCol[3] = 111f;

        ccmadCol[4] = 0.2f;
        latCol[4] = 51f;
        lonCol[4] = 106f;

        ccmadCol[5] = 0.3f;
        latCol[5] = 48.73f;
        lonCol[5] = 104f;

        df = new DataFrame(ccmadCol, latCol, lonCol);
        dO = new DataObj(df);
    }
    */
}
