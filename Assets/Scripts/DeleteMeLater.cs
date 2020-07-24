using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMeLater : MonoBehaviour
{
    [SerializeField]
    GameObject gO;
    // Start is called before the first frame update
    void Start()
    {
        PrimitiveDataFrameColumn<int> colOne = new PrimitiveDataFrameColumn<int>("colOne", 1);
        colOne[0] = 1;
        DataFrame df = new DataFrame(colOne);

        Color minColor = Color.blue;
        Color maxColor = Color.green;

        gO.GetComponent<DataPoint>().SetData(df.Rows[0]);
        gO.SetActive(false);
        IAnimation scaleAnim = new LerpValueAnimation(IAnimation.AnimateAttrs.SCALE, IAnimation.EndBehav.CALL_ANOTHER, Vector3.zero, Vector3.one);
        IAnimation colorAnim = new LerpColorAnimation(IAnimation.EndBehav.REVERSE, minColor, maxColor);
        IAnimation posAnim = new LerpValueAnimation(IAnimation.AnimateAttrs.POS, IAnimation.EndBehav.REVERSE, Vector3.zero, Vector3.one);
        scaleAnim.nextAnim = colorAnim;
        //scaleAnim.sizeMultiplier = 1f;
        gO.SetActive(true);
        scaleAnim.Activate(gO);
        //posAnim.Activate(gO);
        //colorAnim.Activate(gO);
    }
}
