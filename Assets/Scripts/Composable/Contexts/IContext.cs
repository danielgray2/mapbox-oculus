﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContext : IComposable
{
    void Generate();
}