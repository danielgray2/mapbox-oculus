
public abstract class IAbstractMenu : IController
{
    public IModel model { get; set; }
    public IAbstractView view { get; set; }

    public IAbstractMenu(IAbstractView view)
    {
        this.view = view;
    }

    public abstract void Transition(IAbstractMenu next);

    public abstract void Initialize(IModel iModel);

    public abstract void Update();
}
