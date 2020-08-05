using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    protected TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    [SerializeField]
    public GameObject menuManagerGO;
    
    public void AddButton(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }
    
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
        int index = button.transform.GetSiblingIndex();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab)
            {
                
            }
        }
    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);   
    }

    public void EnableMenu()
    {
        gameObject.SetActive(true);
    }
}
