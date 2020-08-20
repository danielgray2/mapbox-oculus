
using System;
using UnityEngine.Events;

public abstract class IAbsModel
{
    public class ModelUpdateEvent : UnityEvent { }

    public ModelUpdateEvent modelUpdateEvent { get; protected set; } = new ModelUpdateEvent();
    public Guid gUID { get; protected set; } = Guid.NewGuid();
    public string friendlyName { get; protected set; } = "No name provided";
}