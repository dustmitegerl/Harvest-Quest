using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

[CreateAssetMenu(menuName = "New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;

    public int BaseHealth;
    public int BaseStr;
    public int BaseInitiative;

    public int BaseIntelligence;  
    public int BaseDefense;       
    public int BaseResistance;    

    public int BaseSP = 20;  

    public Skill[] Skills;

    public GameObject MemberBattleVisualPrefab;
    public GameObject MemberOverworldVisualPrefab;
}

