using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameTime : MonoBehaviour
{
    
    public TextMeshProUGUI clockRead; // the clock prefab's display text
    public TextMeshProUGUI dayRead; // the day indicator's display text
    /// <summary>
    /// clock's visual stuff stuff
    /// </summary>
    public string AM = "a.m.";
    public string PM = "p.m.";
    public int timeSpeedModulator = 1; // used to change speed of seconds
    public int secsInMin = 60;
    public int minsInHr = 60;
    public int hrsInDay = 24;
    public float[] gameTime;
    public float secs;
    public int mins;
    public int hrs;
    public int days = 1; // starting days index at 1 rather than 0
    // Start is called before the first frame update
    void Start()
    {
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
        int adjHrs = 0; // adjusting for am vs pm
        string dayPeriod = AM; // default is a.m.

        if (dayPeriod == AM) // during A.M. hours
        {
            adjHrs = hrs; // the hours aren't adjusted
            
            if (adjHrs >= 12) // when the clock strikes 12
            {
                dayPeriod = PM; // period becomes PM
            }
        }

        else if (dayPeriod == "p.m.") // during P.M. hours
        {
            adjHrs = hrs - 12; // subtract 12 to get the adjusted time

            if (adjHrs >= 12)
            {
                dayPeriod = AM; // period becomes AM
            }
        }

        clockRead.SetText(adjHrs.ToString() + ":" + mins.ToString()); // set the clock to the time, adjusting the hours
        dayRead.SetText("day " + days.ToString()); // set the day in the calendar object
    }
}
