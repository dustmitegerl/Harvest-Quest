using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateClock : MonoBehaviour
{
    public TextMeshProUGUI clockRead; // the clock prefab's display text
    [SerializeField]
    string AM = "a.m.";
    [SerializeField]
    string PM = "p.m.";
    [SerializeField]
    string currentPeriod;
    // Start is called before the first frame update
    void Start()
    {
        clockRead = GetComponent<TextMeshProUGUI>(); // find clock
    }

    // Update is called once per frame
    void Update()
    {
        if (clockRead == null) // if no clock
        {
            clockRead = GetComponent<TextMeshProUGUI>(); // find clock
        }
        int adjHrs = 12; // adjusting for am vs pm; starting at 12 prevents reading "0" at 12pm
        string minsString; // for adding a 0 before single-digit minutes

        if (GameTime.hrs >= 12) // 12 oclock
        {
            currentPeriod = PM; // strikes noon
        }
        else currentPeriod = AM; // between midnight and 12 is morning

        // adjusting for AM/PM
        if (currentPeriod == AM) // during A.M. hours
        {
            adjHrs = GameTime.hrs; // the hours aren't adjusted
        }
        else if (currentPeriod == PM && GameTime.hrs >= 13) // during P.M. hours, after the 12pm hour
        {
            adjHrs = GameTime.hrs - 12; // subtract 12 to get the adjusted time
        }

        // in case of 0:00, set to 12AM
        if (GameTime.hrs == 0)
        {
            adjHrs = 12;
            currentPeriod = AM;
        }

        if (GameTime.mins < 10) // if less than 10 minutes on the clock, s
        {
            minsString = "0" + GameTime.mins.ToString();
        }
        else minsString = GameTime.mins.ToString();

        clockRead.SetText(adjHrs.ToString() + ":" + minsString + currentPeriod); // otherwise just adjust hours and print
    }
}
