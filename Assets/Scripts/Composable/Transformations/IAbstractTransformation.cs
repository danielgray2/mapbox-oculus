using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public abstract class IAbstractTransformation : IAbsTransformationAnimator
{
    public IAbstractTransformation() : base()
    {
        Init(this);
    }
    public IAbsTransformationAnimator aTA { get; set; }
    public List<IAbsTransformation> nestedTransformations { get; set; }
    public IAbstractTransformation containingTransformation { get; set; }
    public abstract DataObj ApplyTransformation(DataObj dO);
}
