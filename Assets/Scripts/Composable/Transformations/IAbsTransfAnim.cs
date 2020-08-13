using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbsTransfAnim
{
    bool running = false;
    protected IAbsTransf transformation { get; set; }
    public IAbsTransfAnim()
    {
        this.transformation = null;
    }
    
    public void Init(IAbsTransf transformation)
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
