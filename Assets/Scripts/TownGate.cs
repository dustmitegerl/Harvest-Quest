using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownGate : MonoBehaviour
{
    GameObject playerPrefab;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    string currentScene;
    [SerializeField]
    string otherScene;

    private void Start()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);
        if (currentScene == "Town")
        {
            otherScene = "Farm";
        }
        else if (currentScene == "Farm")
        {
            otherScene = "Town";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("loading level: " + otherScene);
            levelLoader.LoadLevel(otherScene);
        }
    }
}
