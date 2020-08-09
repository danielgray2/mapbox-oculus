using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsTransformationAnimator
{
    bool running = false;
    protected IAbstractTransformation transformation { get; set; }
    public IAbsTransformationAnimator()
    {
        this.transformation = null;
    }
    
    public void Init(IAbstractTransformation transformation)
    {
        this.transformation = transformation;
    }

    public void Begin()
    {
        running = true;
    }
    public abstract DataObj Update(DataObj dO);
    public void End()
    {
        running = false;
    }
}
