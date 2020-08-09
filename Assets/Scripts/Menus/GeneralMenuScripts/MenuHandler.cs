using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public Dictionary<MenuEnum, GameObject> menuDictionary = new Dictionary<MenuEnum, GameObject>();

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
        List<MenuEnum> menuList = menuDictionary.Keys.ToList();
        foreach(MenuEnum curr in menuList)
        {
            if(menuDictionary[curr] == gO)
            {
                menuDictionary[curr].SetActive(true);
            }
            else
            {
                menuDictionary[curr].SetActive(false);
            }
        }
    }

    public void Register(MenuEnum key, GameObject value)
    {
        menuDictionary.Add(key, value);
    }
}
