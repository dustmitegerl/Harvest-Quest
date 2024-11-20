using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStats : MonoBehaviour
{
    public string combat_class;
    public int attack;
    public int strength;
    public int speed;
    public int health;
    public SpecialAttack[] specialAttacks;
    void Start()
    {
        specialAttacks = GetComponents<SpecialAttack>();
    }
}
