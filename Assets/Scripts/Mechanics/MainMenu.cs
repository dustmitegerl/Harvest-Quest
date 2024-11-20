using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    string mainLevelName;
    public void LoadNewGame()
    {
        SceneManager.LoadScene(mainLevelName);
    }

    public void Close()
    {
        Application.Quit();
    }
}
