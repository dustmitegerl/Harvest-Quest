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

    /// <summary>
    /// clock's visual stuff
    /// </summary>
    [SerializeField]
    string AM = "a.m.";
    [SerializeField]
    string PM = "p.m.";

    [SerializeField]
    int timeSpeedModulator = 1; // used to change speed of seconds
    [SerializeField]
    int secsInMin = 60;
    [SerializeField]
    int minsInHr = 60;
    [SerializeField]
    int hrsInDay = 24;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        hrs = startingHr;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used for time-fixed rather than frame-fixed methods
    private void FixedUpdate()
    {
        UpdateGameTime();
        UpdateTimePrefabs();
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
    /// updates the clock and days calendar displays. 
    /// i intend to replace this part and move it to the actual clock and calendar prefabs, 
    /// leaving this script to only produce the time and then, in those scripts, 
    /// have them read the time from this and render it however they wish.
    /// </summary>
    void UpdateTimePrefabs()
    {
        string currentPeriod;
        int adjHrs = 0; // adjusting for am vs pm
        string minsString; // for adding a 0 before single-digit minutes

        if (hrs >= 12) // 12 oclock
        {
            currentPeriod = PM; // strikes noon
        }
        else currentPeriod = AM; // between midnight and 12 is morning

        if (currentPeriod == AM) // during A.M. hours
        {
            adjHrs = hrs; // the hours aren't adjusted
            
            if (adjHrs >= 12) // when the clock strikes 12
            {
                currentPeriod = PM; // period becomes PM
            }
        }
        else if (currentPeriod == PM) // during P.M. hours
        {
            adjHrs = hrs - 12; // subtract 12 to get the adjusted time

            if (adjHrs >= 12)
            {
                currentPeriod = AM; // period becomes AM
            }
        }

        // in case of 0:00, set to 12AM
        if (hrs == 0)
        {
            adjHrs = 12;
            currentPeriod = AM;
        }

        if (mins < 10) // if less than 10 minutes on the clock, 
        {
            minsString = "0" + mins.ToString();
        }
        else minsString = mins.ToString(); 

        clockRead.SetText(adjHrs.ToString() + ":" + minsString + currentPeriod); // otherwise just adjust hours and print
        dayRead.SetText("day " + days.ToString()); // set the day in the calendar object
    }
}
