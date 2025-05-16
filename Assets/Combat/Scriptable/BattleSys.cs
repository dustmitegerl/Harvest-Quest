using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSys : MonoBehaviour
{

    [SerializeField] private enum BattleState { Start, Selection, Battle, Won, Lost, Run }

    [Header("Battle State")]
    [SerializeField] private BattleState state;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battle Entities")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;
    [SerializeField] private GameObject[] skillButtons;
    [SerializeField] private GameObject skillSelectionMenu; // like the Battle Menu
    [SerializeField] private GameObject allySelectionMenu;
    [SerializeField] private GameObject[] allySelectionButtons;


    //[SerializeField] private Transform skillButtonContainer;
    //[SerializeField] private GameObject skillButtonPrefab;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string ACTION_MESSAGE = "'s Action:";
    private const string WIN_MESSAGE = "You won the battle!";
    private const string LOSE_MESSAGE = "You lost the battle!";
    private const int TURN_DURATION = 2; //turn duration
    void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        enemyManager = EnemyManager.Instance;

        CreatePartyEntities();
        CreateEnemyEntities();

        currentPlayer = 0;
        state = BattleState.Battle;
        ShowBattleMenu();
        GameController.Instance.SetLastScene();
    }


    private IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false); //enemy selection menu disabled
        state = BattleState.Battle; //change state to BattleState
        bottomTextPopUp.SetActive(true); //bottom text pop up enabled

        for (int i = 0; i < allBattlers.Count; i++) //loop through all battlers
        {

            if (state == BattleState.Battle)
            {
                switch (allBattlers[i].BattleAction) //switch statement for the current battlers action
                {
                    case BattleEntities.Action.Attack: //if the action is attack  
                                                       //do the attack   
                        yield return StartCoroutine(AttackRoutine(i)); //wait for the attack to finish
                        break;
                    case BattleEntities.Action.Run: //if the action is run
                        yield return StartCoroutine(RunRoutine()); // Run
                        break;
                    default:
                        Debug.LogError("Error - incorrect battle action");
                        break;
                }
            }
        }
        // if no win or lost repeat the loop by opening battle menu
        if (state == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
        }
        else
        {
            battleMenu.SetActive(false);
            skillSelectionMenu.SetActive(false);
            enemySelectionMenu.SetActive(false);
        }


        yield return null;
    }


    private IEnumerator AttackRoutine(int i)
{
    BattleEntities currAttacker = allBattlers[i];
    if (currAttacker.IsPlayer && currAttacker.ChosenSkill == null)
    {
        Debug.LogWarning(currAttacker.Name + " has no skill selected. Skipping turn.");
        yield break;
    }



    // === PLAYER TURN ===
if (currAttacker.IsPlayer)
{
    Skill skill = currAttacker.ChosenSkill;

    if (skill == null)
    {
        Debug.LogWarning(currAttacker.Name + " has no skill selected. Skipping turn.");
        yield break;
    }

    BattleEntities currTarget = null;

    // Handle targeting based on skill type
    if (skill.TargetType == SkillTarget.OneEnemy || skill.TargetType == SkillTarget.OneAlly)
    {
        if (currAttacker.Target < 0 || currAttacker.Target >= allBattlers.Count)
        {
            int fallbackTarget = (skill.TargetType == SkillTarget.OneEnemy) ? GetRandomEnemy() : GetRandomPartyMember();
            currAttacker.SetTarget(fallbackTarget);
        }

        currTarget = allBattlers[currAttacker.Target];
    }

    AttackAction(currAttacker, currTarget);
    yield return new WaitForSeconds(TURN_DURATION);

    // Handle death only if target exists and died
    if (currTarget != null && currTarget.CurrHealth <= 0)
    {
        allBattlers.Remove(currTarget);
        if (currTarget.IsPlayer)
            playerBattlers.Remove(currTarget);
        else
            enemyBattlers.Remove(currTarget);

        if (currTarget.BattleVisuals != null)
            StartCoroutine(SafeDestroy(currTarget.BattleVisuals));

        bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

        if (enemyBattlers.Count <= 0)
        {
            state = BattleState.Won;
            bottomText.text = WIN_MESSAGE;
            StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
            yield break;
        }

        if (playerBattlers.Count <= 0)
        {
            state = BattleState.Lost;
            bottomText.text = LOSE_MESSAGE;
            StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
            yield break;
        }
    }
}

    // === ENEMY TURN ===
    else
    {
        currAttacker.BattleAction = BattleEntities.Action.Attack;

        Skill chosen = null;
        // Strip number if present to match original EnemyName
        string cleanName = currAttacker.Name.Split(' ')[0];
        Enemy matchingEnemyData = enemyManager.GetCurrentEnemies().Find(e => e.EnemyName == cleanName);


        if (matchingEnemyData != null)
        {
            bool useSpecial = Random.value > 0.7f && matchingEnemyData.SpecialSkill != null;
            chosen = useSpecial ? matchingEnemyData.SpecialSkill : matchingEnemyData.BasicSkill;
        }

        if (chosen == null)
        {
            Debug.LogWarning($"{currAttacker.Name} has no skill assigned. Using fallback.");
            chosen = ScriptableObject.CreateInstance<Skill>();
            chosen.SkillName = "Enemy Claw";
            chosen.DealsDamage = true;
            chosen.DamageAmount = currAttacker.Strength;
            chosen.TargetType = SkillTarget.OneEnemy;
        }

        currAttacker.ChosenSkill = chosen;

        switch (chosen.TargetType)
        {
            case SkillTarget.OneEnemy:
                currAttacker.SetTarget(GetRandomPartyMember());
                break;
            case SkillTarget.AllEnemies:
            case SkillTarget.AllAllies:
            case SkillTarget.Self:
                currAttacker.SetTarget(-1);
                break;
            case SkillTarget.OneAlly:
                currAttacker.SetTarget(GetRandomEnemy());
                break;
        }

        BattleEntities currTarget = currAttacker.Target >= 0 && currAttacker.Target < allBattlers.Count
            ? allBattlers[currAttacker.Target]
            : null;

        AttackAction(currAttacker, currTarget);
        yield return new WaitForSeconds(TURN_DURATION);

        if (currTarget != null && currTarget.IsPlayer && currTarget.CurrHealth <= 0)
        {
            allBattlers.Remove(currTarget);
            playerBattlers.Remove(currTarget);

            if (currTarget.BattleVisuals != null)
                StartCoroutine(SafeDestroy(currTarget.BattleVisuals));

            bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

            if (playerBattlers.Count <= 0)
            {
                state = BattleState.Lost;
                bottomText.text = LOSE_MESSAGE;
                battleMenu.SetActive(false);
                skillSelectionMenu.SetActive(false);
                enemySelectionMenu.SetActive(false);
                StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                yield break;
            }
        }
    }
}




    private IEnumerator RunRoutine()
    {
        if (state == BattleState.Battle)
        {
            if (Random.Range(0, 101) >= 50)
            {
                bottomText.text = "You ran away!";
                state = BattleState.Run;
                allBattlers.Clear();
                yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds
                SceneManager.LoadScene("Farm");
                yield break;
            }
            else
            {
                bottomText.text = "You failed to run away!";
                yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds
                state = BattleState.Battle; // Reset state to Battle if run fails
            }
        }
    }



    //kill enenmy


    //enemies turn



    private void CreatePartyEntities()
{
    List<PartyMember> currentParty = partyManager.GetCurrentParty();

    for (int i = 0; i < currentParty.Count; i++)
    {
        PartyMember member = currentParty[i];

        if (member == null)
        {
            Debug.LogError($"Party member at index {i} is null.");
            continue;
        }

        if (member.MemberBattleVisualPrefab == null)
        {
            Debug.LogError($"Battle visual prefab missing for {member.MemberName}. Skipping.");
            continue;
        }

        if (i >= partySpawnPoints.Length)
        {
            Debug.LogError($"Not enough party spawn points. Index {i} out of range.");
            continue;
        }

        BattleEntities tempEntity = new BattleEntities();

        tempEntity.SetEntityValues(
            member.MemberName,
            member.CurrHealth,
            member.MaxHealth,
            member.Initiative,
            member.Strength,
            member.Intelligence,
            member.Defense,
            member.Resistance,
            member.Level,
            true,
            member.CurrSP,
            member.MaxSP
        );

        GameObject visualObj = Instantiate(
            member.MemberBattleVisualPrefab,
            partySpawnPoints[i].position,
            Quaternion.identity
        );

        BattleVisuals visuals = visualObj.GetComponent<BattleVisuals>();
        if (visuals == null)
        {
            Debug.LogError($"Missing BattleVisuals component on {member.MemberName}'s prefab.");
            continue;
        }

        visuals.SetStartingValues(member.MaxHealth, member.CurrHealth, member.Level);
        tempEntity.BattleVisuals = visuals;

        allBattlers.Add(tempEntity);
        playerBattlers.Add(tempEntity);
    }
}


    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            //  Add numbering to avoid duplicate display names
            string numberedName = currentEnemies[i].EnemyName + " " + (i + 1);

            tempEntity.SetEntityValues(
    numberedName,
    currentEnemies[i].CurrHealth,
    currentEnemies[i].MaxHealth,
    currentEnemies[i].Initiative,
    currentEnemies[i].Strength,
    currentEnemies[i].Intelligence,
    currentEnemies[i].Defense,
    currentEnemies[i].Resistance,
    currentEnemies[i].Level,
    false
);


            BattleVisuals tempBattleVisuals = Instantiate(
                currentEnemies[i].EnemyVisualPrefab,
                enemySpawnPoints[i].position,
                Quaternion.identity
            ).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(
                currentEnemies[i].MaxHealth,
                currentEnemies[i].MaxHealth,
                currentEnemies[i].Level
            );

            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }



    public void ShowBattleMenu()
    {
        // skip dead players
        while (currentPlayer < playerBattlers.Count && playerBattlers[currentPlayer].CurrHealth <= 0)
        {
            currentPlayer++;
        }

        // if no players left, go to enemy turn
        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
            return;
        }

        // don’t open menu unless we’re in battle state
        if (state != BattleState.Battle)
        {
            Debug.LogWarning("Tried to open menu when not in battle");
            return;
        }

        // set the title like "Corn's Action:"
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;

        // grab the current player's actual skill list
        PartyMember memberData = partyManager.GetCurrentParty()[currentPlayer];

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < memberData.Skills.Length)
            {
                Skill skill = memberData.Skills[i];

                // turn on the button and set name
                skillButtons[i].SetActive(true);
                skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = skill.SkillName;

                int index = i; // lock in current index for this button
                Button btn = skillButtons[i].GetComponent<Button>();

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnSkillSelected(skill));

                // hover setup for skill description
                EventTrigger trigger = skillButtons[i].GetComponent<EventTrigger>();
                if (trigger != null)
                {
                    trigger.triggers.Clear();

                    // when we hover, set the description
                    var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                    enter.callback.AddListener((_) =>
                    {
                        //Debug.Log("Hovered on skill: " + skill.SkillName);
                        if (skillDescriptionText != null)
                        {
                            //Debug.Log("Skill Desc Text is working");
                            skillDescriptionText.text = skill.Description;
                            skillDescriptionText.gameObject.SetActive(true); // make sure it's turned on
                        }
                        else
                        {
                            //Debug.LogWarning("skillDescriptionText is NULL");
                        }
                    });

                    // when we stop hovering, clear it (optional)
                    var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                    exit.callback.AddListener((_) =>
                    {
                        skillDescriptionText.text = "";
                        skillDescriptionText.gameObject.SetActive(false);
                    });

                    trigger.triggers.Add(enter);
                    trigger.triggers.Add(exit);
                }
            }
            else
            {
                skillButtons[i].SetActive(false); // button not used for this character
            }
        }

        battleMenu.SetActive(true);
    }






   private void OnSkillSelected(Skill selectedSkill)
{
    skillSelectionMenu.SetActive(false);

    BattleEntities entity = playerBattlers[currentPlayer];
    entity.ChosenSkill = selectedSkill;

    switch (selectedSkill.TargetType)
    {
        case SkillTarget.OneEnemy:
            ShowEnemySelectionMenu();
            break;

        case SkillTarget.OneAlly:
            ShowAllySelectionMenu();
            break;

        default: // e.g. AllAllies, Self
            entity.SetTarget(-1);
            entity.BattleAction = BattleEntities.Action.Attack;
            currentPlayer++;
            if (currentPlayer >= playerBattlers.Count)
                StartCoroutine(BattleRoutine());
            else
                ShowBattleMenu();
            break;
    }
}


 
public void ShowEnemySelectionMenu()
{
    battleMenu.SetActive(false); //disable action menu
    enemySelectionMenu.SetActive(true); //enable selection menu

    SetEnemySelectionButtons();
}

    


   private void SetEnemySelectionButtons()
{
    for (int i = 0; i < enemySelectionButtons.Length; i++)
        enemySelectionButtons[i].SetActive(false);

    for (int j = 0; j < enemyBattlers.Count; j++)
    {
        enemySelectionButtons[j].SetActive(true);
        enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;

        int capturedIndex = j; // 🔒 LOCK THE INDEX

        Button btn = enemySelectionButtons[j].GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => SelectEnemy(capturedIndex));

        EventTrigger trigger = enemySelectionButtons[j].GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = enemySelectionButtons[j].AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((_) =>
        {
            if (enemyBattlers.Count > capturedIndex && enemyBattlers[capturedIndex] != null)
                enemyBattlers[capturedIndex].BattleVisuals.Highlight(true);
        });

        var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((_) =>
        {
            if (enemyBattlers.Count > capturedIndex && enemyBattlers[capturedIndex] != null)
                enemyBattlers[capturedIndex].BattleVisuals.Highlight(false);
        });

        trigger.triggers.Add(enter);
        trigger.triggers.Add(exit);
    }
}



public void SelectAlly(int index)
{
    BattleEntities selected = playerBattlers[index];
    if (selected == null) return;

    BattleEntities currentEntity = playerBattlers[currentPlayer];

    // Set correct index in allBattlers
    int globalIndex = allBattlers.IndexOf(selected);
    currentEntity.SetTarget(globalIndex);

    currentEntity.BattleAction = BattleEntities.Action.Attack;
    currentPlayer++;

    allySelectionMenu.SetActive(false);

    if (currentPlayer >= playerBattlers.Count)
        StartCoroutine(BattleRoutine());
    else
        ShowBattleMenu();
}




   public void SelectEnemy(int currentEnemy)
{
    if (currentEnemy < 0 || currentEnemy >= enemyBattlers.Count)
    {
        Debug.LogError($"Tried to select enemy at invalid index {currentEnemy}. Enemy count: {enemyBattlers.Count}");
        return;
    }

    BattleEntities selectedEnemy = enemyBattlers[currentEnemy];
    if (selectedEnemy == null)
    {
        Debug.LogError("Selected enemy is null.");
        return;
    }

    BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
    if (currentPlayerEntity == null)
    {
        Debug.LogError("Current player entity is null.");
        return;
    }

    currentPlayerEntity.SetTarget(allBattlers.IndexOf(selectedEnemy));
    currentPlayerEntity.BattleAction = BattleEntities.Action.Attack;
    currentPlayer++;

    enemySelectionMenu.SetActive(false);

    if (currentPlayer >= playerBattlers.Count)
        StartCoroutine(BattleRoutine());
    else
        ShowBattleMenu();
}




    private void AttackAction(BattleEntities currAttacker, BattleEntities currTarget)
{
    if (currAttacker == null)
    {
        Debug.LogError("currAttacker is null in AttackAction()");
        return;
    }

    if (currAttacker.ChosenSkill == null)
    {
        Debug.LogError(currAttacker.Name + " has no selected skill in AttackAction()");
        bottomText.text = currAttacker.Name + " skipped their turn (no skill selected).";
        return;
    }

    Skill usedSkill = currAttacker.ChosenSkill;
    string actionLine = currAttacker.Name + " uses " + usedSkill.SkillName;

    // === SP LOGIC ===
    if (usedSkill.RestoresSP)
{
    currAttacker.CurrSP += usedSkill.SPRestoreAmount;
    if (currAttacker.CurrSP > currAttacker.MaxSP)
        currAttacker.CurrSP = currAttacker.MaxSP;

    bottomText.text += $"\n{currAttacker.Name} restores {usedSkill.SPRestoreAmount} SP!";
    currAttacker.BattleVisuals?.ChangeSP(currAttacker.CurrSP, currAttacker.MaxSP);
}

    else if (usedSkill.SPCost > 0)
    {
        currAttacker.CurrSP -= usedSkill.SPCost;
        if (currAttacker.CurrSP < 0)
            currAttacker.CurrSP = 0;

        currAttacker.BattleVisuals?.ChangeSP(currAttacker.CurrSP, currAttacker.MaxSP);
    }

  


    // Animation
    if (currAttacker.BattleVisuals != null)
    {
        if (!string.IsNullOrEmpty(usedSkill.AnimationTrigger))
            currAttacker.BattleVisuals.PlayAnimation(usedSkill.AnimationTrigger);
        else if (usedSkill.SkillName.ToLower().Contains("special"))
            currAttacker.BattleVisuals.PlaySpecialAttackAnimation();
        else
            currAttacker.BattleVisuals.PlayAttackAnimation();
    }

    // === Damage ===
    if (usedSkill.DealsDamage)
    {
        if (usedSkill.TargetType == SkillTarget.AllEnemies)
        {
            List<BattleEntities> toRemove = new();
            foreach (var enemy in enemyBattlers)
            {
                int damage = Mathf.Max(0, usedSkill.DamageAmount + currAttacker.Strength - enemy.Defense);
                enemy.CurrHealth -= damage;
                enemy.UpdateUI();
                bottomText.text = $"{currAttacker.Name} hits {enemy.Name} for {damage}!";

                if (enemy.CurrHealth <= 0)
                    toRemove.Add(enemy);
            }

            foreach (var dead in toRemove)
            {
                allBattlers.Remove(dead);
                enemyBattlers.Remove(dead);
                StartCoroutine(SafeDestroy(dead.BattleVisuals));
            }

            if (enemyBattlers.Count <= 0)
            {
                state = BattleState.Won;
                bottomText.text = WIN_MESSAGE;
                StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                return;
            }
        }
        else if (currTarget != null)
        {
            int damage = Mathf.Max(0, usedSkill.DamageAmount + currAttacker.Strength - currTarget.Defense);
            currTarget.CurrHealth -= damage;
            currTarget.UpdateUI();
            bottomText.text = actionLine + " on " + currTarget.Name + " for " + damage + " damage!";

            if (currTarget.CurrHealth <= 0)
            {
                allBattlers.Remove(currTarget);
                if (!currTarget.IsPlayer) enemyBattlers.Remove(currTarget);
                else playerBattlers.Remove(currTarget);
                StartCoroutine(SafeDestroy(currTarget.BattleVisuals));

                bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                    return;
                }

                if (playerBattlers.Count <= 0)
                {
                    state = BattleState.Lost;
                    bottomText.text = LOSE_MESSAGE;
                    StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                    return;
                }
            }
        }
    }

  // === Heal ===
else if (usedSkill.Heals)
{
    if (usedSkill.TargetType == SkillTarget.AllAllies)
    {
        List<BattleEntities> targets = currAttacker.IsPlayer ? playerBattlers : enemyBattlers;

        bottomText.text = actionLine + " and heals the team!";
        foreach (var ally in targets)
        {
            if (ally == null || ally.CurrHealth <= 0) continue;

            ally.CurrHealth += usedSkill.HealAmount;
            if (ally.CurrHealth > ally.MaxHealth)
                ally.CurrHealth = ally.MaxHealth;

            ally.UpdateUI();
        }
    }
    else
    {
        // Single target heal (like OneAlly or Self)
        BattleEntities healTarget = currTarget ?? currAttacker;

        if (healTarget != null && currAttacker.IsPlayer == healTarget.IsPlayer)
        {
            healTarget.CurrHealth += usedSkill.HealAmount;
            if (healTarget.CurrHealth > healTarget.MaxHealth)
                healTarget.CurrHealth = healTarget.MaxHealth;

            healTarget.UpdateUI();
            bottomText.text = actionLine + " and heals " + healTarget.Name + " for " + usedSkill.HealAmount + " HP!";
        }
        else
        {
            bottomText.text = actionLine + ", but it failed (wrong target).";
        }
    }
}


    // === Buff ===
    else if (usedSkill.AppliesBuff)
{
    BattleEntities buffTarget = currTarget ?? currAttacker;

    if (buffTarget.IsPlayer == currAttacker.IsPlayer)
    {
        // example logic
        switch (usedSkill.TargetStatName.ToLower())
        {
            case "strength":
                buffTarget.Strength += usedSkill.BuffAmount;
                bottomText.text += $"\n{buffTarget.Name}'s Strength increased!";
                break;
            // Add other cases if needed
        }

        buffTarget.UpdateUI();
    }
    else
    {
        bottomText.text += "\nBuff failed (invalid target).";
    }

    return;
}


    // === Debuff ===
    if (usedSkill.AppliesDebuff && currTarget != null)
    {
        switch (usedSkill.TargetStatName.ToLower())
        {
            case "defense":
                currTarget.Defense = Mathf.Max(0, currTarget.Defense - usedSkill.DebuffAmount);
                bottomText.text += $"\n{currTarget.Name}'s Defense dropped!";
                break;
            case "resistance":
                currTarget.Resistance = Mathf.Max(0, currTarget.Resistance - usedSkill.DebuffAmount);
                bottomText.text += $"\n{currTarget.Name}'s Resistance dropped!";
                break;
            case "initiative":
            case "speed":
                currTarget.Initiative = Mathf.Max(0, currTarget.Initiative - usedSkill.DebuffAmount);
                bottomText.text += $"\n{currTarget.Name} slowed down!";
                break;
            default:
                Debug.LogWarning("Unknown debuff stat: " + usedSkill.TargetStatName);
                break;
        }
    }

    // === Nothing ===
    if (!usedSkill.DealsDamage && !usedSkill.Heals && !usedSkill.AppliesDebuff)
    {
        bottomText.text = actionLine + ", but nothing happened.";
    }
}





    private int GetRandomPartyMember()
    {
        List<int> partyMembers = new List<int>(); //create a temp list of party members type int

        for (int i = 0; i < allBattlers.Count; i++) //loop through all player battlers
        {
            if (allBattlers[i].IsPlayer == true) // we have a party member
            {
                partyMembers.Add(i); //add the index to the party members list
            }
        }
        return partyMembers[Random.Range(0, partyMembers.Count)]; //return a random party member index
    }

    private int GetRandomEnemy()
    {
        List<int> enemies = new List<int>(); //create a temp list of enemy members type int

        for (int i = 0; i < allBattlers.Count; i++) //loop through all enemy battlers
        {
            if (allBattlers[i].IsPlayer == false) // we have an enemy member
            {
                enemies.Add(i); //add the index to the enemy members list
            }
        }
        return enemies[Random.Range(0, enemies.Count)]; //return a random enemy member index
    }

    public void SelectRunAction()
    {
        //state = BattleState.Run; // Set the state to Run
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer]; // Local reference for current player

        currentPlayerEntity.BattleAction = BattleEntities.Action.Run; // Setting current member's action to run

        battleMenu.SetActive(false); // Disable battle menu
        currentPlayer++; // Incrementing current player

        if (currentPlayer >= playerBattlers.Count) // If current player is greater than or equal to player battlers count, end the current turn if we went through all players
        {
            StartCoroutine(BattleRoutine());
        }
        else
        {
            enemySelectionMenu.SetActive(false); // Disable enemy selection menu
            ShowBattleMenu(); // Show battle menu
        }
    }

    public void SelectAttackAction()
    {
        battleMenu.SetActive(false);
        skillSelectionMenu.SetActive(true);
        //ShowBattleMenu();
    }

    private IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator SafeDestroy(BattleVisuals visuals, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        if (visuals != null)
            Destroy(visuals.gameObject);
    }

    private void SetAllySelectionButtons()
{
    for (int i = 0; i < allySelectionButtons.Length; i++)
        allySelectionButtons[i].SetActive(false);

   for (int j = 0; j < playerBattlers.Count; j++)
{
    allySelectionButtons[j].SetActive(true);
    allySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = playerBattlers[j].Name;

    int capturedIndex = j;

    Button btn = allySelectionButtons[j].GetComponent<Button>();
    btn.onClick.RemoveAllListeners();
    btn.onClick.AddListener(() => SelectAlly(capturedIndex));

    // Optional hover effect
    EventTrigger trigger = allySelectionButtons[j].GetComponent<EventTrigger>();
    if (trigger == null)
        trigger = allySelectionButtons[j].AddComponent<EventTrigger>();

    trigger.triggers.Clear();

    var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
    enter.callback.AddListener((_) =>
    {
        if (playerBattlers[capturedIndex] != null)
            playerBattlers[capturedIndex].BattleVisuals.Highlight(true);
    });

    var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
    exit.callback.AddListener((_) =>
    {
        if (playerBattlers[capturedIndex] != null)
            playerBattlers[capturedIndex].BattleVisuals.Highlight(false);
    });

    trigger.triggers.Add(enter);
    trigger.triggers.Add(exit);
}

}
public void ShowAllySelectionMenu()
{
    battleMenu.SetActive(false);
    allySelectionMenu.SetActive(true);
    SetAllySelectionButtons();
}





}





[System.Serializable]
public class BattleEntities
{
    public enum Action { Attack, Run }
    public Action BattleAction;

    public string Name;

    public int CurrHealth;
    public int MaxHealth;

    public int Initiative;

    // my offensive stats
    public int Strength;     // for physical damage (e.g. punching)
    public int Intelligence; // for special/magic type skills

    // my defensive stats
    public int Defense;      // reduces physical dmg taken
    public int Resistance;   // reduces special dmg taken

    public int Level;

    public int CurrSP;
    public int MaxSP;

    public bool IsPlayer;

    public Skill ChosenSkill;
    public BattleVisuals BattleVisuals;
    public int Target;

    // updated to handle all the new stats for balancing
    public void SetEntityValues(string name, int currHealth, int maxHealth, int initiative,
                                int strength, int intelligence, int defense, int resistance,
                                int level, bool isPlayer, int currSP = 10, int maxSP = 20)
    {
        Name = name;
        CurrHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strength = strength;
        Intelligence = intelligence;
        Defense = defense;
        Resistance = resistance;
        Level = level;
        IsPlayer = isPlayer;
        CurrSP = currSP;
        MaxSP = maxSP;
    }

    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()
    {
        BattleVisuals.ChangeHealth(CurrHealth);
    }
}












