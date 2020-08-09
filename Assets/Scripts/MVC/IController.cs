using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    IModel model { get; set; }
    IAbstractView view { get; set; }
    void Initialize(IModel iModel);
    void Update();
}
