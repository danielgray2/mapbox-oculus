
public abstract class IAbsMenuContr : IController
{
    public IAbsModel model { get; set; }
    public IAbstractView view { get; set; }

    public IAbsMenuContr(IAbstractView view, IAbsModel model)
    {
        this.view = view;
        this.model = model;
    }

    public abstract void Update();
}
