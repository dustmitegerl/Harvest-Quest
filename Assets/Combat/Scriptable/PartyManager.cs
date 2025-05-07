using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentParty;

    [SerializeField] private PartyMemberInfo defaultPartyMember;

    private void Awake()
    {
        // Clear current party to avoid duplicates on reloads
        currentParty.Clear();

        // Add all members at game start
        foreach (var member in allMembers)
        {
            AddMemberToPartyByName(member.MemberName);
        }
    }

    public void AddMemberToPartyByName(string memberName)
    {
        // Don’t add same member twice
        foreach (var p in currentParty)
        {
            if (p.MemberName == memberName)
                return;
        }

        for (int i = 0; i < allMembers.Length; i++)
        {
            if (allMembers[i].MemberName == memberName)
            {
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrHealth;
                newPartyMember.Strength = allMembers[i].BaseStr;
                newPartyMember.Initiative = allMembers[i].BaseInitiative;
                newPartyMember.CurrExp = 0;
                newPartyMember.MaxExp = 100;
                newPartyMember.Intelligence = allMembers[i].BaseIntelligence;
                newPartyMember.Defense = allMembers[i].BaseDefense;
                newPartyMember.Resistance = allMembers[i].BaseResistance;

                newPartyMember.CurrSP = allMembers[i].BaseSP;
                newPartyMember.MaxSP = allMembers[i].BaseSP;

                newPartyMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;
                newPartyMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;
                newPartyMember.Skills = allMembers[i].Skills;

                currentParty.Add(newPartyMember);
                return;
            }
        }
    }

    public List<PartyMember> GetCurrentParty()
    {
        return currentParty;
    }
}

[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public int Level;

    public int CurrHealth;
    public int MaxHealth;

    public int Strength;
    public int Intelligence;
    public int Defense;
    public int Resistance;

    public int Initiative;

    public int CurrExp;
    public int MaxExp;

    public int CurrSP;
    public int MaxSP;

    public Skill[] Skills;

    public GameObject MemberBattleVisualPrefab;
    public GameObject MemberOverworldVisualPrefab;
}
