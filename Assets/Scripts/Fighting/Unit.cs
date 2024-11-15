using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs

    // Describing the Unit

    public string unitName;
    

    public int damage;

    public int maxHP;
    public int currentHP;

    public int maxSP;
    public int currentSP;

    [SerializeField] int currentExperience, maxExperience, currentLevel;

    //Creating Take Damage

    public bool TakeDamage(int dmg) 
    { 
        currentHP -= dmg;

        if (currentHP < 0) 
        {
            currentHP = 0;
        }

        return currentHP <= 0;

            //return true;
        //else
            //return false;
    }

    // Having enough SP for attack
    public bool HasEnoughSP(int cost) 
    {
        return currentSP >= cost;
    }

    // Regenerating SP 
    public void RegenerateSP(int amount)
    {
        currentSP = Mathf.Min(currentSP + amount, maxSP); 
    }
}
