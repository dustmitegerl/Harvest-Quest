using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Light2D worldLight;
    [SerializeField] private Gradient gradient;
    [SerializeField] private float dayPercentage;
    GameTime gameTime;

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
        worldLight.color = gradient.Evaluate(dayPercentage);
    }
   
}
