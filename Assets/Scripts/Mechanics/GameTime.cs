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
    public int days = 1; // starting days index at 1 rather than 0
    public int hrs;
    public int mins;
    public float secs;
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
        hrs = startingHr;
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

        if (hrs >= 12) // 12 oclock
        {
            currentPeriod = PM; // strikes noon
        }
        else currentPeriod = AM; // between midnight and 12 is morning

        // adjusting for AM/PM
        if (currentPeriod == AM) // during A.M. hours
        {
            adjHrs = hrs; // the hours aren't adjusted
        }
        else if (currentPeriod == PM && hrs >= 13) // during P.M. hours, after the 12pm hour
        {
            adjHrs = hrs - 12; // subtract 12 to get the adjusted time
        }

        // in case of 0:00, set to 12AM
        if (hrs == 0)
        {
            adjHrs = 12;
            currentPeriod = AM;
        }

        if (mins < 10) // if less than 10 minutes on the clock, s
        {
            minsString = "0" + mins.ToString();
        }
        else minsString = mins.ToString(); 

        clockRead.SetText(adjHrs.ToString() + ":" + minsString + currentPeriod); // otherwise just adjust hours and print
        dayRead.SetText("day " + days.ToString()); // set the day in the calendar object
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