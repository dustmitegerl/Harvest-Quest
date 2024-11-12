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
}
