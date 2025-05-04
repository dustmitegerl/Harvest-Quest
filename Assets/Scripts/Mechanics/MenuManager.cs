using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject optionCanvas;
    //[SerializeField] private GameObject saveCanvas;
    [SerializeField] private GameObject quitCanvas;

    [SerializeField] private PlayerMovement player;

    private bool isPaused;

    // Start is called before the first frame update
    private void Start()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        //saveCanvas.SetActive(false);
        quitCanvas.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            if(!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    #region Pause/Unpause Functions

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        player.enabled = false;

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        player.enabled = true;

        CloseAllMenus();
    }

    #endregion

    #region Canvas Activations/Deactivations

    private void OpenMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        //saveCanvas.SetActive(false);
        quitCanvas.SetActive(false);
    }

    private void OpenSettingsHandle()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(true);
        quitCanvas.SetActive(false);
    }

    private void OpenInstructionHandle()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        quitCanvas.SetActive(false);
    }

    private void OpenInventoryHandle()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(true);
        optionCanvas.SetActive(false);
        quitCanvas.SetActive(false);
    }

    private void OpenQuitHandle()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        quitCanvas.SetActive(true);
    }

    private void CloseAllMenus()
    {
        mainMenuCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        //saveCanvas.SetActive(false);
        quitCanvas.SetActive(false);
    }

    #endregion

    #region Main Menu Button Actions

    public void OnSettingsPress()
    {
        OpenSettingsHandle();
    }

    public void OnInstructionsPress()
    {
        OpenInstructionHandle();
    }

    public void OnInventoryPress()
    {
        OpenInventoryHandle();
    }

    public void OnQuitPress()
    {
        OpenQuitHandle();
    }

    public void OnResumePress()
    {
        Unpause();
    }

    #endregion

    #region Settings Menu Button Action

    public void OnSettingsBackPress()
    {
        OpenMainMenu();
    }

    #endregion
}
