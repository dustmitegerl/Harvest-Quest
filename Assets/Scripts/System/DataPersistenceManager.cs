using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
<<<<<<< Updated upstream
{
=======
{ //Reference: https://m.youtube.com/watch?v=aUi9aijvpgs&pp=0gcJCdgAo7VqN5tD

    private GameData gameData;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found  more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    public void NewGame()
    {
        
    }
    public void LoadGame()
    {

    }

    public void SaveGame()
    {

    }
}
