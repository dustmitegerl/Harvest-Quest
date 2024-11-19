using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Combatant
{
    public int phase;
    [SerializeField]
    Ability[] abilities;

    int expAmount = 10;

    void Die()
    {
        ExperienceManager.Instance.AddExperience(expAmount);
    }
}
