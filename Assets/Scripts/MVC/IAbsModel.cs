
using System;

public abstract class IAbsModel
{
    public Guid gUID { get; protected set; } = Guid.NewGuid();
    public ComposableModel compModel { get; set; }
}