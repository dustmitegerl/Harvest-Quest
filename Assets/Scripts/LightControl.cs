using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Light2D worldLight;
    [SerializeField] private Gradient gradient;
    [SerializeField] private float dayPercentage;

    void LateUpdate()
    {
        SetLight();
    }
    private void SetLight ()
    {
        dayPercentage = GameTime.Instance.GetPercentOfDay();
        worldLight.color = gradient.Evaluate(dayPercentage);
    }
   
}
