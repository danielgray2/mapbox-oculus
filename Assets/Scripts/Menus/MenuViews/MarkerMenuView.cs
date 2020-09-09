using UnityEngine;

public class MarkerMenuView : IAbsMenuView
{
    // TODO: Add the glyph menu and glyph prefab in at some point
    [SerializeField]
    GameObject menuHandlerGo;

    [SerializeField]
    GameObject contextMenu;

    protected GameObject next;
    protected MarkerType markerPrefab;

    private void Start()
    {
        Setup(MenuEnum.DATA_MARK_MENU, menuHandlerGo.GetComponent<MenuView>());
    }

    public override void Initialize(IAbsModel iAbsModel) { }

    public void PrepForTransition()
    {
        mV.ActivateMenu(next.GetComponent<IAbsMenuView>());
    }

    public void HandlePointsBtnClicked()
    {
        mV.UpdateMarkerType(MarkerType.POINT);
        next = contextMenu;
        PrepForTransition();
    }
}
