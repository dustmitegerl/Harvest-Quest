using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Combatant
{
    public int phase;
    [SerializeField]
    Ability[] abilities;

    [SerializeField] int expAmount;

    void Die()
    {
        ExperienceManager.Instance.AddExperience(expAmount);
    }
}
