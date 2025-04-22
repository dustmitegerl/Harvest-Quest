using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameTime : MonoBehaviour
{
    /// <summary>
    /// game timekeeping
    /// </summary>
    [SerializeField]
    int startingHr;
    public bool isPaused = false; // for pausing

    /// <summary>
    /// clock's visual stuff
    /// </summary>
    

    [SerializeField]
    int timeSpeedModulator = 1; // used to change speed of seconds
    [SerializeField]
    int secsInMin = 60;
    [SerializeField]
    int minsInHr = 60;
    [SerializeField]
    int hrsInDay = 24;

    void Start()
    {
        GameData.hrs = startingHr;
        UnPause();
    }


    // used for time-fixed rather than frame-fixed methods
    private void FixedUpdate()
    {
        if (!isPaused)
        {
            UpdateGameTime();
        }
        
    }

    // updates the internal game time, intended for FixedUpdate
    // largely taken from https://pastebin.com/6Yfhy50x
    void UpdateGameTime()
    {
        GameData.secs += Time.fixedDeltaTime * timeSpeedModulator; // multiply time between fixed update by tick

        if (GameData.secs >= secsInMin) // using adjustable time ratios
        {
            GameData.secs = 0;
            GameData.mins += 1;
            GameData.minsElapsed += 1;
        }

        if (GameData.mins >= minsInHr)
        {
            GameData.mins = 0;
            GameData.hrs += 1;
        }

        if (GameData.hrs >= hrsInDay)
        {
            GameData.hrs = 0;
            GameData.days += 1;
            GameData.minsElapsed=0;
        }
    }

    public void Pause()
    {
        isPaused = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false; // stops player movement
    }

    public void UnPause()
    {
        isPaused = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true; // restarts player movement
    }
}