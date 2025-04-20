using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks to https://www.youtube.com/watch?v=aUi9aijvpgs&t=1211s
[System.Serializable]
public class GameData {

    // Game settings
    public string currentScene;

    // Time data
    public long lastUpdated;
    public int days;
    public int hrs;
    public int mins;
    public float secs;

    // Farm tracking section
    public List<PlantingSpot> plantingSpots;

    // Player info section
    public Vector3 playerPosition;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public void LoadData(GameData data)
    {
        #region Game Settings
        currentScene = data.currentScene;
        #endregion

        #region Time Section
        #endregion

        #region Player Section
        playerPosition = data.playerPosition;
        #endregion

        #region Farming Section
        plantingSpots = data.plantingSpots;
        #endregion
    }
    public void SaveData(ref GameData data)
    {
        #region Game Settings
        data.currentScene = currentScene;
        #endregion

        #region Time Section
        data.secs = secs;
        data.mins = mins;
        data.hrs = hrs;
        data.days = days;
        #endregion

        #region Player Section
        data.playerPosition = playerPosition;
        #endregion

        #region Farming Section
        data.plantingSpots = plantingSpots;
        #endregion
    }
}