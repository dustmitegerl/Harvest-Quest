using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health", order = 1)]
public class CharacterStats : ScriptableObject
{ // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data

    public string charName = "name";
    public float[] position = new float[2];
    public GameObject characterGameObject;
    public int level = 1;
    public float maxHP;
    public float maxMP;
    public float hp;
    public float mp;

    
}
