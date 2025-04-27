using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using TMPro.Examples;

public class GameTime : MonoBehaviour, IDataPersistence
{
    /// <summary>
    /// game timekeeping
    /// </summary>
    [SerializeField]
    int startingHr;
    public bool isPaused = false; // for pausing
    public static int days = 1; // starting days index at 1 rather than 0
    public static int hrs;
    public static int mins;
    public static float secs;

    /// <summary>
    /// clock's visual stuff
    /// </summary>


    [SerializeField]
    float timeSpeedModulator = 1; // used to change speed of seconds
    [SerializeField]
    int secsInMin = 60;
    [SerializeField]
    int minsInHr = 60;
    [SerializeField]
    int hrsInDay = 24;

    void Start()
    {
        hrs = startingHr;
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
        secs += Time.fixedDeltaTime * timeSpeedModulator; // multiply time between fixed update by tick

        if (secs >= secsInMin) // using adjustable time ratios
        {
            secs = 0;
            mins += 1;
        }

        if (mins >= minsInHr)
        {
            mins = 0;
            hrs += 1;
        }

        if (hrs >= hrsInDay)
        {
            hrs = 0;
            days += 1;
        }
    }
    /// <summary>
    /// Pause and Unpause
    /// </summary>
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

    /// <summary>
    /// get time totals
    /// </summary>
    public int GetTotalHrs()
    {
        int totalHrs = days * hrsInDay + hrs;
        return totalHrs;
    }
    public int GetTotalMins()
    {
        int totalHrs = GetTotalHrs();
        int totalMins = totalHrs * minsInHr + mins;
        return totalMins;
    }
    public float GetTotalSecs()
    {
        int totalMins = GetTotalMins();
        float totalSecs = totalMins + secsInMin;
        return totalSecs;
    }

    public float GetPercentOfDay()
    {
        int hrOfDay = hrs;
        float minsToday = hrOfDay * minsInHr + mins;
        float minsInDay = minsInHr * hrsInDay;
        float percentOfDay = minsToday / minsInDay;
        return percentOfDay;
    }

    /// <summary>
    /// Load and Save
    /// </summary>
    public void LoadData(GameData data)
    {
        secs = data.secs; 
        mins = data.mins; 
        hrs = data.hrs; 
        days = data.days;
    }
    public void SaveData(ref GameData data)
    {
        data.secs = secs; 
        data.mins = mins; 
        data.hrs = hrs; 
        data.days = days;
    }
}