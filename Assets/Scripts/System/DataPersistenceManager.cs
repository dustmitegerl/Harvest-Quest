using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{ //Reference: https://m.youtube.com/watch?v=aUi9aijvpgs&pp=0gcJCdgAo7VqN5tD

    GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found  more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()    
    {
        if(this.gameData == null)
        Debug.Log("No data was found. Initializing data to defaults.");
        NewGame();
    }

    public void SaveGame()
    {

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
