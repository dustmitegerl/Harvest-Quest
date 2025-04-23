using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Light2D worldLight;
    [SerializeField] private Gradient gradient;
    [SerializeField] private float dayPercentage;
    [SerializeField] float crackOfDawn;
    [SerializeField] float endOfDusk;
    GameTime gameTime;
    void Start()
    {
        gameTime = GameObject.FindWithTag("Game Manager").GetComponent<GameTime>(); 
    }
    void Update()
    {
        if (gameTime == null)
        {
            gameTime = GameObject.FindWithTag("Game Manager").GetComponent<GameTime>();
        }
    }
    void LateUpdate()
    {
        SetLight();
    }
    private void SetLight ()
    {
        dayPercentage = gameTime.GetPercentOfDay();
        if (dayPercentage < endOfDusk && dayPercentage > crackOfDawn)
        {
            worldLight.color = gradient.Evaluate(dayPercentage);
        }
        else worldLight.color = gradient.Evaluate(1);
    }
   
}
