using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject menu;

    private void Update()
    {
        HandleMenuToggle();
    }

    public void HandleMenuToggle()
    {
        if (menu.activeSelf && (Input.GetKeyDown(KeyCode.I) || OVRInput.GetDown(OVRInput.RawButton.Y)))
        {
            menu.SetActive(false);
        }
        else if (!menu.activeSelf && (Input.GetKeyDown(KeyCode.I) || OVRInput.GetUp(OVRInput.RawButton.Y)))
        {
            menu.SetActive(true);
        }
    }
}
