using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    [SerializeField]
    string overworldName = "Interact";

    // pause when opening in overworld
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == overworldName)
        {
            GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameTime>().Pause();
        }
    }

    // unpause when closing

    void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == overworldName)
        {
            GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameTime>().UnPause();
        }
    }
   
    /*void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            QuitGame();
        }
        else if (gameObject.activeInHierarchy == true && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            CloseMenu();
        }
    }*/
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void CloseMenu()
    {
        Debug.Log("Closing " + gameObject.name);
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
