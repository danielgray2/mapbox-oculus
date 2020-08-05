using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public Dictionary<MenuEnum, IAbstractMenu> menuDictionary = new Dictionary<MenuEnum, IAbstractMenu>();

    protected MenuEnum startMenu = MenuEnum.GENERAL_MENU;

    private void Start()
    {
        List<MenuEnum> menuList = menuDictionary.Keys.ToList();
        foreach (MenuEnum curr in menuList)
        {
            if(curr != startMenu)
            {
                menuDictionary[curr].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateMenu(GameObject gO)
    {
        IAbstractMenu aM = gO.GetComponent<IAbstractMenu>();
        List<MenuEnum> menuList = menuDictionary.Keys.ToList();
        foreach(MenuEnum curr in menuList)
        {
            if(menuDictionary[curr] == aM)
            {
                menuDictionary[curr].gameObject.SetActive(true);
            }
            else
            {
                menuDictionary[curr].gameObject.SetActive(false);
            }
        }
    }

    public void Register(MenuEnum key, IAbstractMenu value)
    {
        menuDictionary.Add(key, value);
    }
}
