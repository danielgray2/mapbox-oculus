using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    IAbsModel model { get; set; }
    IAbstractView view { get; set; }
    void Update();
}
