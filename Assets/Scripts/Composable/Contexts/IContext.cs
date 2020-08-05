using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContext : IComposable
{
    public IOptions options { get; set; }
    void Generate();
}
