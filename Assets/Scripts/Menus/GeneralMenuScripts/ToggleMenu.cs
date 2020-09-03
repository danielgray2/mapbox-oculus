using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject selectorMenu;
    [SerializeField]
    public GameObject mainMenu;

    private void Update()
    {
        HandleMenuToggle();
    }

    public void HandleMenuToggle()
    {
        if (selectorMenu.activeSelf && Input.GetKeyDown(KeyCode.I) || selectorMenu.activeSelf && OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            selectorMenu.SetActive(false);
        }
        else if (!selectorMenu.activeSelf && Input.GetKeyDown(KeyCode.I) || !selectorMenu.activeSelf && OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            selectorMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else if (mainMenu.activeSelf && Input.GetKeyDown(KeyCode.O) || mainMenu.activeSelf && OVRInput.GetDown(OVRInput.RawButton.X))
        {
            mainMenu.SetActive(false);
        }
        else if (!mainMenu.activeSelf && Input.GetKeyDown(KeyCode.O) || !mainMenu.activeSelf && OVRInput.GetDown(OVRInput.RawButton.X))
        {
            mainMenu.SetActive(true);
            selectorMenu.SetActive(false);
        }
    }
}
