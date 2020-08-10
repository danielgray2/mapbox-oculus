﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoxMenuContr : IAbstractMenu
{
    public NewBoxMenuContr(IAbstractView view) : base(view) { }

    public override void Initialize(IModel iModel)
    {
        view.gameObject.SetActive(true);
        view.Initialize(null);
    }

    public override void Transition(IAbstractMenu next)
    {
        view.gameObject.SetActive(false);
        next.Initialize(model);
    }

    public override void Update() { }
}