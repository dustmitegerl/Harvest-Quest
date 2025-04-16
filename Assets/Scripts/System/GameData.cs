using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks to https://www.youtube.com/watch?v=aUi9aijvpgs&t=1211s
[System.Serializable]
public class GameData {
    public long lastUpdated;
    public int days;
    public int hrs;
    public int mins;
    public float secs;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {

    }
}