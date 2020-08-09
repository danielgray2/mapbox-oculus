using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractWrapper : MonoBehaviour
{
    public abstract void Create();
    public IModel model { get; set; }
}
