using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ToggleEscMenu : MonoBehaviour
{
    [SerializeField]
    GameObject escMenu;
    [SerializeField]
    GameObject currentCanvas;
    void Start()
    {
    }

    void Update()
    {
        if (escMenu == null)
        {
            FindEscMenu();
        }
    }

    void LateUpdate()
    {
        if (escMenu.activeInHierarchy == false && Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape key pressed");
                FindEscMenu();
                OpenEscMenu();
            }
    }

    // find canvas and esc menu
    public void FindEscMenu()
    {
        currentCanvas = GameObject.FindGameObjectWithTag("Canvas");
        escMenu = currentCanvas.transform.Find("Esc Menu").gameObject;
    }
    public void OpenEscMenu()
    {
        Debug.Log("Opening Esc Menu");
        escMenu.SetActive(true);
    }
}
