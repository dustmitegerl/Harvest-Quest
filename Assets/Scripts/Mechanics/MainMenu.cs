using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject instructionCanvas;
    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private GameObject quitCanvas;
    [SerializeField] private GameObject saveCanvas;

    public GameObject pauseFirstButton, instructionFirstButton, optionButton, quitButton, saveButton, instructionClosed, optionClosed, quitClosed, saveClosed;

    private void Start()
    {
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void LoadNewGame()
    {
        GlobalAchievements.ach01Count++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenOption()
    {
        optionCanvas.SetActive(true);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(optionButton);
    } 

    public void CloseOption()
    {
        optionCanvas.SetActive(false);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(optionClosed);
    }

    public void OpenInstruction() 
    { 
        instructionCanvas.SetActive(true);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(instructionFirstButton);


    }

    public void CloseInstruction() 
    { 
        instructionCanvas.SetActive(false);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(instructionClosed);
    }

    public void OpenQuit() 
    { 
        quitCanvas.SetActive(true);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(quitButton);
    }

    public void CloseQuit() 
    { 
        quitCanvas.SetActive(false);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(quitClosed);
    }

    public void OpenSave() 
    { 
        saveCanvas.SetActive(true);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(saveButton);
    }

    public void CloseSave() 
    { 
        saveCanvas.SetActive(false);

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected object
        EventSystem.current.SetSelectedGameObject(saveClosed);
    }

    public void QuitGame()
    {
        Debug.Log("You've exited the game!");
        Application.Quit();
    }
}
