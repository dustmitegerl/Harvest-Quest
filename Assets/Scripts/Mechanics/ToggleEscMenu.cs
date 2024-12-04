using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ToggleEscMenu : MonoBehaviour
{
    [SerializeField]
    GameObject escMenu;
    void Start()
    {
        escMenu.SetActive(false);
    }

    void LateUpdate()
    {
        if (escMenu.activeInHierarchy == false && Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape key pressed");
                OpenEscMenu();
            }
    }

    public void OpenEscMenu()
    {
        Debug.Log("Opening Esc Menu");
        escMenu.SetActive(true);
    }
}
