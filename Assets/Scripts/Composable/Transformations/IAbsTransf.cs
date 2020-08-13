using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public abstract class IAbsTransf : IAbsTransfAnim
{
    public IAbsTransf() : base()
    {
        Init(this);
    }
    public IAbsTransfAnim aTA { get; set; }
    public List<IAbsTransf> nestedTransformations { get; set; }
    public IAbsTransf containingTransformation { get; set; }
    public abstract DataObj ApplyTransformation(DataObj dO);
}
