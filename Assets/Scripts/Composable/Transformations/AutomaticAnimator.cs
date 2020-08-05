using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Data.Analysis;

public class AutomaticAnimator : IAbsTransformationAnimator
{
    IAbsTransformationAnimator wrapped;
    public AutomaticAnimator(IAbstractTransformation transformation) : base()
    {
        wrapped = new ManualAnimator(transformation);
        Init(transformation);
    }
    public override DataObj Update(DataObj dO)
    {
        return new DataObj(new DataFrame());
    }
}
