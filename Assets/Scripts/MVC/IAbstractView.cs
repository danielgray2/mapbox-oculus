using UnityEngine;

public abstract class IAbstractView : MonoBehaviour
{
    public IController controller { get; set; }
    public IAbsModel model { get; set; }

    //public abstract void Initialize(IAbsModel iAbsModel);
}
