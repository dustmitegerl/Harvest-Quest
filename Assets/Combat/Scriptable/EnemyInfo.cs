using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;

    public int BaseHealth;
    public int BaseStr;
    public int BaseInitiative;

    public int BaseIntelligence;  
    public int BaseDefense;      
    public int BaseResistance;    

    public int BaseSP = 20; // optional, probably not used much

    public int StartingLevel = 1;

    public GameObject EnemyVisualPrefab;

    public Skill BasicSkill;
    public Skill SpecialSkill;
}
