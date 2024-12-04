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

    // maybe this is silly but i want this to work as soon as AppManager is dropped into a scene in which
    // an Esc Menu has been added to the Canvas
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
