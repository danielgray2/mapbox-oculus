using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMenuContr : IAbsMenuContr
{
    public GenMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update(){}
}
