using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManualAnimator : IAbsTransformationAnimator
{
    public ManualAnimator(IAbstractTransformation transformation) : base()
    {
        Init(transformation);
    }
    public override DataObj Update(DataObj dO)
    {
        throw new System.NotImplementedException();
    }
}
