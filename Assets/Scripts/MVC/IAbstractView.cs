using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public abstract class IAbstractView : MonoBehaviour
{
    public IController controller { get; set; }
    public IModel model { get; set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public abstract void Initialize(IModel iModel);
}
