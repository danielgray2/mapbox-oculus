using Microsoft.Data.Analysis;
using Microsoft.Recognizers.Text.Matcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphCreator : MonoBehaviour
{
    [SerializeField]
    GameObject glyphContPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject glyphContGo = Instantiate(glyphContPrefab);
        TensorGlyphWrapper tG = glyphContGo.GetComponent<TensorGlyphWrapper>();

        DataFrame df = DataFrame.LoadCsv("C:\\Users\\dgray\\data\\l_time.csv");
        DataObj dO = new DataObj(df);

        TensorGlyphModel tGM = new TensorGlyphModel();
        tGM.dataObj = dO;
        tGM.xPosColName = "Latitude";
        tGM.yPosColName = "Depth";
        tGM.zPosColName = "Longitude";
        tGM.axisOneName = "S1";
        tGM.axisTwoName = "S2";
        tGM.axisThreeName = "S3";

        tGM.oneCompAxisOneName = "S1-v-x";
        tGM.oneCompAxisTwoName = "S1-v-y";
        tGM.oneCompAxisThreeName = "S1-v-z";

        tGM.twoCompAxisOneName = "S2-v-x";
        tGM.twoCompAxisTwoName = "S2-v-y";
        tGM.twoCompAxisThreeName = "S2-v-z";

        tGM.threeCompAxisOneName = "S3-v-x";
        tGM.threeCompAxisTwoName = "S3-v-y";
        tGM.threeCompAxisThreeName = "S3-v-z";

        tG.Initialize(tGM);
        tG.Plot();
    }
}
