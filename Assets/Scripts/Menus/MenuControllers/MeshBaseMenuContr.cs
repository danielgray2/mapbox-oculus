using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBaseMenuContr : IAbsMenuContr
{
    public MeshBaseMenuContr(IAbstractView view, IAbsModel model) : base(view, model) { }

    public override void Update() { }
}
