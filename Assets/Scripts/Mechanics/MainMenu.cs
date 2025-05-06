using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject instructionCanvas;

    public void LoadNewGame()
    {
        GlobalAchievements.ach01Count++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenOption()
    {

    } 

    public void CloseOption()
    {

    }

    public void QuitGame()
    {
        Debug.Log("You've exited the game!");
        Application.Quit();
    }
}
