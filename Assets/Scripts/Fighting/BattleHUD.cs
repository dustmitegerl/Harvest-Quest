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

    public TextMeshProUGUI HPnumberText;
    public TextMeshProUGUI SPnumberText;

    // To prevent the player from interacting with the HP and SP Bar Sliders
    void Start()
    {
        hpSlider.interactable = false;
        spSlider.interactable = false;
    }

    // Creating a function that will use the involed element

    public void SetHUD(Unit unit) 
    {
    
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        spSlider.maxValue = unit.maxSP;
        spSlider.value = unit.currentSP;

        HPnumberText.text = unit.currentHP.ToString();
        SPnumberText.text = unit.currentSP.ToString();

    }

    //Setting the HP Value

    public void SetHP(int hp) 
    {
        hpSlider.value = hp;
        HPnumberText.text = hp.ToString();
        
    }

    //Setting the SP Value

    public void SetSP(int sp)
    {
        spSlider.value = sp;
        SPnumberText.text = sp.ToString();
    }
}
