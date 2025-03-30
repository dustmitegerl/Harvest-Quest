using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTarget
{
    OneEnemy,     // single player
    AllEnemies,   // all players
    OneAlly,      // single enemy ally (for healing or buffs)
    AllAllies,    // all enemy team
    Self          // self-cast
}

[CreateAssetMenu(menuName = "New Skill")]
public class Skill : ScriptableObject
{
    [Header("Basic Info")]
    public string SkillName;
    [TextArea] public string Description;
    public int SPCost;

    public bool RestoresSP; // If true, this skill gives SP instead of costing it
    public int SPRestoreAmount; // How much SP it restores


    [Header("Effect Type")]
    public bool DealsDamage;
    public int DamageAmount;

    public bool Heals;
    public int HealAmount;

    public SkillTarget TargetType;

    [Header("Extra (Optional)")]
    public float ChanceToDebuff;  // like poison, etc.
    public string AnimationTrigger;
}
