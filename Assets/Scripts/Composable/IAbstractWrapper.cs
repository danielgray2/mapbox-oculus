using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractWrapper : MonoBehaviour
{
    public IAbsModel model { get; set; }
    public abstract void Create();
    public abstract void ReRender();
    // TODO: Probably going to want to move this up
    // to IAbsView at some point
    public void HandleModelUpdate()
    {
        ReRender();
    }
}
