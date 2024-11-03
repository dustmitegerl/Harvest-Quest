using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{ 
    // Reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs
    
    public TextMeshProUGUI nameText;

    public Slider hpSlider;
    public Slider spSlider;

    // Creating a function that will use the involed element

    public void SetHUD(Unit unit) 
    {
        if (nameText == null) Debug.LogError("nameText is not assigned.");
        if (hpSlider == null) Debug.LogError("hpSlider is not assigned.");
        if (spSlider == null) Debug.LogError("spSlider is not assigned.");
        if (unit == null) Debug.LogError("unit is null in SetHUD.");

        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        spSlider.maxValue = unit.maxSP;
        spSlider.value = unit.currentSP;
    }

    //Setting the HP Value

    public void SetHP(int hp) 
    {
        hpSlider.value = hp;
    }

    //Setting the SP Value

    public void SetSP(int sp)
    {
        spSlider.value = sp;
    }
}
