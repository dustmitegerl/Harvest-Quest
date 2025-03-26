using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameTime : MonoBehaviour
{
    /// <summary>
    /// prefabs
    /// </summary>
    public TextMeshProUGUI clockRead; // the clock prefab's display text
    public TextMeshProUGUI dayRead; // the day indicator's display text

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
    string AM = "a.m.";
    [SerializeField]
    string PM = "p.m.";
    [SerializeField]
    string currentPeriod;

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
        clockRead = GameObject.Find("Clock").GetComponent<TextMeshProUGUI>(); // find clock
        dayRead = GameObject.Find("Calendar").GetComponent<TextMeshProUGUI>(); // find calendar
    }


    // used for time-fixed rather than frame-fixed methods
    private void FixedUpdate()
    {
        if (!isPaused)
        {
            UpdateGameTime();
            UpdateTimePrefabs();
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
        }
    }

    /// <summary>
    /// updates the clock and days calendar displays
    /// </summary>
    void UpdateTimePrefabs()
    {
        if (clockRead == null) // if no clock
        {
            clockRead = GameObject.Find("Clock").GetComponent<TextMeshProUGUI>(); // find clock
        }
        if (dayRead == null) // if no calendar
        {
            dayRead = GameObject.Find("Calendar").GetComponent<TextMeshProUGUI>(); // find calendar
        }

        int adjHrs = 12; // adjusting for am vs pm; starting at 12 prevents reading "0" at 12pm
        string minsString; // for adding a 0 before single-digit minutes

        if (GameData.hrs >= 12) // 12 oclock
        {
            currentPeriod = PM; // strikes noon
        }
        else currentPeriod = AM; // between midnight and 12 is morning

        // adjusting for AM/PM
        if (currentPeriod == AM) // during A.M. hours
        {
            adjHrs = GameData.hrs; // the hours aren't adjusted
        }
        else if (currentPeriod == PM && GameData.hrs >= 13) // during P.M. hours, after the 12pm hour
        {
            adjHrs = GameData.hrs - 12; // subtract 12 to get the adjusted time
        }

        // in case of 0:00, set to 12AM
        if (GameData.hrs == 0)
        {
            adjHrs = 12;
            currentPeriod = AM;
        }

        if (GameData.mins < 10) // if less than 10 minutes on the clock, s
        {
            minsString = "0" + GameData.mins.ToString();
        }
        else minsString = GameData.mins.ToString(); 

        clockRead.SetText(adjHrs.ToString() + ":" + minsString + currentPeriod); // otherwise just adjust hours and print
        dayRead.SetText("day " + GameData.days.ToString()); // set the day in the calendar object
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