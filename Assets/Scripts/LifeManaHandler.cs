using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeManaHandler : MonoBehaviour
{ // reference for the script: https://www.youtube.com/watch?v=WXUsJnupiPQ

    public Slider HPBar;
    public Slider MPBar;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI MPText;

    public float myHP;
    public float myMP;
    private float currentHP;
    private float currentMP;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHP = myHP;
        currentMP = myMP;

        HPBar.maxValue = myHP;
        MPBar.maxValue = myMP;

        HPBar.value = myHP;
        MPBar.value = myMP;
    }

    void Update()
    {

        HPBar.value = currentHP;
        MPBar.value = currentMP;

        HPText.text = "" + (int)currentHP;
        MPText.text = "" + Mathf.FloorToInt(currentMP);
        
        if (currentMP < myMP) 
        {
            currentMP = Mathf.MoveTowards(currentMP, myMP, Time.deltaTime * 0.01f);
        }

        if (currentMP < 0) 
        {   
          currentMP = 0;
        }

        if (currentHP < 0) 
        {
            Debug.Log("Game Over"); 
        }
    }

    public void Damage(float damage) 
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, myHP);
    }

    public void ReduceMP(float mp) 
    {
         currentMP -= mp;
           if (currentMP < 0) 
           {
               currentMP = 0;
           }
    }
}
