using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Light2D worldLight;
    [SerializeField] private Gradient gradient;
    [SerializeField] private float dayPercentage;
    [SerializeField] float MinutesHolder;
    GameTime gameTime;

    // Start is called before the first frame update
    void Start()
    {
        MinutesHolder = GameData.minsElapsed; //used to check if game has already been started and time has started moving
    }

    // Update is called once per frame
    void Update()
    {
        changeColor();
    }
    private void changeColor() //sets the value of the gradient to the percentage of time in minutes
    {
       /* Debug.Log("hrs is" + GameData.hrs);
        Debug.Log("mins is" + GameData.mins);
        Debug.Log("24f+ is"+ (24f + GameData.minsElapsed)); */

        
        if (MinutesHolder != 0)
        {
            Debug.Log("MinsHolder != 0");
            calculate();

            
        }

        if (MinutesHolder == 0)
        {

            GameData.minsElapsed = 360; //sets minsElapsed to startingHour x minInHour
            MinutesHolder += 1;
            

        }

        
    }

    private void calculate ()
    {
        dayPercentage = GameData.minsElapsed / 1440; //minElapsed is divided by minutes in a hour x hours in a day
       // Debug.Log("dayPercent is" + dayPercentage);
        worldLight.color = gradient.Evaluate(dayPercentage);
    }
   
}
