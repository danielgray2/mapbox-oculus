using UnityEngine;

public interface IGraph : IComposable
{
    Vector3 minDpSize { get; set; }
    Vector3 maxDpSize { get; set; }
}