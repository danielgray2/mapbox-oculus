using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject thisMenu;
    [SerializeField]
    GameObject menuToNavTo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(menuToNavTo != null)
        {
            IAbstractMenu aM = menuToNavTo.GetComponent<IAbstractMenu>();
            MenuEnum? mE = aM.GetMenuEnum();
            thisMenu.GetComponent<IAbstractMenu>().Transition(mE);
        }
        else
        {
            thisMenu.GetComponent<IAbstractMenu>().Transition(MenuEnum.NONE);
        }
    }
}
