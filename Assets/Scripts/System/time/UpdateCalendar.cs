using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCalendar : MonoBehaviour
{
    public TextMeshProUGUI dayRead; // the day indicator's display text

    // Start is called before the first frame update
    void Start()
    {
        dayRead = GetComponent<TextMeshProUGUI>(); // find calendar
    }

    // Update is called once per frame
    void Update()
    {
        if (dayRead == null) // if no calendar
        {
            dayRead = GetComponent<TextMeshProUGUI>(); // find calendar
        }
        dayRead.SetText("day " + GameData.days.ToString()); // set the day in the calendar object
    }
}
