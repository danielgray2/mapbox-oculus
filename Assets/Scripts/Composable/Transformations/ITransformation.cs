using System.Collections.Generic;

public abstract class IAbsTransformation : IAbsTransformationAnimator
{
    public IOptions options { get; set; }
    public abstract DataObj ApplyTransformation(DataObj dO);
    public IAbsTransformationAnimator transformationAnimator { get; set; }
    public List<IAbsTransformation> nestedTransformations { get; set; }
    public IAbsTransformation containingTransformation { get; set; }
}
