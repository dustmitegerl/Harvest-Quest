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

    private void OnEnable()
    {
        //Subscribe Event
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        //Unsubscribe from Event
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //what ever you want to level up

        currentExperience = 0;
        maxExperience += 50;
    }

    public void Heal(int amount) 
    {
        currentHP += amount;
        if (currentHP > maxHP) 
        {
            currentHP = maxHP;
        }
    }
}
